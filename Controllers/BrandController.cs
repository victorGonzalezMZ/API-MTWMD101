using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIUsers.Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API_MTWDM101.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase{

        readonly IConfiguration _configuration;
        public string ConnectionStringLocal;


        public BrandController(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult GetBrandBySelect()
        {
            List<APIUsers.Library.Models.Brand> list = new List<APIUsers.Library.Models.Brand>();
            using (IBrand Brand = Factorizador.CrearConexionServicioBrand(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                list = Brand.obtenerMarcasSelected();
            }
            return Ok(new { list });

        }
    }
}
