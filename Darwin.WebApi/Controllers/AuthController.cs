namespace Darwin.WebApi.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Darwin.Services.Authorization;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfigurationRoot configuration { get; }

        private IAuthService authService { get; }

        public AuthController(IConfigurationRoot configuration, IAuthService authService)
        {
            this.configuration = configuration;
            this.authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Google([FromBody]ProviderToken providerToken)
        {
            try
            {
                var identityProviderUser = await this.authService.GetAuthorizedUser(new IdentityProviderResponse
                {
                    Provider = IdentityProvider.Google,
                    AuthorizationCode = providerToken.AuthorizationCode
                });

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, identityProviderUser.UserId),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["AppSettings:JwtSecret"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(string.Empty,
                  string.Empty,
                  claims,
                  expires: DateTime.Now.AddDays(1),
                  signingCredentials: creds);

                return Ok(
                    new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
            }
            catch(Exception ex)
            {
                BadRequest(ex.Message);
            }

            return BadRequest();
        }
    }
}