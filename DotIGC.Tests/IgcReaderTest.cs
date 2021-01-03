namespace DotIGC.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using DotIGC.Records;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass]
    public class IgcReaderTest
    {
        [TestMethod]
        public void Read_simple_igc_file_succeeds()
        {
            var path = "Data/Simple.igc";
            var records = new List<Record>();
            
            var container = new Container<RecordType, IRecordReader>();
            container.Bind(RecordType.A).To<FlightRecorderRecordReader>();
            container.Bind(RecordType.H).To<HeaderRecordReader>();
            container.Bind(RecordType.B).To<FixRecordReader>();

            using (var stream = new FileStream(path, FileMode.Open))
            using (var reader = new IgcReader(stream, new RecordReader(container)))
            {
                Record record;
                while (reader.Read(out record))
                    records.Add(record);
            }

            var manufacturerRecord = records.OfType<FlightRecorderRecord>().FirstOrDefault();
            Assert.IsNotNull(manufacturerRecord);
            Assert.AreEqual(manufacturerRecord.Manufacturer, "XXX");

            var fixRecords = records.OfType<FixRecord>().ToList();
            Assert.AreEqual(fixRecords.Count, 9);
        }
    }
}
