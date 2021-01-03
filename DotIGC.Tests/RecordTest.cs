namespace DotIGC.Tests
{
    using DotIGC.Records;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass]
    public class RecordTest
    {
        [TestMethod]
        public void New_instance_with_record_type_succeeds()
        {
            var record = new Record(RecordType.A);
            Assert.IsTrue(record.RecordType == RecordType.A);
        }

        [TestMethod]
        public void New_instance_with_args_succeeds()
        {
            var exptectedText = "ExpectedText";
            var record = new Record(RecordType.A, exptectedText);
            
            Assert.IsTrue(record.RecordType == RecordType.A);            
            Assert.AreEqual(record.Text, exptectedText);
        }
    }
}
