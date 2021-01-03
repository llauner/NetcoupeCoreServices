using IgcRestApi.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IgcRestApi.Models
{
    public class IgcFlightModel
    {
        public int Id { get; set; }                 // Netcoupe Id
        public string Name { get; set; }            // IGC file name inside Zip file

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ZipFileName { get; set; }     // Zip file name

        [JsonConverter(typeof(StringEnumConverter))]
        public FlightStatus Status { get; set; }
    }
}
