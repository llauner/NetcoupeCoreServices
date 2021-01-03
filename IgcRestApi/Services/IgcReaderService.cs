using DotIGC;
using IgcRestApi.Services.Interfaces;
using System.IO;
using System.IO.Compression;

namespace IgcRestApi.Services
{
    public class IgcReaderService : IIgcReaderService
    {
        public IgcDocumentHeader GetHeader(string filePath)
        {
            var header = IgcDocumentHeader.Load(filePath);
            return header;
        }


        /// <summary>
        /// GetHeaderFromZip
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <returns></returns>
        public IgcDocumentHeader GetHeaderFromZip(string zipFilePath)
        {
            IgcDocumentHeader header = null;

            using (var file = File.OpenRead(zipFilePath))
            using (var zip = new ZipArchive(file, ZipArchiveMode.Read))
            {
                var firstEntry = zip.Entries[0];
                var stream = firstEntry.Open();
                header = IgcDocumentHeader.Load(stream);
            }
            return header;
        }


        /// <summary>
        /// GetHeaderFromStream
        /// </summary>
        /// <param name="igcStream"></param>
        /// <returns></returns>
        public IgcDocumentHeader GetHeaderFromStream(Stream igcStream)
        {
            var header = IgcDocumentHeader.Load(igcStream);
            return header;
        }


    }
}
