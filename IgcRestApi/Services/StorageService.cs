using Google.Cloud.Storage.V1;
using IgcRestApi.Dto;
using IgcRestApi.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IgcRestApi.Services
{
    public class StorageService : IStorageService
    {
        private readonly ILogger _logger;
        private readonly IConfigurationService _configuration;
        private readonly StorageClient _storageClient;

        public StorageService(ILoggerFactory loggerFactory, IConfigurationService configuration)
        {
            _logger = loggerFactory.CreateLogger<StorageService>();
            _configuration = configuration;
            _storageClient = StorageClient.Create();
        }


        /// <summary>
        /// UploadToBucket
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="inStream"></param>
        public void UploadToBucket(string objectName, Stream inStream)
        {
            _storageClient.UploadObject(_configuration.StorageBucketName, objectName, "text/plain", inStream);
        }


        /// <summary>
        /// DeleteFileAsync
        /// </summary>
        /// <param name="filename"></param>
        public async Task<IgcFlightDto> DeleteFileAsync(string filename)
        {
            var enumerable = _storageClient.ListObjects(_configuration.StorageBucketName);
            var list = enumerable.ToList();
            var fileFullPath = list.SingleOrDefault(o => o.Name.ToLower().Contains(filename.ToLower()));

            if (fileFullPath == null)
            {
                var message = $"Could not find file in GCP bucket: {filename}";
                _logger.LogError(message);
                throw new FileNotFoundException(message);
            }

            await _storageClient.DeleteObjectAsync(_configuration.StorageBucketName, fileFullPath.Name);

            var flightDto = new IgcFlightDto()
            {
                Name = fileFullPath.Name,
                Status = FlightStatus.DELETED
            };

            return flightDto;
        }


        /// <summary>
        /// GetFilenameList
        /// </summary>
        /// <returns></returns>
        public IList<string> GetFilenameList()
        {
            var enumerable = _storageClient.ListObjects(_configuration.StorageBucketName);
            var list = enumerable.ToList();
            var filenameList = list.Select(o => o.Name).ToList();

            return filenameList;
        }


    }
}
