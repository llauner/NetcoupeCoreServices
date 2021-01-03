using IgcRestApi.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace IgcRestApi.Common.Helper
{
    /// <summary>
    /// Helper class for Json handling
    /// </summary>
    public static class JsonHelper
    {
        public static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                },
                Formatting = Formatting.Indented
            };
        }

        /// <summary>
        /// Deserialize a string into an object with standard settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string jsonString)
        {
            T jsonObject;
            try
            {
                jsonObject = JsonConvert.DeserializeObject<T>(jsonString, GetJsonSerializerSettings());
            }
            catch (Exception e)
            {
                throw new CoreException(e.Message);
            }

            return jsonObject;
        }

        /// <summary>
        /// Deserializes the specified json string.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="CoreException"></exception>
        public static dynamic Deserialize(string jsonString, Type type)
        {
            dynamic jsonObject;
            try
            {
                jsonObject = JsonConvert.DeserializeObject(jsonString, type, GetJsonSerializerSettings());
            }
            catch (Exception e)
            {
                throw new CoreException(e.Message);
            }

            return jsonObject;
        }

        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        public static string Serialize(object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize, GetJsonSerializerSettings());
        }
    }
}
