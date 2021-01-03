using IgcRestApi.Dto;
using System.Collections.Generic;

namespace IgcRestApi.Services.Interfaces
{
    public interface IFirestoreService
    {
        /// <summary>
        /// GetLastProcessedFile
        /// </summary>
        /// <returns></returns>
        string GetLastProcessedFile();

        void UpdateLastProcessedFile(string lastProcessedFilename);

        public List<string> GetCumulativeTrackBuilderProcessedDays(bool includePastYear);
        public List<CumulativeTracksStatDto> GetCumulativeTrackBuilderStatistics();


        public List<string> GetHeatmapBuilderProcessedDays(bool includePastYear);
    }
}