namespace DotIGC.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using DotIGC.Records;

    [TestClass]
    public class RecordTypeTest
    {
        [TestMethod]
        public void ToString_returns_non_empty_string()
        {
            var description = RecordType.A.ToString();
            Assert.IsNotNull(description);
            Assert.IsTrue(description.Length > 0);
        }
    }
}
