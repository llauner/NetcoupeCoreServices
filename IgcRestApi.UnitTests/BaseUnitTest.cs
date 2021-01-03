using IgcRestApi.Services;
using IgcRestApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace IgcRestApi.UnitTests
{
    internal class BaseUnitTest
    {
        protected static readonly ILoggerFactory UnitTestLoggerFactory = LoggerFactory.Create(builder => builder.AddConsole().AddDebug());

        protected readonly IConfigurationService ConfigurationService = new ConfigurationService(InitConfiguration());

        public BaseUnitTest()
        {
            SetEnvironmentVariables();
        }

        public BaseUnitTest(IConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        public void SetEnvironmentVariables()
        {
            using (var file = File.OpenText("Properties\\launchSettings.json"))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);

                var variables = jObject
                    .GetValue("profiles")
                    //select a proper profile here
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }
        }

        /// <summary>
        /// IConfiguration
        /// Get configuration from json file
        /// </summary>
        /// <returns></returns>
        protected static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            return config;
        }


    }
}
