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
        public ActionResult Form(int? idProducto)
        {
            ML.Producto.Producto producto = new ML.Producto.Producto();
            producto.Subcategoria = new ML.Producto.Subcategoria();
            producto.Subcategoria.Categoria = new ML.Producto.Categoria();

            producto.Subcategoria.Categoria.Categorias = new List<object>();

            if (idProducto > 0)
            {
                ML.Result result = BL.Producto.Producto.GetById(idProducto.Value);
                producto = (ML.Producto.Producto)result.Object;

                if (producto.Subcategoria == null)
                {
                    producto.Subcategoria = new ML.Producto.Subcategoria();
                }
                if(producto.Subcategoria.Categoria == null)
                {
                    producto.Subcategoria.Categoria = new ML.Producto.Categoria();
                }
                if(producto.Subcategoria.Subcategorias == null)
                {
                    producto.Subcategoria.Subcategorias = new List<object>();
                }

                ML.Result ddlSubcategoria = BL.Producto.Subcategoria.GetByIdCategoria(producto.Subcategoria.Categoria.IdCategoria);
                producto.Subcategoria.Subcategorias = ddlSubcategoria.Objects;
            }


            ML.Result ddlCategoria = BL.Producto.Categoria.GetAll();
            producto.Subcategoria.Categoria.Categorias = ddlCategoria.Objects;


            return View(producto);
        }

        /*
         [HttpGet]
        public ActionResult Form(int? idProducto)
        {
            ML.Producto.Producto producto = new ML.Producto.Producto();

            // Asegurar que Subcategoria y Categoria están inicializadas
            producto.Subcategoria = new ML.Producto.Subcategoria();
            producto.Subcategoria.Categoria = new ML.Producto.Categoria();

            // Inicializar la lista de subcategorías como lista vacía para evitar errores en la vista
            producto.Subcategoria.Subcategorias = new List<Object>();

            // Cargar las categorías
            ML.Result ddlCategoria = BL.Producto.Categoria.GetAll();
            producto.Subcategoria.Categoria.Categorias = ddlCategoria.Objects;

            // Si es edición (tiene Id)
            if (idProducto > 0)
            {
                ML.Result result = BL.Producto.Producto.GetById(idProducto.Value);
                producto = (ML.Producto.Producto)result.Object;

                // Asegurar que Subcategoria y Categoria no sean nulos
                if (producto.Subcategoria == null)
                {
                    producto.Subcategoria = new ML.Producto.Subcategoria();
                }
                if (producto.Subcategoria.Categoria == null)
                {
                    producto.Subcategoria.Categoria = new ML.Producto.Categoria();
                }

                // Volver a cargar las categorías para el DropDownList
                producto.Subcategoria.Categoria.Categorias = ddlCategoria.Objects;

                // Aquí podrías cargar las subcategorías disponibles si quieres que salgan habilitadas al editar
                ML.Result resultSubcategorias = BL.Producto.Subcategoria.GetByIdCategoria(producto.Subcategoria.Categoria.IdCategoria);
                producto.Subcategoria.Subcategorias = resultSubcategorias.Objects;
            }

            return View(producto);
        }
         */

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

        /*
        [HttpGet]
        public JsonResult DDLCategorias(string name)
        {
            ML.Result result = BL.Producto.Categoria.GetAll();
            return Json(result, JsonRequestBehavior.AllowGet);
        }*/

        //Funcion Convertir Imagen
        public byte[] ConvertirAArrayBytes(HttpPostedFileBase foto)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(foto.InputStream);
            byte[] data = reader.ReadBytes((int)foto.ContentLength);
            return data;
        }
    }
}