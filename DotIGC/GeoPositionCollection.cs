namespace DotIGC
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using DotIGC.Records;
    
    public class GeoPositionCollection : Collection<GeoPosition>
    {
        public GeoPositionCollection(IEnumerable<Record> records)
        {
            if (records.IsEmpty())
                return;
            
            var fixRecords = records.OfType<FixRecord>();
            var first = fixRecords.FirstOrDefault();

            if (null != first)
                Add(new GeoPosition(first.TimeUTC, first.Latitude, first.Longitude, first.PressureAltitude, first.GnssAltitude, 0.0, 0.0));

            fixRecords.Zip(fixRecords.Skip(1), CreateGeoPosition).ForEach(Add);
        }

        GeoPosition CreateGeoPosition(FixRecord a, FixRecord b)
        {
            double seconds = (b.TimeUTC - a.TimeUTC).TotalSeconds;
            double distance = GeoPosition.Distance(a.Latitude, a.Longitude, b.Latitude, b.Longitude);
            double speed = (distance / seconds) * 3.6;

            return new GeoPosition(b.TimeUTC, b.Longitude, b.Latitude, b.PressureAltitude, b.GnssAltitude, double.NaN, speed);
        }      
    }
}
 