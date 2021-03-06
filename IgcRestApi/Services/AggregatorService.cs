﻿using IgcRestApi.Dto;
using IgcRestApi.Exceptions;
using IgcRestApi.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;

namespace IgcRestApi.Services
{
    public class AggregatorService : IAggregatorService
    {
        private readonly ILogger _logger;
        private readonly IConfigurationService _configuration;
        private readonly IFtpService _ftpService;
        private readonly IFirestoreService _firestoreService;
        private readonly IStorageService _storageService;
        private readonly IIgcReaderService _igcReaderService;
        private readonly INetcoupeService _netcoupeService;

        public AggregatorService(ILoggerFactory loggerFactory,
            IConfigurationService configuration,
            IFtpService ftpService,
            IFirestoreService fireStoreService,
            IStorageService storageService,
            IIgcReaderService igcReaderService,
            INetcoupeService netcoupeService)
        {
            _logger = loggerFactory.CreateLogger<AggregatorService>();
            _configuration = configuration;
            _ftpService = ftpService;
            _firestoreService = fireStoreService;
            _storageService = storageService;
            _igcReaderService = igcReaderService;
            _netcoupeService = netcoupeService;
        }



        /// <summary>
        /// RunAsync
        /// Entry point for the igc extraction and storage
        /// </summary>
        public async Task<IList<string>> RunAsync(int? maxFilesTpProcess = null)
        {
            var lastProcessedFilename = _firestoreService.GetLastProcessedFile();
            var filesList = _ftpService.GetFileList();

            // Remove files already processed from list
            if (!string.IsNullOrEmpty(lastProcessedFilename))
            {
                filesList.RemoveAll(o => isFileAlreadyProcessed(lastProcessedFilename, o));
            }


            // #### Process files ###
            var processedFilesList = new List<string>();
            var processedItemCount = 0;                 // Keep track of processed items # so that we can regularly store the progress
            var totalProcessedItemCount = 0;
            string lastProcessedFileName = null;
            foreach (var f in filesList)
            {
                _logger.LogInformation($"Dealing with: {f}");

                // --- Get file from FTP ---
                var fileStream = _ftpService.DownloadFile(f);
                fileStream.Seek(0, SeekOrigin.Begin);

                // --- Unzip stream file content into stream ---
                var archive = new ZipArchive(fileStream);
                var igcFile = archive.Entries[0];
                var unzippedStream = igcFile.Open();

                // --- Retrieve flight date ---
                var isProcessingDone = false;
                var targetFolderName = "";
                await using (var igcStream = igcFile.Open())
                {
                    try
                    {
                        var igcHeader = _igcReaderService.GetHeaderFromStream(igcStream);
                        targetFolderName = igcHeader.Date.ToString("yyyy_MM_dd") + "/";
                        isProcessingDone = true;
                        lastProcessedFileName = f;
                        igcStream.Close();
                    }
                    catch (Exception e)
                    {
                        _logger.LogDebug($"Could not extract header from file:{e.Message}");
                        isProcessingDone = false;
                    }
                }

                if (isProcessingDone)
                {
                    // --- Store into a GCP bucket
                    _storageService.UploadToBucket(targetFolderName + igcFile.Name, unzippedStream);
                    processedFilesList.Add(f);
                }

                // --- Store progress ---
                totalProcessedItemCount++;
                processedItemCount++;
                if (processedItemCount >= _configuration.StoreProgressInterval)
                {
                    // Store progress in GCP Firestore so that we don't go over all files next time
                    _firestoreService.UpdateLastProcessedFile(f);
                    processedItemCount = 0;
                }

                // Clean up
                unzippedStream.Close();
                await unzippedStream.DisposeAsync();
                fileStream.Close();
                await fileStream.DisposeAsync();

                if (maxFilesTpProcess != null && totalProcessedItemCount == maxFilesTpProcess)
                {
                    break;
                }
            }
            // --- Store last processed file progress
            _firestoreService.UpdateLastProcessedFile(lastProcessedFileName);       // Update last processed file. Null or Empty won't be taken into account

            return processedFilesList;
        }


        /// <summary>
        /// DeleteFlightAsync
        /// </summary>
        /// <param name="flightNumber"></param>
        /// <param name="dryRun"></param>
        public async Task<IgcFlightDto> DeleteFlightAsync(int flightNumber, bool dryRun = false)
        {
            IgcFlightDto flightDto = null;

            if (!dryRun)
            {
                var filename = _netcoupeService.GetIgcFileNameById(flightNumber);
                

                try
                {
                    flightDto = await _storageService.DeleteFileAsync(filename);
                    flightDto.Id = flightNumber;
                }
                catch (FileNotFoundException e)
                {
                    throw new CoreApiException(HttpStatusCode.NotFound, e.Message);
                }
            }
            // --- Dry Run : used for testing purposese
            else
            {
                var filename = "Dry Run:  " + flightNumber;
                if (flightNumber %2==0)
                {
                    flightDto = new IgcFlightDto()
                    {
                        Name = filename,
                        Status = FlightStatus.DELETED
                    };
                }
                else
                {
                    var message = $"[DeleteFlightAsync] - DryRun - Could not find file in GCP bucket: {filename}";
                    _logger.LogDebug(message);
                    throw new CoreApiException(HttpStatusCode.NotFound, message);
                }
               
            }

            return flightDto;
        }

        /// <summary>
        /// isFileAlreadyProcessed
        /// </summary>
        /// <param name="lastProcessedFilename"></param>
        /// <param name="candidateFilename"></param>
        /// <returns>True if the candidate file has already been processed</returns>
        private bool isFileAlreadyProcessed(string lastProcessedFilename, string candidateFilename)
        {
            if (int.TryParse(Path.GetFileNameWithoutExtension(lastProcessedFilename), out var lastProcessedFileNumber))
            {
                if (int.TryParse(Path.GetFileNameWithoutExtension(candidateFilename), out var candidateFileNumber))
                {
                    return candidateFileNumber <= lastProcessedFileNumber;
                }
                else
                {
                    return true;        //  The candidate file is not a number: discard it
                }
            }
            else
            {
                throw new CoreApiException(HttpStatusCode.InternalServerError, $"The last processed file name is not following Netcoupe naming (not a number): {lastProcessedFilename}");
            }
        }




    }
}
