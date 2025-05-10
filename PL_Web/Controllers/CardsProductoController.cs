using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL_Web.Controllers
{
    public class CardsProductoController : Controller
    {
        // GET: CardsProducto
        [HttpGet]
        public ActionResult CardsProducto()
        {
            ML.Producto.Producto producto = new ML.Producto.Producto();
            producto.Subcategoria = new ML.Producto.Subcategoria();
            producto.Subcategoria.Categoria = new ML.Producto.Categoria();

            // Llenar DDL Categorías
            ML.Result ddlCategoria = BL.Producto.Categoria.GetAll();
            producto.Subcategoria.Categoria.Categorias = ddlCategoria.Objects;

            return View(producto);
        }

        [HttpPost]
        public ActionResult Form(ML.Producto.Producto producto)
        {
            //Obtener el archivo de la Imagen:      Nombre del input (o el id?)
            HttpPostedFileBase file = Request.Files["inptFileImagen"];
            if (file != null && file.ContentLength > 0)
            {
                producto.Imagen = ConvertirAArrayBytes(file);
            }
            else
            {
                //Si es actualización conservar la imágen previa
                if (producto.IdProducto != 0)
                {
                    var result = BL.Producto.Producto.GetById(producto.IdProducto);
                    if (result.Correct && result.Object != null)
                    {
                        var productoDB = (ML.Producto.Producto)result.Object;
                        producto.Imagen = productoDB.Imagen;
                    }
                }
            }

            if (producto.IdProducto == 0)
            {
                BL.Producto.Producto.Add(producto);
            }
            else
            {
                //UPDATE
                BL.Producto.Producto.Update(producto);

            }
            return RedirectToAction("CardsProducto");

        }
        public byte[] ConvertirAArrayBytes(HttpPostedFileBase foto)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(foto.InputStream);
            byte[] data = reader.ReadBytes((int)foto.ContentLength);
            return data;
        }

        [HttpPost]
        public ActionResult CardsProducto(ML.Producto.Producto producto)
        {
            //Rellenar parámetro de busqueda
            producto.Subcategoria.IdSubcategoria = producto.Subcategoria.IdSubcategoria == 0 ? 0 : producto.Subcategoria.IdSubcategoria;

            //Llamar al método de BL con los parámetros de busqueda
            ML.Result result = BL.Producto.Producto.GetAll(producto.Subcategoria.IdSubcategoria); //usuario ya tiene los valores que asignamos arriba
            if (result.Correct)
            {
                producto.Productos = result.Objects; //Mostrar la lista de Productos
            }
            else
            {
                producto.Productos = new List<object>(); //Mostrar una lista de productos VACIA
            }

            // Llenar DDL Categorías
            ML.Result ddlCategoria = BL.Producto.Categoria.GetAll();
            producto.Subcategoria.Categoria.Categorias = ddlCategoria.Objects;

            return View(producto);
        }

        [HttpGet]
        public JsonResult GetByIdCategoria(int idCategoria)
        {
            ML.Result result = BL.Producto.Subcategoria.GetByIdCategoria(idCategoria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarCards(int idSucategoria)
        {
            ML.Result result = BL.Producto.Producto.GetAll(idSucategoria); //Traer todos los productos

            return RedirectToAction("CardsProducto");
        }

        [HttpGet]
        public ActionResult Delete(int idProducto)
        {
            BL.Producto.Producto.Delete(idProducto);

            return RedirectToAction("CardsProducto");
        }
    }
}