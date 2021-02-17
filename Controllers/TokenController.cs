using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API_MTWDM101.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase{

        readonly IConfiguration _configuration;
        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(Models.Token token){

            if (token is null){
                return BadRequest("Invalid client request");
            }
            string accessToken = token.AccessToken;
            string refreshToken = token.RefreshToken;
            var principal = GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (APIUsers.Library.Interfaces.IUser User = APIUsers.Library.Interfaces.Factorizador.CrearConexionServicio(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {

                APIUsers.Library.Models.User objusr = User.CheckRefreshToken(username);

                if (objusr == null || objusr.RefreshToken != refreshToken || objusr.RefreshTokenExpiryTime <= DateTime.Now){
                    return BadRequest("Invalid client request");
                }

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey")));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);


                var tokeOptions = new JwtSecurityToken(
                      issuer: "https://api03mtw102.azurewebsites.net",
                      audience: "https://api03mtw102.azurewebsites.net",
                      claims: principal.Claims,
                      expires: DateTime.Now.AddMinutes(120),
                      signingCredentials: signinCredentials
                );


                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                var refreshString = GenerateRefreshToken();

                objusr.RefreshToken = refreshString;
                objusr.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

                using (APIUsers.Library.Interfaces.IUser User2 = APIUsers.Library.Interfaces.Factorizador.CrearConexionServicio(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
                {
                    User2.UpdateUserRefreshToken(objusr);
                }


                return new ObjectResult(new
                {
                    Token = tokenString,
                    RefreshToken = refreshString
                });
            }
            
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token){
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey"))),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }


    }
}
