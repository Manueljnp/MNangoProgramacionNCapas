using BL.Producto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL_Web.Controllers
{
    public class StockController : Controller
    {
        // GET: Stock
        public ActionResult Stock()
        {
            //Creando instancias
            ML.Producto.Producto producto = new ML.Producto.Producto();
            producto.Sucursal = new ML.Producto.Sucursal();
            producto.ProductoSucursal = new ML.Producto.ProductoSucursal();

            //Cargar Sucursales
            ML.Result ddlSucursal = BL.Producto.Sucursal.GetAll();
            producto.Sucursal.Sucursales = ddlSucursal.Objects;

            return View(producto);
        }

        [HttpPost]
        public ActionResult CargarProductos(ML.Producto.Producto producto)
        {
            int idSucursal = producto.Sucursal.IdSucursal;

            // Cargar Sucursales
            ML.Result ddlSucursal = BL.Producto.Sucursal.GetAll();
            producto.Sucursal.Sucursales = ddlSucursal.Objects;

            //Cargar Datos ProductoSucursal
            ML.Result result = BL.Producto.ProductoSucursal.GetProductoBySucursal(idSucursal);
            producto.ProductoSucursal = new ML.Producto.ProductoSucursal();
            producto.ProductoSucursal.ProductosSucursales = result.Objects;

            return View("Stock", producto);
        }

        [HttpPost]
        public JsonResult ActualizarStock(int idProductoSucursal, int stock)
        {
            ML.Result result = BL.Producto.ProductoSucursal.ActualizarStock(idProductoSucursal, stock);
            return Json(result);
        }
    }
}