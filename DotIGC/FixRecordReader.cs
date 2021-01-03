namespace DotIGC
{
    using System;
    using DotIGC.Records;
    
    public class FixRecordReader : IRecordReader
    {
        Record IRecordReader.Read(string text)
        {
            var recordType = RecordTypeExtension.Parse(text);

            if (recordType != RecordType.B)
                throw new ArgumentException("Wrong record type");

            var hours = TimeSpan.FromHours(double.Parse(text.Substring(1, 2)));
            var minutes = TimeSpan.FromMinutes(double.Parse(text.Substring(3, 2)));
            var seconds = TimeSpan.FromSeconds(double.Parse(text.Substring(5, 2)));

            var timeStamp = hours + minutes + seconds;
            var latitude = ParseLatitude(text.Substring(7, 8));
            var longitude = ParseLongitude(text.Substring(15, 9));
            var validity = text[24] == 'A' ? FixValidity.ThreeDimensions : FixValidity.TwoDimensions;
            var pressureAltitude = int.Parse(text.Substring(25, 5));
            var gnssAltitude = int.Parse(text.Substring(30, 5));
            var additionalData = text.Length > 35 ? text.Substring(35) : string.Empty;

            return new FixRecord(timeStamp, latitude, longitude, pressureAltitude, gnssAltitude, additionalData, validity);
        }

        private double ParseLongitude(string longitude)
        {
            double direction = longitude[8] == 'E' ? 1.0 : -1.0;
            return direction * DegreesMinutesDecimalMinutesToDecimalDegrees(longitude.Substring(0, 3), longitude.Substring(3, 2), longitude.Substring(5, 3));
        }

        private double ParseLatitude(string latitude)
        {
            double direction = latitude[7] == 'N' ? 1.0 : -1.0;
            return direction * DegreesMinutesDecimalMinutesToDecimalDegrees(latitude.Substring(0, 2), latitude.Substring(2, 2), latitude.Substring(4, 3));
        }
 
        public static double DegreesMinutesDecimalMinutesToDecimalDegrees(string degrees, string minutes, string minuteFraction)
        {
            const double oneOver60 = 1 / 60.0;
            double d = double.Parse(degrees);
            double m = double.Parse(minutes);
            double f = double.Parse(minuteFraction) * 0.001;
            return d + (m + f) * oneOver60;
        }
    }
}
