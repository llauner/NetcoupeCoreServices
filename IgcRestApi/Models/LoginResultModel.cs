namespace IgcRestApi.Models
{
    public class LoginResultModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>admin</example>
        public string UserName { get; set; }
        public string JwtToken { get; set; }
    }
}
