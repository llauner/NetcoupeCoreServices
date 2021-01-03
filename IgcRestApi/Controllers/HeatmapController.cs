using IgcRestApi.DataConversion;
using IgcRestApi.Filters;
using IgcRestApi.Models;
using IgcRestApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace IgcRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiKey]
    public class HeatmapController : ControllerBase
    {
        private readonly ILogger<NetcoupeController> _logger;
        private readonly IConfigurationService _configuration;
        private readonly IDataConverter _dataConverter;
        private readonly IFirestoreService _firestoreService;


        public HeatmapController(ILogger<NetcoupeController> logger,
                                IConfigurationService configuration,
                                IDataConverter dataConverter,
                                IFirestoreService firestoreService)
        {
            _logger = logger;
            _configuration = configuration;
            _dataConverter = dataConverter;
            _firestoreService = firestoreService;
        }


        /// <summary>
        /// Get list of heatmap processed days. List is sorted by date (oldest first)
        /// </summary>
        /// <remarks>
        /// Used by: heatmap.volavoile.net<br/>
        /// Requires: apiKey
        /// </remarks>
        /// <returns></returns>
        [HttpGet("days")]
        public IActionResult GetHeatmapProcessedDays([FromQuery(Name = "includePastYear")] bool includePastYear = true)
        {
            var tracksList = _firestoreService.GetHeatmapBuilderProcessedDays(includePastYear);
            return Ok(new ApiResponseModel(HttpStatusCode.OK, tracksList));
        }


    }
}
