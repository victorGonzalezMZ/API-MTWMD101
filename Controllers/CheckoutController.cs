using APIUsers.Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MTWDM101.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        readonly IConfiguration _configuration;
        public string ConnectionStringLocal;

        public CheckoutController(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
        }

        [HttpPost]
        public int InsertCheckout([FromBody] APIUsers.Library.Models.Checkout value)
        {
            int id = 0;
            using (ICheckout checkout = Factorizador.CrearConexionServicio2(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = checkout.InsertCheckout(
                    value.idUser,
                    value.code,
                    value.paymentMethod
                    );
            }
            return id;
        }

        [Route("[action]/{nick}")]
        [HttpGet]
        public IEnumerable<APIUsers.Library.Models.Checkout> GetchkUserByNick(string nick)
        {
            /*
            APIUsers.Library.Models.Checkout userDAta = new APIUsers.Library.Models.Checkout();
            using (ICheckout chk = Factorizador.CrearConexionServicio2(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                userDAta = chk.GetUserByNick(nick);
            }
            return userDAta;*/
            return null;
        }

   
        [HttpPost("validCodePromo")]
        public int GetProductValidbyCode([FromBody] string code)
        {

            int userDAta = 0;
            using (ICheckout chk = Factorizador.CrearConexionServicio2(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                userDAta = chk.getProductValidbyCode(code);
            }
            return userDAta;
        }

        [HttpGet("getShopping/{idUser}")]
        public string GetPromotions(int idUser)
        {

            string listShopping = "";
            using (ICheckout chk = Factorizador.CrearConexionServicio2(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listShopping = chk.getShoping(idUser);
            }
            return listShopping;
        }

    }
}
