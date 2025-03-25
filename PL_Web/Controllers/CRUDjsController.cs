using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;

namespace PL_Web.Controllers
{
    public class CRUDjsController : Controller
    {
        // GET: CRUDjs
        public ActionResult GetAll()
        {
            return View();
        }

        // GET: Mostrar GetAll (TODOS)
        [HttpGet]
        public JsonResult GetAllusuario()
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

            JsonResult jsonResult = Json(result, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int IdUsuario)
        {
            ML.Result result = BL.Usuario.Delete(IdUsuario);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(ML.Usuario usuario)
        {
            /*if (!string.IsNullOrEmpty(usuario.ImagenBase64))
            {
                usuario.Imagen = Convert.FromBase64String(usuario.ImagenBase64);
            }*/

            ML.Result result = BL.Usuario.Add(usuario);

            if (result.Correct)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.Update(usuario);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetById(int idUsuario)
        {
            ML.Result result = BL.Usuario.GetById(idUsuario);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //Rutas para DROP DOWN LIST
        [HttpGet]
        public JsonResult GetRoles()
        {
            ML.Result resultRoles = BL.Rol.GetAll();

            if (resultRoles.Correct)
            {
                var roles = resultRoles.Objects.Select(r => new
                {
                    IdRol = ((ML.Rol)r).IdRol,
                    Nombre = ((ML.Rol)r).Nombre
                }).ToList();

                return Json(roles, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "No se pudieron obtener los roles" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllEstados()
        {
            ML.Result result = BL.Estado.GetAll();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMunicipiosByEstado(int idEstado)
        {
            ML.Result municipios = BL.Municipio.GetByIdEstado(idEstado);

            return Json(municipios, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetColoniasByMunicipio(int idMunicipio)
        {
            ML.Result colonias = BL.Colonia.GetByIdMunicipio(idMunicipio);
            return Json(colonias, JsonRequestBehavior.AllowGet);
        }


    }
}