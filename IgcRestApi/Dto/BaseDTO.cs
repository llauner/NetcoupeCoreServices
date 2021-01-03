using IgcRestApi.Common.Helper;
using IgcRestApi.Exceptions;
using Newtonsoft.Json;
using System;

namespace IgcRestApi.Dto
{
    /// <summary>
    /// Base DTO Class
    /// Provides Json serialize and deserialize capabilities
    /// </summary>
    public abstract class BaseDto<T>
    {
        /// <summary>
        /// Deserialize a string into an object with standard settings
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns>T</returns>
        public static T Deserialize(string jsonString)
        {
            try
            {
                return JsonHelper.Deserialize<T>(jsonString);
            }
            catch (Exception e)
            {
                throw new CoreJsonException($"Deserialize exception: {e.Message}", e);
            }
        }

        public string ToJsonString(bool prettyPrint = false)
        {
            var settings = JsonHelper.GetJsonSerializerSettings();
            if (prettyPrint)
            {
                settings.Formatting = Formatting.Indented;
            }
            else
            {
                settings.Formatting = Formatting.None;
            }
            try
            {
                return JsonConvert.SerializeObject(this, settings);
            }
            catch (Exception e)
            {
                throw new CoreJsonException($"Serialize exception: {e.Message}", e);
            }
        }


    }
}
