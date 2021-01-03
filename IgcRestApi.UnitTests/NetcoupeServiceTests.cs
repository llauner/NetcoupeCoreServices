using IgcRestApi.Services;
using NFluent;
using NUnit.Framework;
using System.Collections.Generic;

namespace IgcRestApi.UnitTests
{
    class NetcoupeServiceTests : BaseUnitTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            // Arrange Tests
        }

        private static IEnumerable<TestCaseData> NetcoupeIdData
        {
            get
            {
                yield return new TestCaseData(12, 2020, "Netcoupe2020_12.igc");
                yield return new TestCaseData(1234, 2020, "Netcoupe2020_1234.igc");
                yield return new TestCaseData(1234, null, "Netcoupe2020_1234.igc");
                yield return new TestCaseData(23, 2021, "Netcoupe2021_23.igc");
            }
        }



        /// <summary>
        /// GetIgcFileNameById_Ok
        /// Confirm that we can gte the igc filename from a Netcoupe flight Id
        /// </summary>
        /// <param name="flightId"></param>
        /// <param name="currentYear"></param>
        /// <param name="expectedIgcFilename"></param>
        [Test, TestCaseSource(nameof(NetcoupeIdData))]
        public void GetIgcFileNameById_Ok(int flightId, int? currentYear, string expectedIgcFilename)
        {
            // Arrange
            var netcoupeService = new NetcoupeService();

            // Act
            var filename = netcoupeService.GetIgcFileNameById(flightId, currentYear);

            // Assert
            Check.That(filename).IsEqualTo(expectedIgcFilename);

        }

    }
}
