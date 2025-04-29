using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL_Web.Controllers
{
    public class ProductoController : Controller
    {
        // GET: Producto
        public ActionResult Producto()
        {
            ML.Producto.Producto producto = new ML.Producto.Producto();
            producto.Subcategoria = new ML.Producto.Subcategoria();
            producto.Subcategoria.Categoria = new ML.Producto.Categoria();

            ML.Result DDLCategoria = BL.Producto.Categoria.GetAll();

            producto.Subcategoria.Categoria.Categorias = DDLCategoria.Objects;

            return View(producto);
        }

        [HttpGet]
        public JsonResult GetByIdCategoria(int idCategoria)
        {
            ML.Result result = BL.Producto.Subcategoria.GetByIdCategoria(idCategoria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CargarTabla(int idSubcategoria)
        {
            ML.Producto.Producto producto = new ML.Producto.Producto();

            ML.Result result = BL.Producto.Producto.GetAll(idSubcategoria);

            if (result.Correct)
            {
                producto.Productos = result.Objects;
            }
            else
            {
                producto.Productos = new List<object>();
            }

            return Json(producto, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Form()
        {
            ML.Producto.Producto producto = new ML.Producto.Producto();
            producto.Subcategoria = new ML.Producto.Subcategoria();
            producto.Subcategoria.Categoria = new ML.Producto.Categoria();

            ML.Result ddlCategoria = BL.Producto.Categoria.GetAll();
            producto.Subcategoria.Categoria.Categorias = ddlCategoria.Objects;

            //ML.Result ddlSubcategoria = BL.Producto.Subcategoria.GetByIdCategoria();


            return View(producto);
        }

        [HttpPost]
        public ActionResult Form(ML.Producto.Producto producto)
        {
            //Obtener el archivo de la Imagen:      Nombre del input (o el id?)
            HttpPostedFileBase file = Request.Files["inptFileImagen"];
            if (file != null /*&& file.ContentLength > 0*/)
            {
                producto.Imagen = ConvertirAArrayBytes(file);
            }
            /*else
            {
                //Si no selecciona una nueva imagen, mantener la imagen acctual
                if (!string.IsNullOrEmpty(Request.Form["ImagenActual"]))
                {
                    usuario.Imagen = Convert.FromBase64String(Request.Form["ImagenActual"]);
                }
            }*/

            if (producto.IdProducto == 0)
            {
                BL.Producto.Producto.Add(producto);
            }
            else
            {
                //UPDATE
                BL.Producto.Producto.Update(producto);

            }
            return RedirectToAction("Producto");

        }

        //Funciones para JS
        [HttpPost]
        public JsonResult Delete(int idProducto)
        {
            ML.Result result = BL.Producto.Producto.Delete(idProducto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPatch]
        public JsonResult Update(ML.Producto.Producto producto)
        {
            ML.Result result = BL.Producto.Producto.Update(producto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetById(int idProducto)
        {
            ML.Result result = BL.Producto.Producto.GetById(idProducto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DDLCategorias(string name)
        {
            ML.Result result = BL.Producto.Categoria.GetAll();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Funcion Convertir Imagen
        public byte[] ConvertirAArrayBytes(HttpPostedFileBase foto)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(foto.InputStream);
            byte[] data = reader.ReadBytes((int)foto.ContentLength);
            return data;
        }
    }
}