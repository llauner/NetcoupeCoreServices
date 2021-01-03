using IgcRestApi.Services;
using NFluent;
using NUnit.Framework;
using System.IO;

namespace IgcRestApi.UnitTests
{
    class FtpClientServiceTests : BaseUnitTest
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
        public void GetFileListFromFtp_Ok()
        {
            // Arrange
            var ftpClient = new FtpService(ConfigurationService);

            // Act 
            var fileList = ftpClient.GetFileList();


            // Assert
            Check.That(fileList).Not.IsEmpty();
            if (fileList.Count >= 2)
                for (int i = 0; i < fileList.Count - 2; i++)
                {
                    var current = fileList[i];
                    var next = fileList[i + 1];

                    var currentValue = int.Parse(Path.GetFileNameWithoutExtension(current));
                    var nextValue = int.Parse(Path.GetFileNameWithoutExtension(next));

                    Check.That(currentValue).IsBefore(nextValue);
                }
        }


    }
}
