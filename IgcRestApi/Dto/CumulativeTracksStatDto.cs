using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IgcRestApi.Dto
{
    public class CumulativeTracksStatDto : BaseDto<CumulativeTracksStatDto>
    {
        public string Date { get; set; }
        public int Value { get; set; }

        public CumulativeTracksStatDto(string date, int value)
        {
            Date = date;
            Value = value;
        }

    }
}
