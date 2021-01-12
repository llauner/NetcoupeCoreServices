using Newtonsoft.Json;
using System;

namespace TraceAggregator.Dto
{
    public class TracksMetadataDto
    {
        public int FlightsCount { get; set; }
        public int ProcessedFlightsCount { get; set; }

        [JsonProperty(PropertyName = "script_end_time")]
        public DateTime ScriptEndTime { get; set; }

        [JsonProperty(PropertyName = "script_start_time")]
        public DateTime ScriptStartTime { get; set; }
        public DateTime TargetDate { get; set; }

        public TracksMetadataDto()
        {
            FlightsCount = 0;
            ProcessedFlightsCount = 0;
            ScriptStartTime = DateTime.UtcNow;
            ScriptEndTime = DateTime.UtcNow;
            TargetDate =  DateTime.UtcNow;
        }
    }
}
