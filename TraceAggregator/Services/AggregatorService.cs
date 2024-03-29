﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceAggregator.Dto;
using TraceAggregator.Helper;
using TraceAggregator.Services.Interfaces;

namespace TraceAggregator.Services
{
    public class AggregatorService : IAggregatorService
    {
        private readonly ILogger _logger;
        private readonly IConfigurationService _configurationService;
        private readonly IZipStorageService _zipStorageService;
        private readonly IStorageService _storageService;

        private static readonly string YearlyTracksFilename = $"{DateTime.Now.Year}-tracks.geojson";
        private static readonly string YearlyTracksZipFilename = $"{DateTime.Now.Year}-tracks.geojson.zip";
        private static readonly string YearlyTtrackMetadataFilename = $"{DateTime.Now.Year}-tracks-metadata.json";

        public const short DefaultCoordinatesReductionFactor = 50;

        public AggregatorService(ILogger<AggregatorService> logger, 
                                IConfigurationService configurationService,
                                IZipStorageService zipStorageService,
                                IStorageService storageService)
        {
            _logger = logger;
            _configurationService = configurationService;
            _zipStorageService = zipStorageService;
            _storageService = storageService;
        }

        public static string StreamToString(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }


        /// <summary>
        /// Run
        /// Default Aggregation entry point: process backlog and integrated reduced daily files into yearly cumulative tracks file.
        /// </summary>
        /// <param name="keepBacklog">Set to true to preserve backlog (files will not be deleted once processed)</param>
        /// <returns></returns>
        public async Task Run(float? reductionFactor = AggregatorService.DefaultCoordinatesReductionFactor,
                                bool keepBacklog=false)
        {
            _logger.LogInformation("######### [AggregatorService] Building yearly cumulative track ... ###########");
            var appliedReductionFactor = reductionFactor.HasValue ? reductionFactor.Value : AggregatorService.DefaultCoordinatesReductionFactor;

            // --- List files to be processed in backlog ---
            var backlogFilesList = _storageService.GetFilenameList("backlog");
            backlogFilesList = backlogFilesList.Where(f => Path.GetExtension(f) == ".zip").ToList();     // Keep zip files only

            if (backlogFilesList.Count > 0)
            {
                // --- Get yearly geojson
                _logger.LogInformation($"Downloading zip file from bucket: {_configurationService.TraceAggregatorBucketName} / {YearlyTracksZipFilename}");

                // --- Get yearly geojson
                GeoJsonDto yearGeojson = null;
                List<string> yearFlightIdList = null;   // FlightIds in the yearly aggregation
                try
                {
                    var yearlyGeojsonFile = await _zipStorageService.DownloadZipedFileAsStringAsync(YearlyTracksZipFilename);
                    yearGeojson = JsonHelper.Deserialize<GeoJsonDto>(yearlyGeojsonFile);
                    yearFlightIdList = yearGeojson.features.Select(f => f.properties.flightId).ToList();
                }
                catch (Google.GoogleApiException e)
                {
                    _logger.LogWarning($"[AggregatorService]: {e.Message}");
                    yearGeojson = new GeoJsonFeatureCollectionDto();
                    yearFlightIdList = new List<string>();
                }

                // -- Get yearly metadata
                TracksMetadataDto metadataDto = null;
                try
                {
                    var metadatajson = await _storageService.DownloadObjectFromBucketAsStringAsync(YearlyTtrackMetadataFilename);
                    metadataDto = JsonHelper.Deserialize<TracksMetadataDto>(metadatajson);
                }
                catch (Google.GoogleApiException e)
                {
                    _logger.LogWarning($"[AggregatorService]: {e.Message}");
                    metadataDto = new TracksMetadataDto();
                }

                // --- Process files ---
                var currentFileCount = 0;
                var totalFileCount = backlogFilesList.Count;
                var addedFeatureCount = 0;

                foreach (var filename in backlogFilesList)
                {
                    // --- Get daily geojson
                    _logger.LogInformation($"{currentFileCount}/{totalFileCount} Integrating file into yearly tracks: {filename}");
                    currentFileCount++;
                    
                    var tracksForDayAsJson = await _zipStorageService.DownloadZipedFileAsStringAsync(filename);
                    var dayGeojson = JsonHelper.Deserialize<GeoJsonDto>(tracksForDayAsJson);

                    // Get rid of features that are already present in the yearly aggregation
                    //var newFeatures = dayGeojson.features.Where(f => !yearFlightIdList.Contains(f.properties.flightId)).ToList();

                    var newFeatures = dayGeojson.features;

                    if (newFeatures.Count>0)
                    {
                        _logger.LogInformation($"Adding new feature to the aggregated file. New features count= {newFeatures.Count}");
                        dayGeojson.features = newFeatures;
                        addedFeatureCount += newFeatures.Count;

                        //--- Reduce number of features
                        ReduceGeojsonFeatures(ref dayGeojson, appliedReductionFactor);
                        yearGeojson.features.AddRange(dayGeojson.features);                     // Add the features to the yearly aggregation

                        // --- Crop to eliminate points outside of the bounding box ---
                        CropGeoJsonFeatures(ref dayGeojson);
                    }
                    // --- Delete the processed file from the backlog
                    if (!keepBacklog)
                    {
                        await _storageService.DeleteFileAsync(filename);
                    }

                }

                if (addedFeatureCount >0)
                {
                    // --- Update metadata ---
                    metadataDto.ScriptEndTime = DateTime.UtcNow;
                    metadataDto.TargetDate = DateTime.UtcNow;
                    metadataDto.FlightsCount += addedFeatureCount;
                    metadataDto.ProcessedFlightsCount += addedFeatureCount;

                    // --- Store yearly geojson file with new feature added ---
                    _logger.LogInformation("Storing new yearly tracemap into bucket ...");
                    var yearlyGeojsonText = JsonHelper.Serialize(yearGeojson);
                    await _zipStorageService.UploadStringToZipFileAsync(yearlyGeojsonText, YearlyTracksFilename, YearlyTracksZipFilename);   // Store into a GCP bucket: geojson

                    // --- Store updated metadata ---
                    _logger.LogInformation("Storing updated metadata file into bucket ...");
                    var metadataText = JsonHelper.Serialize(metadataDto);
                    await _storageService.UploadToBucketAsync(YearlyTtrackMetadataFilename, metadataText);
                }
                else
                {
                    _logger.LogInformation("No new feature to add.");
                }
                _logger.LogInformation("######### [AggregatorService] ######### Done !");

            }
            else
            {
                _logger.LogInformation("######### [AggregatorService] ######### No file to process in the backlog. Done !");
            }
        }


