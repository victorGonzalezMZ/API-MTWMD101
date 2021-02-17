using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_MTWDM101.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

namespace API_MTWDM101.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase{
        readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration){
            _configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usMin">{"Nick": "rgatilanov","Password": "96CAE35CE8A9B0244178BF28E4966C2CE1B8385723A96A6B838858CDD6CA0A1E"}</param>
        /// <returns></returns>
        /*
       #region Método de simulación (sin conexión a BD ) de login
       public Models.Login Authenticate(UserMin usMin)
       {
           // Integración a base de datos
           if (usMin.Nick == "rgatilanov" && usMin.Password == "96CAE35CE8A9B0244178BF28E4966C2CE1B8385723A96A6B838858CDD6CA0A1E") //SHA2
           {
               // Leemos el secret_key desde nuestro appseting
               var secretKey = _configuration.GetValue<string>("SecretKey");
               var key = Encoding.ASCII.GetBytes(secretKey);

               var tokenDescriptor = new SecurityTokenDescriptor
               {
                   // Nuestro token va a durar un día
                   Expires = DateTime.UtcNow.AddDays(1),
                   // Credenciales para generar el token usando nuestro secretykey y el algoritmo hash 256
                   SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
               };

               var tokenHandler = new JwtSecurityTokenHandler();
               var createdToken = tokenHandler.CreateToken(tokenDescriptor);

               return new Login()
               {
                   ID = usMin.ID,
                   Nick = usMin.Nick,
                   Token = tokenHandler.WriteToken(createdToken),
               };
           }
           else
               return null;

       }
       #endregion
        */
        /*
        [HttpPost]
        #region Método para integración con Angular
        public IActionResult Login([FromBody] UserMin user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            if (user.Nick == "rgatilanov" && user.Password == "4297f44b13955235245b2497399d7a93") //MD5 (123123)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey")));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:44395",
                    audience: "http://localhost:44395",
                    claims: new List<System.Security.Claims.Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }
        #endregion
        

        /*
        [HttpPost]
        #region Método para integrar login con SQL Server
        public APIUsers.Library.Models.User Login([FromBody] APIUsers.Library.Models.UserMin user)
        {
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorLocal");
             using (APIUsers.Library.Interfaces.ILogin Login = APIUsers.Library.Interfaces.Factorizador.CrearConexionServicioLogin(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                APIUsers.Library.Models.User objusr = Login.EstablecerLogin(user.Nick, user.Password);

                if (objusr.ID > 0)
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey")));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var tokeOptions = new JwtSecurityToken(
                        issuer: "http://localhost:44395",
                        audience: "http://localhost:44395",
                        claims: new List<System.Security.Claims.Claim>(),
                        expires: DateTime.Now.AddMinutes(5),
                        signingCredentials: signinCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    objusr.JWT = tokenString;
                }
                return objusr;
            }
        }
        #endregion
        */
       
       [HttpPost]
       #region Método para integrar login con SQL Server y Angular
       public IActionResult Login([FromBody] APIUsers.Library.Models.UserMin user){

            if (user == null){
                return BadRequest("Invalid client request");
            }

            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (APIUsers.Library.Interfaces.ILogin Login = APIUsers.Library.Interfaces.Factorizador.CrearConexionServicioLogin(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal)){
                
                APIUsers.Library.Models.User objusr = Login.EstablecerLogin(user.Nick, user.Password);
                
                if(objusr == null)
                    return Unauthorized();
               

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey")));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    
                    var claims = new List<Claim>{
                        new Claim(ClaimTypes.Name, objusr.Nick),
                        new Claim(ClaimTypes.Role, objusr.Role),
                        new Claim("Id",objusr.ID.ToString()),
                        new Claim("imagen", objusr.Imagen)
                    };

                    var tokeOptions = new JwtSecurityToken(
                       issuer: "https://api03mtw102.azurewebsites.net",
                       audience: "https://api03mtw102.azurewebsites.net",
                       claims: claims,
                       expires: DateTime.Now.AddMinutes(120),
                       signingCredentials: signinCredentials
                   );
                    

                   var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                   var refreshString = GenerateRefreshToken();

                   objusr.RefreshToken = refreshString;
                   objusr.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

                    using (APIUsers.Library.Interfaces.IUser User = APIUsers.Library.Interfaces.Factorizador.CrearConexionServicio(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
                    {
                        User.UpdateUserRefreshToken(objusr);
                    }
                


                    return Ok(new { 
                    Token = tokenString,
                    RefreshToken = refreshString
                });
           }
       }
        #endregion

        public string GenerateRefreshToken(){
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }
}