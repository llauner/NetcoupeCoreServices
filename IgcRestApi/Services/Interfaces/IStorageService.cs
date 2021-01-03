using IgcRestApi.Dto;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IgcRestApi.Services.Interfaces
{
    public interface IStorageService
    {
        /// <summary>
        /// UploadToBucket
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="inStream"></param>
        void UploadToBucket(string objectName, Stream inStream);

        Task<IgcFlightDto> DeleteFileAsync(string filename);
        IList<string> GetFilenameList();
    }
}