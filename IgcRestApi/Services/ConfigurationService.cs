using IgcRestApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace IgcRestApi.Services
{
    public class ConfigurationService : IConfigurationService
    {
        // ########## FTP IGC ##########
        public string FtpNetcoupeIgcHost => _configuration?["FtpNetcoupeIgcHost"];
        public string FtpNetcoupeIgcUsername => _configuration?["FtpNetcoupeIgcUsername"];
        public string FtpNetcoupeIgcPassword => _configuration?["FtpNetcoupeIgcPassword"];

        // ########## GCP ##########
        public string GcpProjectId => GetSetting("GcpProjectId", "igcheatmap");
        public string GcpSecretKeyIgcRestApiInternalApiKey => GetSetting("GcpSecretKeyIgcRestApiInternalApiKey", "igcRestApi-internal-apiKey");
        public string GcpSecretKeyIgcRestApiPubliclApiKey => GetSetting("GcpSecretKeyIgcRestApiPubliclApiKey", "igcRestApi-public-apiKey");

        // ########## Firestore ##########
        public string FirestoreCollectionName => GetSetting("firestoreCollectionName", "igc");
        public string FirestoreDocumentName => GetSetting("firestoreDocumentName", "NetcoupeIgcExtractor");
        public string FirestorFieldLastProcessedFile => GetSetting("firestorFieldLastProcessedFile", "lastProcessedFile");
        public int StoreProgressInterval => GetSetting("StoreProgressInterval", 10);

        public string FirestoreCollectionNameTracemapProgress => GetSetting("FirestoreCollectionNameTracemapProgress", "tracemapProgress");
        public string FirestoreDocumentNameTracemapProgress => GetSetting("FirestoreDocumentNameTracemapProgress", $"{DateTime.Now.Year}_dailyCumulativeTrackBuilder");
        public string GetFirestoreDocumentNameTracemapProgress(int targetYear)=>  $"{targetYear}_dailyCumulativeTrackBuilder";
        

        public string FirestoreCollectionNameHeatmapProgress => GetSetting("FirestoreCollectionNameHeatmapProgress", "heatmapProgress");
        public string FirestoreDocumentNameHeatmapProgress => GetSetting("FirestoreDocumentNameHeatmapProgress", $"{DateTime.Now.Year}_heatmapBuilder");
        public string GetFirestoreDocumentNameHeatmapProgress(int targetYear) => $"{targetYear}_heatmapBuilder";



        public string FirestoreFieldNameProgress => GetSetting("FirestoreFieldNameProgress", "processedDays");
        public string FirestoreFieldNameStatistics => GetSetting("FirestoreFieldNameTracemapProgress", "statistics");

        // ########## Storage Bucket ##########
        public string StorageBucketName => GetSetting("StorageBucketName", "netcoupe-igc-" + DateTime.Now.Year);


        // ######### Default Credentials #########
        public string ApiDefaultLogin => GetSetting<string>("ApiDefaultLogin", null);
        public string ApiDefaultPassword => GetSetting<string>("ApiDefaultPassword", null);




        #region Configuration Service
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// GetSetting
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private T GetSetting<T>(string key, T defaultValue = default(T)) where T : IConvertible
        {
            var val = _configuration?[key];
            if (string.IsNullOrEmpty(val))
                val = Environment.GetEnvironmentVariable(key);

            val = val ?? "";

            T result = defaultValue;
            if (!string.IsNullOrEmpty(val))
            {
                var typeDefault = default(T);
                if (typeof(T) == typeof(string))
                {
                    typeDefault = (T)(object)string.Empty;
                }
                result = (T)Convert.ChangeType(val, typeDefault.GetTypeCode());
            }
            return result;
        }
        #endregion

    }
}
