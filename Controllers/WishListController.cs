using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIUsers.Library.Interfaces;
using APIUsers.Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API_MTWDM101.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {

        readonly IConfiguration _configuration;
        public string ConnectionStringLocal;

        public WishListController(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
        }

        [HttpGet("get_wishlist_products/{id}")]
        public IEnumerable<Product> GetWishList_Products(int id)
        {
            List<Product> listProducts = new List<Product>();

            using (IWishList WishList = Factorizador.CrearConexionServicioWishList(ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = WishList.getProducts_Wishlist(id);
            }

            return listProducts;
        }


        [HttpGet("get_wishlist_products_byorder/{id}/{order}")]
        public IEnumerable<Product> GetWishList_Products_ByOrder(int id, string order)
        {
            List<Product> listProducts = new List<Product>();

            using (IWishList WishList = Factorizador.CrearConexionServicioWishList(ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = WishList.getProducts_Wishlist_ByOrder(id, order);
            }

            return listProducts;
        }



        [HttpPost("add_wishlist")]
        public int AddToWishList([FromBody] APIUsers.Library.Models.WishList wishList)
        {
            int id = 0;
            using (IWishList WishList = Factorizador.CrearConexionServicioWishList(ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = WishList.addToWishList(wishList.ID_User, wishList.ID_Product);
            }
            return id;
        }

        [HttpPost("remove_wishlist")]
        public int RemovefromWishList([FromBody] APIUsers.Library.Models.WishList wishList)
        {
            int id = 0;
            using (IWishList WishList = Factorizador.CrearConexionServicioWishList(ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = WishList.removeFromWishList(wishList.ID_User, wishList.ID_Product);
            }
            return id;
        }



        [Route("[action]/{id}/{search}")]
        [HttpGet]
        public IActionResult SearchWishList_Products(int id, string search)
        {
            List<APIUsers.Library.Models.Product> listProducts = new List<APIUsers.Library.Models.Product>();
            using (IWishList WishList = Factorizador.CrearConexionServicioWishList(ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = WishList.searchProducts_Wishlist(search, id);
            }
            return Ok(new { listProducts });
        }

    }
}