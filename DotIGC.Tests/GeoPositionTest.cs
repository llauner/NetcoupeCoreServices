namespace DotIGC.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GeoPositionTest
    {
        [TestMethod]
        public void Construction_with_valid_arguments_succueds()
        {
            var expectedTimestamp = TimeSpan.Zero;
            double expectedLatitude = 59.45311;
            double expectedLongitude = 13.34046;
            double expectedPressureAltitude = 100.0;
            double expectedGpsAltitude = 101.0;
            double expectedCourse = 3.45;
            double expectedSpeed = 30.0;

            var coord = new GeoPosition(expectedTimestamp,
                                        expectedLatitude,
                                        expectedLongitude,
                                        expectedPressureAltitude,
                                        expectedGpsAltitude,
                                        expectedCourse,
                                        expectedSpeed);

            Assert.AreEqual(expectedTimestamp, coord.Timestamp);
            Assert.AreEqual(expectedLatitude, coord.Latitude);
            Assert.AreEqual(expectedLongitude, coord.Longitude);
            Assert.AreEqual(expectedPressureAltitude, coord.PressureAltitude);
            Assert.AreEqual(expectedGpsAltitude, coord.GpsAltitude);
            Assert.AreEqual(expectedCourse, coord.Course);
            Assert.AreEqual(expectedSpeed, coord.Speed);
        }

        [TestMethod]
        public void Default_construction_initializes_members_to_nan()
        {
            var expectedTimestamp = TimeSpan.MinValue;
            double expectedLatitude = double.NaN;
            double expectedLongitude = double.NaN;
            double expectedPressureAltitude = double.NaN;
            double expectedGpsAltitude = double.NaN;
            double expectedCourse = double.NaN;
            double expectedSpeed = double.NaN;

            var coord = new GeoPosition();

            Assert.AreEqual(expectedTimestamp, coord.Timestamp);
            Assert.AreEqual(expectedLatitude, coord.Latitude);
            Assert.AreEqual(expectedLongitude, coord.Longitude);
            Assert.AreEqual(expectedPressureAltitude, coord.PressureAltitude);
            Assert.AreEqual(expectedGpsAltitude, coord.GpsAltitude);
            Assert.AreEqual(expectedCourse, coord.Course);
            Assert.AreEqual(expectedSpeed, coord.Speed);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construction_with_latitude_smaller_than_90_throws_execption()
        {
            var expectedTimestamp = TimeSpan.Zero;
            double expectedLatitude = -90.5;
            double expectedLongitude = 13.34046;
            double expectedPressureAltitude = 100.0;
            double expectedGpsAltitude = 101.0;
            double expectedCourse = 3.45;
            double expectedSpeed = 30.0;

            var coord = new GeoPosition(expectedTimestamp,
                                        expectedLatitude,
                                        expectedLongitude,
                                        expectedPressureAltitude,
                                        expectedGpsAltitude,
                                        expectedCourse,
                                        expectedSpeed);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construction_with_latitude_greater_than_90_throws_execption()
        {
            var expectedTimestamp = TimeSpan.Zero;
            double expectedLatitude = 90.5;
            double expectedLongitude = 13.34046;
            double expectedPressureAltitude = 100.0;
            double expectedGpsAltitude = 101.0;
            double expectedCourse = 3.45;
            double expectedSpeed = 30.0;

            var coord = new GeoPosition(expectedTimestamp,
                                        expectedLatitude,
                                        expectedLongitude,
                                        expectedPressureAltitude,
                                        expectedGpsAltitude,
                                        expectedCourse,
                                        expectedSpeed);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construction_with_longitude_lesser_than_180_throws_execption()
        {
            var expectedTimestamp = TimeSpan.Zero;
            double expectedLatitude = 89.0;
            double expectedLongitude = -180.01;
            double expectedPressureAltitude = 100.0;
            double expectedGpsAltitude = 101.0;
            double expectedCourse = 3.45;
            double expectedSpeed = 30.0;

            var coord = new GeoPosition(expectedTimestamp,
                                        expectedLatitude,
                                        expectedLongitude,
                                        expectedPressureAltitude,
                                        expectedGpsAltitude,
                                        expectedCourse,
                                        expectedSpeed);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construction_with_longitude_greater_than_180_throws_execption()
        {
            var expectedTimestamp = TimeSpan.Zero;
            double expectedLatitude = 89.0;
            double expectedLongitude = 180.01;
            double expectedPressureAltitude = 100.0;
            double expectedGpsAltitude = 101.0;
            double expectedCourse = 3.45;
            double expectedSpeed = 30.0;

            var coord = new GeoPosition(expectedTimestamp,
                                        expectedLatitude,
                                        expectedLongitude,
                                        expectedPressureAltitude,
                                        expectedGpsAltitude,
                                        expectedCourse,
                                        expectedSpeed);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construction_with_speed_lesser_than_zero_throws_execption()
        {
            var expectedTimestamp = TimeSpan.Zero;
            double expectedLatitude = -90.5;
            double expectedLongitude = 13.34046;
            double expectedPressureAltitude = 100.0;
            double expectedGpsAltitude = 101.0;
            double expectedCourse = 3.45;
            double expectedSpeed = -0.1;

            var coord = new GeoPosition(expectedTimestamp,
                                        expectedLatitude,
                                        expectedLongitude,
                                        expectedPressureAltitude,
                                        expectedGpsAltitude,
                                        expectedCourse,
                                        expectedSpeed);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construction_with_course_lesser_than_zero_throws_execption()
        {
            var expectedTimestamp = TimeSpan.Zero;
            double expectedLatitude = -89.0;
            double expectedLongitude = 13.34046;
            double expectedPressureAltitude = 100.0;
            double expectedGpsAltitude = 101.0;
            double expectedCourse = -3.45;
            double expectedSpeed = 0.1;

            var coord = new GeoPosition(expectedTimestamp,
                                        expectedLatitude,
                                        expectedLongitude,
                                        expectedPressureAltitude,
                                        expectedGpsAltitude,
                                        expectedCourse,
                                        expectedSpeed);
        }
    }
}
