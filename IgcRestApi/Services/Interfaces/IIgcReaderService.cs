using DotIGC;
using System.IO;

namespace IgcRestApi.Services.Interfaces
{
    public interface IIgcReaderService
    {
        IgcDocumentHeader GetHeader(string filePath);

        /// <summary>
        /// GetHeaderFromZip
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <returns></returns>
        IgcDocumentHeader GetHeaderFromZip(string zipFilePath);

        /// <summary>
        /// GetHeaderFromStream
        /// </summary>
        /// <param name="igcStream"></param>
        /// <returns></returns>
        IgcDocumentHeader GetHeaderFromStream(Stream igcStream);
    }
}