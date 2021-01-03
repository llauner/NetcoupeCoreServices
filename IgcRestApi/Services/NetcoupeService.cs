using System;
using IgcRestApi.Services.Interfaces;


namespace IgcRestApi.Services
{
    public class NetcoupeService : INetcoupeService
    {
        private static readonly string _netcoupeFileName = "NetCoupe{0}_{1}.igc";

        /// <summary>
        /// GetIgcFileNameById
        /// </summary>
        /// <param name="netcoupeFlightId"></param>
        /// <param name="currentYear"></param>
        /// <returns></returns>
        public string GetIgcFileNameById(int netcoupeFlightId, int? currentYear = null)
        {
            currentYear ??= DateTime.Now.Year;
            var netcoupeIgcFilename = string.Format(_netcoupeFileName, currentYear, netcoupeFlightId);

            return netcoupeIgcFilename;
        }
    }
}
