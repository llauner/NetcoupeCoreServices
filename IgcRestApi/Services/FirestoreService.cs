using Google.Cloud.Firestore;
using IgcRestApi.Dto;
using IgcRestApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IgcRestApi.Services
{
    public class FirestoreService : IFirestoreService
    {
        private readonly IConfigurationService _configuration;
        private readonly FirestoreDb _firestoreDb = null;

        public FirestoreService(IConfigurationService configuration)
        {
            _configuration = configuration;
            _firestoreDb = FirestoreDb.Create("igcheatmap");
        }


        #region Netcoupe Extractor
        /// <summary>
        /// GetLastProcessedFile
        /// </summary>
        /// <returns></returns>
        public string GetLastProcessedFile()
        {
            var docRef = GetDocumentRef();
            var docSnapshot = docRef.GetSnapshotAsync().Result;
            var docDict = docSnapshot.ToDictionary();
            var lastProcessedFile = (string)docDict.GetValueOrDefault(_configuration.FirestorFieldLastProcessedFile, null);
            return lastProcessedFile;
        }


        /// <summary>
        /// UpdateLastProcessedFile
        /// </summary>
        /// <param name="lastProcessedFilename"></param>
        public void UpdateLastProcessedFile(string lastProcessedFilename)
        {
            if (!string.IsNullOrEmpty(lastProcessedFilename))
            {
                var docRef = GetDocumentRef();
                var updatedDocument = new Dictionary<string, object>
                {
                    { _configuration.FirestorFieldLastProcessedFile, lastProcessedFilename },
                };
                docRef.UpdateAsync(updatedDocument).GetAwaiter().GetResult();
            }

        }


        /// <summary>
        /// DocumentReference
        /// </summary>
        /// <returns></returns>
        private DocumentReference GetDocumentRef()
        {
            var igcRef = _firestoreDb.Collection(_configuration.FirestoreCollectionName);
            var docRef = igcRef.Document(_configuration.FirestoreDocumentName);
            return docRef;
        }
        #endregion

        #region Tracemap Progress

        /// <summary>
        /// GetCumulativeTrackBuilderProcessedDays
        /// List of processed days with data (hash is not null)
        /// </summary>
        /// <returns>Sorted list of processed days</returns>
        public List<string> GetCumulativeTrackBuilderProcessedDays(bool includePastYear)
        {
            var processedDays = new List<string>();
            var pastYearProcessedDays = new List<string>();

            // Current year
            var currentYearProcessedDays = GetProcessedDays(_configuration.FirestoreCollectionNameTracemapProgress, _configuration.FirestoreDocumentNameTracemapProgress);

            if (includePastYear)
            {
                pastYearProcessedDays = GetProcessedDays(_configuration.FirestoreCollectionNameTracemapProgress, _configuration.GetFirestoreDocumentNameTracemapProgress(DateTime.Now.Year - 1));
            }

            processedDays.AddRange(pastYearProcessedDays);
            processedDays.AddRange(currentYearProcessedDays);

            return processedDays;
        }

        public List<CumulativeTracksStatDto> GetCumulativeTrackBuilderStatistics()
        {
            var igcRef = _firestoreDb.Collection(_configuration.FirestoreCollectionNameTracemapProgress);
            var docRef = igcRef.Document(_configuration.FirestoreDocumentNameTracemapProgress);
            var docSnapshot = docRef.GetSnapshotAsync().Result;

            var statisticsDic = docSnapshot.GetValue<Dictionary<string, int>>(_configuration.FirestoreFieldNameStatistics);
            var listStatistics = statisticsDic.Select(e => new CumulativeTracksStatDto(e.Key, e.Value)).ToList();


            listStatistics = listStatistics.OrderBy(f => f.Date.Length).ThenBy(f => f.Date).ToList();
            return listStatistics;
        }

        #endregion

        #region Heatmap
        /// <summary>
        /// GetHeatmapBuilderProcessedDays
        /// List of processed days with data (hash is not null)
        /// </summary>
        /// <returns>Sorted list of processed days</returns>
        public List<string> GetHeatmapBuilderProcessedDays(bool includePastYear)
        {
            var processedDays = new List<string>();
            var pastYearProcessedDays = new List<string>();

            // Current year
            var currentYearProcessedDays = GetProcessedDays(_configuration.FirestoreCollectionNameHeatmapProgress, _configuration.FirestoreDocumentNameHeatmapProgress);

            if (includePastYear)
            {
                pastYearProcessedDays = GetProcessedDays(_configuration.FirestoreCollectionNameHeatmapProgress, _configuration.GetFirestoreDocumentNameHeatmapProgress(DateTime.Now.Year - 1));
            }

            processedDays.AddRange(pastYearProcessedDays);
            processedDays.AddRange(currentYearProcessedDays);

            return processedDays;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progressCollectionName"></param>
        /// <param name="documentName"></param>
        /// <returns></returns>
        private List<string> GetProcessedDays(string progressCollectionName, string documentName)
        {
            var igcRef = _firestoreDb.Collection(progressCollectionName);
            var docRef = igcRef.Document(documentName);
            var docSnapshot = docRef.GetSnapshotAsync().Result;

            var processedDaysDict = docSnapshot.GetValue<Dictionary<string, string>>(_configuration.FirestoreFieldNameProgress);

            var listProcessedDays = processedDaysDict.Where(x => x.Value != null)
                                                    .Select(x => x.Key)
                                                    .ToList();

            listProcessedDays = listProcessedDays.OrderBy(f => f.Length).ThenBy(f => f).ToList();
            return listProcessedDays;
        }

    }
}
