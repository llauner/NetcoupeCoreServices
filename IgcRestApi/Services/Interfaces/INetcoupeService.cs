namespace IgcRestApi.Services.Interfaces
{
    public interface INetcoupeService
    {
        /// <summary>
        /// GetIgcFileNameById
        /// </summary>
        /// <param name="netcoupeFlightId"></param>
        /// <param name="currentYear"></param>
        /// <returns></returns>
        string GetIgcFileNameById(int netcoupeFlightId, int? currentYear = null);
    }
}