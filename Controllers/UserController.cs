using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_MTWDM101.Helpers;
using API_MTWDM101.Models;
using APIUsers.Library.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API_MTWDM101.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class UserController : ControllerBase{
        
        readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration){
            _configuration = configuration;
        }

       
        [HttpGet, Authorize(Roles = "ADMIN")]
        public IEnumerable<APIUsers.Library.Models.User> GetUsers(){
            List<APIUsers.Library.Models.User> listUsers = new List<APIUsers.Library.Models.User>();
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IUser User = Factorizador.CrearConexionServicio(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal)){
                listUsers = User.GetUsers();
            }
            return listUsers;
        }


        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public IActionResult GetUser(int id){

            APIUsers.Library.Models.User user = new APIUsers.Library.Models.User();
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IUser User = Factorizador.CrearConexionServicio(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                user = User.GetUser(id);
            }
            return Ok(new { user });
        }


        [Route("[action]/{nick}")]
        [HttpGet]
        public IActionResult GetUserByNick(string nick)
        {

            APIUsers.Library.Models.User user = new APIUsers.Library.Models.User();
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IUser User = Factorizador.CrearConexionServicio(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                user = User.GetUser(nick);
            }
            return Ok(new { user });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"> {"id": 3,"nick": "leones2019","password": "123123","createDate": "2019-08-02T12:43:02.9396464-05:00"}
        /// </param>
        /// <returns></returns>
        // POST api/<UserController>
        [HttpPost]
        public IActionResult InsertUser([FromBody] APIUsers.Library.Models.UserMin value)
        {
            int id = 0;
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IUser User = Factorizador.CrearConexionServicio(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = User.InsertUser(value);
            }
            if (id != 0) {
                return Ok(id);
            }
            else{
                return BadRequest();
            }
        }

        [HttpDelete("{id}"), Authorize(Roles = "ADMIN")]
        public Boolean DeleteUser(int id){
            Boolean status;

            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IUser User = Factorizador.CrearConexionServicio(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                status = User.DeleteUser(id);
            }

            return status;
        }

        [HttpPut]
        public Boolean UpdateUser([FromBody] APIUsers.Library.Models.UserMin value)
        {
            Boolean status;
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IUser User = Factorizador.CrearConexionServicio(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                status = User.UpdateUser(value);
            }
            return status;
        }

        [Route("[action]")]
        [HttpPut]
        public Boolean UpdateUserDomicilio([FromBody] APIUsers.Library.Models.User value)
        {
            Boolean status;
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IUser User = Factorizador.CrearConexionServicio(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                status = User.UpdateUser_domicilio(value);
            }
            return status;
        }







    }
}
