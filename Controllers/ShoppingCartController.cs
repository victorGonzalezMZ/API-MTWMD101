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
    public class ShoppingCartController : ControllerBase{
        
        readonly IConfiguration _configuration;
        public string ConnectionStringLocal;

        public ShoppingCartController(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
        }

        [HttpPost("add_Shoppingcart")]
        public int AddToShoppingCart([FromBody] APIUsers.Library.Models.ShoppingCart shoppingCart){
            int id = 0;

            using (IShoppingCart Shopping = Factorizador.CrearConexionServicioShoppingCart(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = Shopping.addToShoppingCart(shoppingCart.ID_User, shoppingCart.ID_Product);
            }
            return id;
        }

        [HttpPost("add_Shoppingcart_fromlocal")]
        public int AddToShoppingCartFromLocal([FromBody] APIUsers.Library.Models.ShoppingCart shoppingCart)
        {
            int id = 0;

            using (IShoppingCart Shopping = Factorizador.CrearConexionServicioShoppingCart(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = Shopping.addToShoppingCart(shoppingCart.ID_User, shoppingCart.ID_Product,shoppingCart.Quantity);
            }
            return id;
        }


        [HttpPost("remove_Shoppingcart")]
        public int RemoveFromShoppingCart([FromBody] APIUsers.Library.Models.ShoppingCart shoppingCart){
            int id = 0;
            using (IShoppingCart Shopping = Factorizador.CrearConexionServicioShoppingCart(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal)){
                id = Shopping.removeFromShoppingCart(shoppingCart.ID_User, shoppingCart.ID_Product);
            }
            return id;
        }

        [HttpPut("update_Shoppingcart_quantity")]
        public int UpdateShoppingCartQuantity([FromBody] APIUsers.Library.Models.ShoppingCart shoppingCart)
        {
            int id = 0;
            using (IShoppingCart Shopping = Factorizador.CrearConexionServicioShoppingCart(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = Shopping.updateShoppingCart_ProductQuantity(shoppingCart.ID_User, shoppingCart.ID_Product, shoppingCart.Quantity);
            }
            return id;
        }


        [HttpGet("get_shoppingcart_products/{id}")]
        public IEnumerable<APIUsers.Library.Models.ShoppingCart> GetShoppingCart_Products(int id)
        {
            List<APIUsers.Library.Models.ShoppingCart> listCart = new List<APIUsers.Library.Models.ShoppingCart>();
            using (IShoppingCart Shopping = Factorizador.CrearConexionServicioShoppingCart(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listCart = Shopping.getShoppingCart(id);
            }

            return listCart;
        }

    }
}
