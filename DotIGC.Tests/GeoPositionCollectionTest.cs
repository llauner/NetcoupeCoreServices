namespace DotIGC.Tests
{
    using System;
    using System.Linq;
    using DotIGC.Records;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GeoPositionCollectionTest
    {
        [TestMethod]
        public void Construction_with_emtpy_list_of_records_succeeds()
        {
            var positions = new GeoPositionCollection(Enumerable.Empty<FixRecord>());
            Assert.AreEqual(positions.Count, 0);
        }

        [TestMethod]
        public void Construction_with_one_record_succeeds()
        {
            var record = new FixRecord(TimeSpan.FromSeconds(5.0), 1, 2, 3, 4, string.Empty, FixValidity.ThreeDimensions);
           
            var positions = new GeoPositionCollection(new[] { record });
            Assert.AreEqual(positions.Count, 1);
            Assert.AreEqual(positions[0].Timestamp, record.TimeUTC);
        }

        [TestMethod]
        public void Construction_with_two_records_succeeds()
        {
            var r1 = new FixRecord(TimeSpan.FromSeconds(0.0), 1, 2, 3, 4, string.Empty, FixValidity.ThreeDimensions);
            var r2 = new FixRecord(TimeSpan.FromSeconds(5.0), 5, 6, 7, 8, string.Empty, FixValidity.ThreeDimensions);

            var positions = new GeoPositionCollection(new[] { r1, r2 });
            Assert.AreEqual(positions.Count, 2);
            Assert.AreEqual(positions[0].Timestamp, r1.TimeUTC);
            Assert.AreEqual(positions[1].Timestamp, r2.TimeUTC);
        }
    }
}
