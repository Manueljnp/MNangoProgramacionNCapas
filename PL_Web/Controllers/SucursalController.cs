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
        public  ActionResult GetAll()
        {
            ML.Producto.Sucursal sucursal = new ML.Producto.Sucursal();

            ML.Result result = BL.Producto.Sucursal.GetAll();
            sucursal.Sucursales = result.Objects;

            return View(sucursal);
        }

        [HttpGet]
        public ActionResult Form(int? idSucursal)
        {
            ML.Producto.Sucursal sucursal = new ML.Producto.Sucursal();

            if (idSucursal > 0)
            {
                ML.Result result = BL.Producto.Sucursal.GetById(idSucursal.Value);
                sucursal = (ML.Producto.Sucursal)result.Object;
            }

            return View(sucursal);
        }

        [HttpPost]
        public ActionResult Form(ML.Producto.Sucursal sucursal)
        {

            if (sucursal.IdSucursal == 0)
            {
                BL.Producto.Sucursal.Add(sucursal);
            }
            else
            {
                //UPDATE
                BL.Producto.Sucursal.Update(sucursal);

            }

            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public ActionResult Delete(int idSucursal)
        {
            BL.Producto.Sucursal.Delete(idSucursal);

            return RedirectToAction("GetAll");
        }
    }
}