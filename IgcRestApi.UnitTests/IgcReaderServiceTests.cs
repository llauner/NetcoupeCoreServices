using IgcRestApi.Services;
using NFluent;
using NUnit.Framework;
using System;

namespace IgcRestApi.UnitTests
{
    public class IgcReaderServiceTests
    {
        [SetUp]
        public void Setup()
        {
            // Method intentionally left empty.
        }

        [Test]
        public void ReadDateFromFile_Ok()
        {
            // Arrange
            var igcReader = new IgcReaderService();

            // Act 
            var header = igcReader.GetHeader("data/06OV89C1.igc");

            // Assert
            Check.That(header.Date).IsEqualTo(new DateTimeOffset(new DateTime(2020, 06, 24)));
        }


        [Test]
        public void ReadFromZipFile_Ok()
        {
            // Arrange
            var igcReader = new IgcReaderService();

            // Act 
            var header = igcReader.GetHeaderFromZip("data/06OV89C1.zip");

            // Assert
            Check.That(header.Date).IsEqualTo(new DateTimeOffset(new DateTime(2020, 06, 24)));
        }


    }
}