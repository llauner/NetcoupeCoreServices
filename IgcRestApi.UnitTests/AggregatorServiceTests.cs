using IgcRestApi.Services;
using IgcRestApi.Services.Interfaces;
using Moq;
using NFluent;
using NUnit.Framework;

namespace IgcRestApi.UnitTests
{
    class AggregatorServiceTests : BaseUnitTest
    {
        private IFirestoreService _firestoreService;
        private IFtpService _ftpService;
        private IStorageService _storageService;
        private IIgcReaderService _igcReaderService;
        private readonly Mock<INetcoupeService> _netcoupeServiceMock = new Mock<INetcoupeService>();


        public AggregatorServiceTests()
        {
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _firestoreService = new FirestoreService(ConfigurationService);
            _ftpService = new FtpService(ConfigurationService);
            _storageService = new StorageService(UnitTestLoggerFactory, ConfigurationService);
            _igcReaderService = new IgcReaderService();

        }


        [Test]
        //[Ignore("Used for dev only")]
        public void Aggregate_Ok()

        {
            // Arrange
            var aggregatorService = new AggregatorService(UnitTestLoggerFactory,
                                                            ConfigurationService,
                                                            _ftpService,
                                                            _firestoreService,
                                                            _storageService,
                                                            _igcReaderService,
                                                            _netcoupeServiceMock.Object);

            // Act
            aggregatorService.RunAsync();

            // Assert
            Check.That(true);

        }

    }
}
