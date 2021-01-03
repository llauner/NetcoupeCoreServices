using IgcRestApi.Services;
using NUnit.Framework;

namespace IgcRestApi.UnitTests
{
    class StorageServiceTests : BaseUnitTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            // Arrange Tests
        }

        /// <summary>
        /// Confirm that we get a list of filenames in a numerical ascending order (not alphabetical)
        /// </summary>
        [Test]
        public void DeleteFileFromStorage_Ok()
        {
            // Arrange
            var storageService = new StorageService(UnitTestLoggerFactory, ConfigurationService);

            // Act 
            storageService.DeleteFileAsync("NetCoupe2020_1345.igc");


            // Assert

        }


    }
}
