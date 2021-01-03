using System.Collections.Generic;
using System.IO;

namespace IgcRestApi.Services.Interfaces
{
    public interface IFtpService
    {
        public List<string> GetFileList();

        public Stream DownloadFile(string remoteFileName);
    }
}