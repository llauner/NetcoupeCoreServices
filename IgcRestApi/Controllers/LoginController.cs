using IgcRestApi.Dto;
using IgcRestApi.Models;
using IgcRestApi.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IgcRestApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<LoginController> _logger;
        private readonly IUserService _userService;
        private readonly TokenManagementDto _tokenManagementDto;
        public LoginController(ILogger<LoginController> logger, IUserService userService, TokenManagementDto tokenManagementDto)
        {
            _logger = logger;
            _userService = userService;
            _tokenManagementDto = tokenManagementDto;
        }

        /// <summary>
        /// JWT token login
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="requestModel"></param>
        /// <returns>LoginResultModel</returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResultModel))]
        public ActionResult Login([FromBody] LoginRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }

            if (!_userService.IsValidUser(requestModel.UserName, requestModel.Password))
            {
                return BadRequest("Invalid Request");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,requestModel.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagementDto.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                _tokenManagementDto.Issuer,
                _tokenManagementDto.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_tokenManagementDto.AccessExpiration),
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            _logger.LogInformation($"User [{requestModel.UserName}] logged in the system.");
            return Ok(new LoginResultModel
            {
                UserName = requestModel.UserName,
                JwtToken = token
            });
        }


        /// <summary>
        /// Ping with Jwt authentication required
        /// </summary>
        /// <returns></returns>
        /// <remarks>Add token to request header.<br/>
        /// Authorization : Bearer {{token}}
        /// </remarks>
        /// <response code="401">Unauthorized: Invalid token supplied</response>
        [HttpGet("ping")]
        [Authorize]
        public PingResponse PingJwt()
        {
            var username = User.Identity.Name;
            var msg = $"User is visiting /login/ping with jwt token authentication: username= {username}";
            _logger.LogInformation(msg);

            return new PingResponse(msg);
        }
    }
}
