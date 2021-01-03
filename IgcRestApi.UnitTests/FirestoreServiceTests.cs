using IgcRestApi.Services;
using NFluent;
using NUnit.Framework;

namespace IgcRestApi.UnitTests
{
    class FirestoreServiceTests : BaseUnitTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            // Arrange Tests
        }

        [Test]
        public void GetLastProcesseFileName_Ok()
        {
            // Arrange
            var firestoreService = new FirestoreService(ConfigurationService);

            // Act
            var lastProcessedFileName = firestoreService.GetLastProcessedFile();

            // Assert
            Check.That(lastProcessedFileName).IsNotEmpty();
            Check.That(lastProcessedFileName).IsNotNull();
        }

        [Test]
        public void GetTracemapProcessedFilesList_ok()
        {
            // Arrange
            var firestoreService = new FirestoreService(ConfigurationService);

            // Act
            var processedFilesList = firestoreService.GetCumulativeTrackBuilderProcessedDays(false);

            // Assert
            Check.That(processedFilesList).IsNotNull();
            Check.That(processedFilesList).Not.IsEmpty();

        }



    }
}
