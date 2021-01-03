namespace DotIGC.Records
{
    using System;
    
    public class FixRecord : Record
    {
        public FixRecord(
            TimeSpan timeUTC, 
            double latitude, 
            double longitude, 
            double pressureAltitude, 
            double gnssAltitude, 
            string additionalData,
            FixValidity validity) 
            : base(RecordType.B)
        {
            TimeUTC = timeUTC;
            Latitude = latitude;
            Longitude = longitude;
            PressureAltitude = pressureAltitude;
            GnssAltitude = gnssAltitude;
            AdditionalData = additionalData;
        }

        public TimeSpan TimeUTC { get; private set; }

        public double Latitude { get; set; }

        public double Longitude { get; private set; }

        public double PressureAltitude { get; private set; }

        public double GnssAltitude { get; private set; }

        public string AdditionalData { get; private set; }

        public FixValidity Validity { get; private set; }

        public override string ToString()
        {
            return string.Format("Timestamp: {0}, Latitude: {1}, Longitude: {2}, Altitude: {3}", TimeUTC, Latitude, Longitude, PressureAltitude);
        }
    }
}
