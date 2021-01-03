using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IgcRestApi.Models
{
    public class LoginRequestModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>admin</example>
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>securePassword</example>
        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
