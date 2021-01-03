using IgcRestApi.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IgcRestApi.Services.Interfaces
{
    public interface IAggregatorService
    {
        /// <summary>
        /// RunAsync
        /// Entry point for the igc extraction and storage
        /// </summary>
        Task<IList<string>> RunAsync(int? maxFilesTpProcess = null);

        Task<IgcFlightDto> DeleteFlightAsync(int flightNumber);
    }
}