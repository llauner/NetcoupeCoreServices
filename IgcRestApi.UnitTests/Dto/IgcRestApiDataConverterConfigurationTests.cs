using AutoMapper;
using IgcRestApi.DataConversion;
using IgcRestApi.Exceptions;
using IgcRestApi.Models;
using NFluent;
using NUnit.Framework;
using System.Net;

namespace IgcRestApi.UnitTests.Dto
{
    [TestFixture]
    public class IgcRestApiDataConverterConfigurationTests
    {
        private IDataConverter _dataConverter;

        [OneTimeSetUp]
        public void InitOneTime()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<IgcRestApiMappingProfile>());
            var mapper = mapperConfiguration.CreateMapper();
            _dataConverter = new AutoMapperDataConverter(mapper);
        }

        [Test]
        public void ConfigureMapping_IsValid()
        {
            _dataConverter.AssertConfigurationIsValid();
        }

        [Test]
        public void CoreApiExceptionToCoreApiExceptionModel()
        {
            // Arrange
            var coreApiException = new CoreApiException(HttpStatusCode.NotFound, "The thing was not found");

            // Act
            var coreApiExceptionModel = _dataConverter.Convert<CoreApiExceptionModel>(coreApiException);

            // Assert
            Check.That(coreApiExceptionModel).IsInstanceOf<CoreApiExceptionModel>();
            Check.That(coreApiExceptionModel.DateTime).IsEqualTo(coreApiException.DateTime);
            Check.That(coreApiExceptionModel.StatusCode).IsEqualTo(coreApiException.StatusCode);
            Check.That(coreApiExceptionModel.Message).IsEqualTo(coreApiException.Message);
        }


    }
}
