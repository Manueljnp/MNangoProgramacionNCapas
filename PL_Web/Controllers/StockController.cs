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

            int sucursal = producto.Sucursal.IdSucursal;

            //Cargar Datos ProductoSucursal
            ML.Result result = BL.Producto.ProductoSucursal.GetProductoBySucursal(sucursal);
            producto.ProductoSucursal.ProductosSucursales = result.Objects;


            return View(producto);
        }
    }
}