namespace DotIGC
{
    using System;

    public class GeoPosition : IEquatable<GeoPosition>
    {
        TimeSpan timestamp;
        double latitude;
        double longitude;
        double pressureAltitude;
        double gpsAltitude;
        double course;
        double speed;

        public GeoPosition()
        {
            this.timestamp = TimeSpan.MinValue;
            this.latitude = double.NaN;
            this.longitude = double.NaN;
            this.pressureAltitude = double.NaN;
            this.gpsAltitude = double.NaN;
            this.course = double.NaN;
            this.speed = double.NaN;
        }

        public GeoPosition(TimeSpan timestamp, double latitude, double longitude)
            : this(timestamp, latitude, longitude, double.NaN, double.NaN, double.NaN, double.NaN) { }

        public GeoPosition(
            TimeSpan timestamp,
            double latitude,
            double longitude,
            double pressureAltitude,
            double gpsAltitude,
            double course,
            double speed)
        {
            if (latitude > 90 || latitude < -90)
                throw new ArgumentOutOfRangeException("Latitude", "Latitude must be in range -90 to 90");

            if (longitude > 180 || longitude < -180)
                throw new ArgumentOutOfRangeException("Latitude", "Latitude must be in range -180 to 180");

            if (pressureAltitude < 0)
                throw new ArgumentOutOfRangeException("PressureAltitude", "PressureAltitude must be greater than zero");

            if (gpsAltitude < 0)
                throw new ArgumentOutOfRangeException("GpsAltitude", "PressureAltitude must be greater than zero");

            if (course < 0 || course > 360)
                throw new ArgumentOutOfRangeException("Course", "Course must be in range 0 to 360");

            if (speed < 0)
                throw new ArgumentOutOfRangeException("Speed", "Speed must be greater than zero");

            this.timestamp = timestamp;
            this.latitude = latitude;
            this.longitude = longitude;
            this.pressureAltitude = pressureAltitude;
            this.gpsAltitude = gpsAltitude;
            this.course = course;
            this.speed = speed;
        }

        public TimeSpan Timestamp
        {
            get { return this.timestamp; }
            set { this.timestamp = value; }
        }

        public double Latitude
        {
            get { return this.latitude; }

            set
            {
                if (value > 90 || value < -90)
                    throw new ArgumentOutOfRangeException("Latitude", "Latitude must be in range -90 to 90");

                this.latitude = value;
            }
        }

        public double Longitude
        {
            get { return this.longitude; }

            set
            {
                if (value > 180 || value < -180)
                    throw new ArgumentOutOfRangeException("Latitude", "Latitude must be in range -180 to -80");

                this.longitude = value;
            }
        }

        public double PressureAltitude
        {
            get { return pressureAltitude; }

            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("PressureAltitude", "PressureAltitude must be greater than zero");

                this.pressureAltitude = value;
            }
        }

        public double GpsAltitude
        {
            get { return this.gpsAltitude; }

            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("GpsAltitude", "PressureAltitude must be greater than zero");

                this.gpsAltitude = value;
            }
        }

        public double Course
        {
            get { return this.course; }

            set
            {
                if (value < 0 || value > 360)
                    throw new ArgumentOutOfRangeException("Course", "Course must be in range 0 to 360");

                this.course = value;
            }
        }

        public double Speed
        {
            get { return this.speed; }

            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Speed", "Speed must be greater than zero");

                this.speed = value;
            }
        }

        public bool Equals(GeoPosition other)
        {
            if (!object.ReferenceEquals(other, null))
            {
                double latitude = this.Latitude;
                if (!latitude.Equals(other.Latitude))
                {
                    return false;
                }
                else
                {
                    double longitude = this.Longitude;
                    return longitude.Equals(other.Longitude);
                }
            }
            else
            {
                return false;
            }
        }

        public double DistanceTo(GeoPosition other)
        {
            if (double.IsNaN(this.Latitude) || double.IsNaN(this.Longitude) || double.IsNaN(other.Latitude) || double.IsNaN(other.Longitude))
            {
                throw new ArgumentException("Argument_LatitudeOrLongitudeIsNotANumber");
            }
            else
            {
                double latitude = this.Latitude * 0.0174532925199433;
                double longitude = this.Longitude * 0.0174532925199433;
                double num = other.Latitude * 0.0174532925199433;
                double longitude1 = other.Longitude * 0.0174532925199433;
                double num1 = longitude1 - longitude;
                double num2 = num - latitude;
                double num3 = Math.Pow(Math.Sin(num2 / 2), 2) + Math.Cos(latitude) * Math.Cos(num) * Math.Pow(Math.Sin(num1 / 2), 2);
                double num4 = 2 * Math.Atan2(Math.Sqrt(num3), Math.Sqrt(1 - num3));
                double num5 = 6376500 * num4;
                return num5;
            }
        }

        public static double Distance(GeoPosition a, GeoPosition b)
        {
            if (object.ReferenceEquals(a, null))
                throw new ArgumentNullException("a");

            if (object.ReferenceEquals(b, null))
                throw new ArgumentNullException("a");

            return a.DistanceTo(b);
        }

        internal static double Distance(double latitudeA, double longitudeA, double latitudeB, double longitudeB)
        {
            var a = new GeoPosition(TimeSpan.Zero, latitudeA, longitudeA);
            var b = new GeoPosition(TimeSpan.Zero, latitudeB, longitudeB);
            return a.DistanceTo(b);
        }
    }
}
