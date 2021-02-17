using System;
using System.Collections.Generic;
using APIUsers.Library.Interfaces;
using APIUsers.Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace API_MTWDM101.Controllers{

    [Route("api/auth/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase {
        readonly IConfiguration _configuration;
        public string ConnectionStringLocal;


        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
        }

        [HttpGet]
        public IActionResult GetAllProducts() {
            List<APIUsers.Library.Models.Product> listProducts = new List<APIUsers.Library.Models.Product>();
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IProduct Product = Factorizador.CrearConexionServicioProduct(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = Product.GetAllProducts();
            }
            return Ok(new { listProducts });

        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {

            APIUsers.Library.Models.Product product = new Product();
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IProduct Product = Factorizador.CrearConexionServicioProduct(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                product = Product.GetProduct(id);
            }
            return Ok(new { product });
        }

        [Route("[action]/{category}")]
        [HttpGet]
        public IActionResult GetProductByCategory(string category) {

            List<APIUsers.Library.Models.Product> listProducts = new List<APIUsers.Library.Models.Product>();
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IProduct Product = Factorizador.CrearConexionServicioProduct(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = Product.GetAllProductsByCategory(category);
            }
            return Ok(new { listProducts });

        }


        [Route("[action]")]
        [HttpGet]
        public IActionResult GetTop3NewProducts()
        {

            List<APIUsers.Library.Models.Product> listProducts = new List<APIUsers.Library.Models.Product>();
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IProduct Product = Factorizador.CrearConexionServicioProduct(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = Product.GetTop3NewProducts();
            }
            return Ok(new { listProducts });

        }


        [Route("[action]")]
        [HttpGet]
        public IActionResult GetTop3Random()
        {

            List<APIUsers.Library.Models.Product> listProducts = new List<APIUsers.Library.Models.Product>();
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IProduct Product = Factorizador.CrearConexionServicioProduct(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = Product.GetTop3Random();
            }
            return Ok(new { listProducts });

        }

        [Route("[action]/{category}/{id}")]
        [HttpGet]
        public IActionResult GetTop3ByCategory(string category, int id)
        {

            List<APIUsers.Library.Models.Product> listProducts = new List<APIUsers.Library.Models.Product>();
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IProduct Product = Factorizador.CrearConexionServicioProduct(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = Product.GetTop3ByCategory(category,id);
            }
            return Ok(new { listProducts });

        }





        [Route("[action]/{brand}")]
        [HttpGet]
        public IActionResult GetProductByBrand(string brand) {
            List<APIUsers.Library.Models.Product> listProducts = new List<APIUsers.Library.Models.Product>();
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IProduct Product = Factorizador.CrearConexionServicioProduct(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = Product.GetAllProductsByBrand(brand);
            }
            return Ok(new { listProducts });

        }

        [Route("[action]/{search}")]
        [HttpGet]
        public IActionResult GetProductBySearch(string search) {

            List<APIUsers.Library.Models.Product> listProducts = new List<APIUsers.Library.Models.Product>();
            using (IProduct Product = Factorizador.CrearConexionServicioProduct(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal)) {
                listProducts = Product.GetAllProductsBySearch(search);
            }
            return Ok(new { listProducts });
        }



        [Route("[action]")]
        [HttpGet]
        public IActionResult GetAllProductsByParams([FromBody] APIUsers.Library.Models.Parametros value)
        {

            List<APIUsers.Library.Models.Product> listProducts = new List<APIUsers.Library.Models.Product>();
            using (IProduct Product = Factorizador.CrearConexionServicioProduct(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = Product.GetAllProductsByParams(value);
            }
            return Ok(new { listProducts });
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult GetAllProductsByParamsPost([FromBody] APIUsers.Library.Models.Parametros value)
        {

            List<APIUsers.Library.Models.Product> listProducts = new List<APIUsers.Library.Models.Product>();
            using (IProduct Product = Factorizador.CrearConexionServicioProduct(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = Product.GetAllProductsByParams(value);
            }
            return Ok(new { listProducts });
        }




        /*Estos deben ser metodos autenticados*/
        [HttpPost, Authorize(Roles = "ADMIN")]
        public int InsertProduct([FromBody] APIUsers.Library.Models.Product value) {
            int id = 0;
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IProduct Product = Factorizador.CrearConexionServicioProduct(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = Product.InsertProduct(value);
            }
            return id;
        }


        [HttpPut, Authorize(Roles = "ADMIN")]
        public Boolean UpdateProduct([FromBody] APIUsers.Library.Models.Product value){
            Boolean status;
            var ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
            using (IProduct Product = Factorizador.CrearConexionServicioProduct(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                status = Product.UpdateProduct(value);
            }
            return status;
        }

        [HttpDelete("{id}"), Authorize(Roles = "ADMIN")]
        public IActionResult DeleteProduct(int id){
            Boolean status; 
            using (IProduct Product = Factorizador.CrearConexionServicioProduct(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                status = Product.DeleteProduct(id);
            }
            return Ok(status);
        }

        
    }



}
