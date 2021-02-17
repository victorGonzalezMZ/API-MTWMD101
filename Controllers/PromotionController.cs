using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIUsers.Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace API_MTWDM101.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        readonly IConfiguration _configuration;
        public string ConnectionStringLocal;

        public PromotionController(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionStringLocal = _configuration.GetValue<string>("ServidorAzure");
        }

        [HttpPost, Authorize(Roles = "ADMIN")]
        public int InsertPromotion([FromBody] APIUsers.Library.Models.Promotion value)
        {
            int id = 0;
            using (IPromotion promotion = Factorizador.CrearConexionServicio3(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = promotion.InsertPromotion(
                    value.code, value.title, value.description, value.expires_date, value.theme, value.discount
                    );
            }
            return id;
        }
        [HttpGet]

        public IEnumerable<APIUsers.Library.Models.Promotion> GetPromotions()
        {

            List<APIUsers.Library.Models.Promotion> listPromotions = new List<APIUsers.Library.Models.Promotion>();
            using (IPromotion promotion = Factorizador.CrearConexionServicio3(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listPromotions = promotion.GetPromotions();
            }
            return listPromotions;
        }
        [HttpPost("remove_promotion"), Authorize(Roles = "ADMIN")]
        public int remove_promotion([FromBody] APIUsers.Library.Models.Promotion value)
        {
            int result = 0;
            using (IPromotion promotion = Factorizador.CrearConexionServicio3(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                result = promotion.removePromotion(value.id);
            }
            return result;


        }
        [HttpPost("update_promotion"), Authorize(Roles = "ADMIN")]
        public int UpdatePromotion([FromBody] APIUsers.Library.Models.Promotion value)
        {
            int id = 0;
            using (IPromotion promotion = Factorizador.CrearConexionServicio3(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = promotion.UpdatePromotion(
                    value.id, value.code, value.title, value.description, value.expires_date, value.theme, value.discount
                    );
            }
            return id;
        }
        [HttpPost("getpromotion")]
        public IEnumerable<APIUsers.Library.Models.Promotion> GetPromotionsById([FromBody] APIUsers.Library.Models.Promotion value)
        {

            List<APIUsers.Library.Models.Promotion> listPromotions = new List<APIUsers.Library.Models.Promotion>();
            using (IPromotion promotion = Factorizador.CrearConexionServicio3(APIUsers.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listPromotions = promotion.GetPromotionsById(value.id);
            }
            return listPromotions;
        }
    }
}