        /// <summary>
        /// ReduceCumulativeTracksZipFile
        /// Reduce the features in an existing .geojson.zip file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="reductionFactor"></param>
        /// <returns></returns>
        public async Task ReduceCumulativeTracksZipFile(string filename= null, 
                                                        float? reductionFactor = AggregatorService.DefaultCoordinatesReductionFactor, 
                                                        bool doBackup=true)
        {
            filename= string.IsNullOrEmpty(filename)? YearlyTracksZipFilename : filename;
            var appliedReductionFactor = reductionFactor.HasValue ? reductionFactor.Value : AggregatorService.DefaultCoordinatesReductionFactor;

            _logger.LogInformation($"######### [AggregatorService] ######### Reducing .geojson.zip file: {filename} / {reductionFactor}");

            // --- Get file as Geojson ---
            var targetGeojsonFile = await _zipStorageService.DownloadZipedFileAsStringAsync(filename);
            var targetGeojson = JsonConvert.DeserializeObject<GeoJsonDto>(targetGeojsonFile);

            // Make a backup if needed
            if (doBackup)
            {
                var backupFilename = $"{DateTime.Now.ToString("yyyy_MM_dd")}#{filename}";
                var backupEntryName = filename.Replace(".zip", "");
                await _zipStorageService.UploadStringToZipFileAsync(targetGeojsonFile, backupEntryName, backupFilename);
                _logger.LogInformation($"Bakcup of file made: {filename} -> {backupFilename}");
            }

            // --- Reduce Features ----
            ReduceGeojsonFeatures(ref targetGeojson, appliedReductionFactor);

            // --- Crop to eliminate points outside of the bounding box ---
            CropGeoJsonFeatures(ref targetGeojson);

            // --- Write result ---
            _logger.LogInformation($"Storing reduced file into bucket ...");
            var geojsonText = JsonConvert.SerializeObject(targetGeojson);
            await _zipStorageService.UploadStringToZipFileAsync(geojsonText, filename.Replace(".zip",""), filename);    // Store into a GCP bucket: geojson

            _logger.LogInformation("######### [AggregatorService] ######### Done !");
        }


        /// <summary>
        /// ReduceGeojsonFeatures
        /// </summary>
        /// <param name="sourceGeojson"></param>
        /// <param name="reductionFactor"></param>
        private void ReduceGeojsonFeatures(ref GeoJsonDto sourceGeojson, float reductionFactor= AggregatorService.DefaultCoordinatesReductionFactor)
        {
            //--- Process daily file: reduce the number of features
            var totalInitialCoordinatesCount = 0;
            var totalReducedCoordinatesCount = 0;
            foreach (var f in sourceGeojson.features)
            {
                // Reduce the number of coordinates per feature
                totalInitialCoordinatesCount += f.geometry.coordinates.Count;
                var reducedCoordiantes = f.geometry.coordinates.Where((_, i) => i % reductionFactor == 0).ToList();
                f.geometry.coordinates = reducedCoordiantes;
                totalReducedCoordinatesCount += reducedCoordiantes.Count;
            }
            _logger.LogInformation($"Reduced Coordinates: {totalReducedCoordinatesCount} = {totalInitialCoordinatesCount} / {reductionFactor}");
        }


        private void CropGeoJsonFeatures(ref GeoJsonDto sourceGeojson)
        {
            //--- Process daily file: reduce the number of features
            var totalInitialCoordinatesCount = 0;
            var totalReducedCoordinatesCount = 0;
            foreach (var f in sourceGeojson.features)
            {
                // Reduce the number of coordinates per feature
                totalInitialCoordinatesCount += f.geometry.coordinates.Count;

                var reducedCoordiantes = f.geometry.coordinates.Where((c, i) => IsInsideBoundingBox(c[0], c[1])).ToList();
                f.geometry.coordinates = reducedCoordiantes;
                totalReducedCoordinatesCount += reducedCoordiantes.Count;
            }
            _logger.LogInformation($"Remove out of the box coordinates: {totalReducedCoordinatesCount} = {totalInitialCoordinatesCount}");
        }


        /// <summary>
        /// IsInsideBoundingBox
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        private bool IsInsideBoundingBox(double lng, double lat)
        {
            var minX = -9.843750;
            var minY = 38.856820;
            var maxX = 10.107422;
            var maxY = 51.371780;

            if (lng < minX || lng > maxX || lat < minY || lat > maxY)
            {
                // We're outside the polygon!
                return false;
            }
            return true;
        }

    }


   
}
