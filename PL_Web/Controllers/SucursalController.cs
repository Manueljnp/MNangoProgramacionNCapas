using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PL_Web.Controllers
{
    public class SucursalController : Controller
    {
        // GET: Sucursal
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            ML.Producto.Sucursal sucursal = new ML.Producto.Sucursal();

            ML.Result result = BL.Producto.Sucursal.GetAll();
            sucursal.Sucursales = result.Objects;

            return View(sucursal);
        }

        public async Task<ActionResult> Form()
        {
            return View();
        }
    }
}