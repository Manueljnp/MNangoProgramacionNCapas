using BL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL_Web.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Mostrar GetAll (TODOS)
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();

            //Asignarle valores vacíos (porque el SP no acepta NULL)
            usuario.Nombre = "";
            usuario.ApellidoPaterno = "";
            usuario.ApellidoMaterno = "";
            usuario.Rol.IdRol = 0;

            ML.Result resultDDL = BL.Rol.GetAll();
            
            usuario.Rol.Roles = resultDDL.Objects;

            ML.Result result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                //Obtuvo toda la información
                usuario.Usuarios = result.Objects;
            }
            else
            {
                usuario.Usuarios = new List<object>();
            }

            return View(usuario);
        }

        //POST: Para realizar la busqueda abierta en GetAll
        [HttpPost]
        public ActionResult GetAll(ML.Usuario usuario)
        {
            //Relacionar los parametros con los atributos del modelo
            usuario.Nombre = usuario.Nombre == null ? "" : usuario.Nombre ;
            usuario.ApellidoPaterno = usuario.ApellidoPaterno == null ? "" : usuario.ApellidoPaterno;
            usuario.ApellidoMaterno = usuario.ApellidoMaterno == null ? "" : usuario.ApellidoMaterno;
            usuario.Rol.IdRol = usuario.Rol.IdRol == 0 ? 0 : usuario.Rol.IdRol;

            //Llamar al método de BL con los parámetros de busqueda
            ML.Result result = BL.Usuario.GetAll(usuario); //usuario ya tiene los valores que asignamos arriba
            if(result.Correct)
            {
                usuario.Usuarios = result.Objects; //Mostrar la lista De Usuarios
            }
            else
            {
                usuario.Usuarios = new List<object>(); //Mostrar una lista de usuarios VACIA
            }

            //Mostrar TODOS los Roles (siempre)
            ML.Result resultDDL = BL.Rol.GetAll();
            usuario.Rol.Roles = resultDDL.Objects;

            return View(usuario);
        }

        [HttpGet] //Mostrando una Vista
        public ActionResult Form(int? IdUsuario)
        {
            ML.Usuario usuario = new ML.Usuario(); //Usuario es vacío

            usuario.Direccion = new ML.Direccion(); //Inicializar la propiedad Dirección
            usuario.Direccion.Colonia = new ML.Colonia(); //Inicializar la propiedad Colonia
            usuario.Direccion.Colonia.Municipio = new ML.Municipio(); //Inicializar la propiedad Municipio
            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado(); //Inicializar la propiedad Estado

            //Condición para mostrar ROL
            if (IdUsuario == null)
            {
                usuario.Rol = new ML.Rol();
            }
            else
            {
                ML.Result result = BL.Usuario.GetById(IdUsuario.Value);
                usuario = (ML.Usuario)result.Object;

                ML.Result resultMunicipio = BL.Municipio.GetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                usuario.Direccion.Colonia.Municipio.Municipios = resultMunicipio.Objects;

                ML.Result resultColonia = BL.Colonia.GetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);
                usuario.Direccion.Colonia.Colonias = resultColonia.Objects;
            }

            //Verificar que la propiedad Estado esté inicializado antes de asignar el resultado
            if (usuario.Direccion?.Colonia?.Municipio?.Estado != null)
            {
                ML.Result resultEstado = BL.Estado.GetAll();
                usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstado.Objects;
            }

            //Obtener los Roles
            ML.Result resultRoles = BL.Rol.GetAll();
            usuario.Rol.Roles = resultRoles.Objects;

            //ML.Result resultEstados = BL.Estado.GetAll();
            //usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstados.Objects;

            //suario.Rol = new ML.Rol();
            return View(usuario);
        }

        [HttpPost]  //Este es para Agregar y Actualizar
        public ActionResult Form(ML.Usuario usuario)
        {
            //Obtener el archivo de la Imagen:      Nombre del input (o el id?)
            HttpPostedFileBase file = Request.Files["inptFileImagen"];
            if(file != null /*&& file.ContentLength > 0*/)
            {
                usuario.Imagen = ConvertirAArrayBytes(file);
            }
            /*else
            {
                //Si no selecciona una nueva imagen, mantener la imagen acctual
                if (!string.IsNullOrEmpty(Request.Form["ImagenActual"]))
                {
                    usuario.Imagen = Convert.FromBase64String(Request.Form["ImagenActual"]);
                }
            }*/

            if (usuario.IdUsuario == 0)
            {
                //ADD CON DIRECCION (Cambiar a 'ADD' al finalizar pruebas) **************************************************************
                BL.Usuario.Add(usuario);
            }
            else
            {
                //UPDATE
                BL.Usuario.Update(usuario);

            }
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public ActionResult Delete(int IdUsuario)
        {
            BL.Usuario.Delete(IdUsuario);
            return RedirectToAction("GetAll");
        }

        [HttpPost]
        public JsonResult CambioEstatus(int IdUsuario, bool Estatus)
        {
            ML.Result JsonResult = BL.Usuario.CambioEstatus(IdUsuario, Estatus);
            return Json (JsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetByIdEstado(int idEstado)
        {
            ML.Result JsonResult = BL.Municipio.GetByIdEstado(idEstado);
            return Json(JsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetByIdMunicipio(int idMunicipio)
        {
            ML.Result JsonResult = BL.Colonia.GetByIdMunicipio(idMunicipio);
            return Json(JsonResult, JsonRequestBehavior.AllowGet);
        }

        public byte[] ConvertirAArrayBytes(HttpPostedFileBase foto)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(foto.InputStream);
            byte[] data = reader.ReadBytes((int)foto.ContentLength);
            return data;
        }

    }
}