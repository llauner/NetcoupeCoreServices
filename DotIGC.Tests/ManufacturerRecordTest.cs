namespace DotIGC.Tests
{
    using DotIGC.Records;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass]
    public class ManufacturerRecordTest
    {
        [TestMethod]
        public void Can_parse_record_with_variable_flight_record_id_and_optional_text_string()
        {
            var text = "ALXNGIIFLIGHT:1";
            var parser = new FlightRecorderRecordReader();
            var record = parser.Read(text) as FlightRecorderRecord;
            Assert.IsTrue(record.Manufacturer == "LXN");
            Assert.IsTrue(record.Id == "GIIFLIGHT");
            Assert.IsTrue(record.AdditionalData == "1");
        }
    }
}
