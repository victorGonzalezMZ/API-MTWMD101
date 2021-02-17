using System;
using System.Collections.Generic;
using APIUsers.Library.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace API_MTWDM101.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase{

        readonly IConfiguration _configuration;
        string ConnectionStringLocal;


        public CategoryController(IConfiguration configuration){
            _configuration = configuration;
            ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
        }

        [HttpGet]
        public IActionResult GetCategories(){
            List<APIUsers.Library.Models.Category> listCategories = new List<APIUsers.Library.Models.Category>();
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (ICategory Category = Factorizador.CrearConexionServicioCategory(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listCategories = Category.ObtenerCategorias();
            }
            return Ok(new { listCategories });
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id){
            APIUsers.Library.Models.Category category = new APIUsers.Library.Models.Category();
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (ICategory Category = Factorizador.CrearConexionServicioCategory(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                category = Category.GetCategory(id);
            }
            return Ok(new { category });
        }

        [Route("[action]/{search}")]
        [HttpGet]
        public IActionResult GetCategoriesBySearch(string search)
        {

            List<APIUsers.Library.Models.Category> list = new List<APIUsers.Library.Models.Category>();
            using (ICategory Category = Factorizador.CrearConexionServicioCategory(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                list = Category.GetAllCategoriesBySearch(search);
            }
            return Ok(new { list });
        }


        [HttpPost, Authorize(Roles = "ADMIN")]
        public int InsertCategory([FromBody] APIUsers.Library.Models.Category value){
            int id = 0;
            using (ICategory Category = Factorizador.CrearConexionServicioCategory(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = Category.InsertCategory(value);
            }
            return id;
        }
       
        [HttpPut, Authorize(Roles = "ADMIN")]
        public IActionResult UpdateCategory([FromBody] APIUsers.Library.Models.Category value){
            Boolean status;
            using (ICategory Category = Factorizador.CrearConexionServicioCategory(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal)){
                status = Category.UpdateCategory(value);
            }
            return Ok(status);
        }

        [HttpDelete("{id}"), Authorize(Roles = "ADMIN")]
        public IActionResult DeleteCategory(int id)
        {
            Boolean status;
            using (ICategory Category = Factorizador.CrearConexionServicioCategory(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                status = Category.DeleteCategory(id);
            }
            return Ok(status);
        }



        [Route("[action]")]
        [HttpGet]
        public IActionResult GetCategoriesBySelect()
        {
            List<APIUsers.Library.Models.CategorySelect> listCategories = new List<APIUsers.Library.Models.CategorySelect>();
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (ICategory Category = Factorizador.CrearConexionServicioCategory(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listCategories = Category.obtenerCategoriasSelected();
            }
            return Ok(new { listCategories });

        }


    }
}
