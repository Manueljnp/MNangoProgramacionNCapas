using BL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.IO;
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
            usuario.Nombre = usuario.Nombre == null ? "" : usuario.Nombre;
            usuario.ApellidoPaterno = usuario.ApellidoPaterno == null ? "" : usuario.ApellidoPaterno;
            usuario.ApellidoMaterno = usuario.ApellidoMaterno == null ? "" : usuario.ApellidoMaterno;
            usuario.Rol.IdRol = usuario.Rol.IdRol == 0 ? 0 : usuario.Rol.IdRol;

            //Llamar al método de BL con los parámetros de busqueda
            ML.Result result = BL.Usuario.GetAll(usuario); //usuario ya tiene los valores que asignamos arriba
            if (result.Correct)
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

        [HttpPost]  //Este es para Agregar y Actualizar (Validaciones DATA ANNOTATION)
        public ActionResult Form(ML.Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Validation success.
                string mensaje = "";
                //Obtener el archivo de la Imagen:      Nombre del input (o el id?)
                HttpPostedFileBase file = Request.Files["inptFileImagen"];
                if (file != null /*&& file.ContentLength > 0*/)
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
                    mensaje = "Usuario agregado correctamente";
                }
                else
                {
                    //UPDATE
                    BL.Usuario.Update(usuario);
                    mensaje = "Usuario actualizado correctamente";

                }

                ViewBag.Mensaje = mensaje;
                return PartialView("_Partial");
            }
            else
            {
                //ML.Usuario usuario = new ML.Usuario(); //Usuario es vacío

                usuario.Direccion = new ML.Direccion(); //Inicializar la propiedad Dirección
                usuario.Direccion.Colonia = new ML.Colonia(); //Inicializar la propiedad Colonia
                usuario.Direccion.Colonia.Municipio = new ML.Municipio(); //Inicializar la propiedad Municipio
                usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado(); //Inicializar la propiedad Estado

                //Condición para mostrar ROL
                if (usuario.IdUsuario == null)
                {
                    usuario.Rol = new ML.Rol();
                }
                else
                {
                    ML.Result result = BL.Usuario.GetById(usuario.IdUsuario);
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
            return Json(JsonResult, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult CargaMasiva()
        {
            string mensaje = "";
            if (Session["RutaExcel"] == null)
            {
                //La primera vez que voy a ller y validar un excel
                HttpPostedFileBase excelUsuario = Request.Files["inptExcel"];

                string extensionPermitida = ".xlsx";

                if (excelUsuario.ContentLength > 0) //El usuario si me dio un archivo
                {
                    string extensionObtenida = Path.GetExtension(excelUsuario.FileName);
                    if (extensionObtenida == extensionPermitida)
                    {
                        //Primero crear carpeta para guardar copias del archivo (PL_Web>Add>Carpeta>CargaMasiva)

                        //Ruta Relativa, guardar ahí el archivo concatenando fecha, hora, etc.
                        string ruta = Server.MapPath("~/CargaMasiva/") +
                            Path.GetFileNameWithoutExtension(excelUsuario.FileName) +
                            "-" + DateTime.Now.ToString("ddMMyyyyHmmssff") + ".xlsx";

                        if (!System.IO.File.Exists(ruta))    //Pegar la cadena de conexión de OLEDB (Web.config)
                        {
                            excelUsuario.SaveAs(ruta);
                            string cadenaConexion = ConfigurationManager.ConnectionStrings["OleDbConnection"] + ruta;

                            ML.Result resultExcel = BL.Usuario.LeerExcel(cadenaConexion);

                            if (resultExcel.Objects.Count > 0) //Si el objeto trae más de 1 entonces o leyó y capturó datos
                            {
                                ML.ResultExcel resultValidacion = BL.Usuario.ValidarExcel(resultExcel.Objects); //Validar, le mandamos la lista de Objetos

                                if (resultValidacion.Errores.Count > 0) //Si la lista de Errores es mayor a cero (hay errores)
                                {
                                    //hubo un error (mostrar una vista, una tabla)
                                    //ViewBag=> Es una variable que se pasa de un CONTROLADOR a una VISTA
                                    //Si el ViewBag se lee en la VISTA, el valor se DESTRUYE
                                    ViewBag.ErroresExcel = resultValidacion.Errores; //Ahora ir a la vista a colocar el ViewBag (hasta arriba despues del model)

                                    //El error mostrarlo en la vista Modal:
                                    return PartialView("_Modal");
                                }
                                else
                                {
                                    //Session en C# => variable global, vive 60 min (determinado) en todo el proyecto
                                    Session["RutaExcel"] = ruta;
                                    mensaje = "Excel sin errores";
                                    ViewBag.Mensaje = mensaje;
                                    return PartialView("_Modal");
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMensaje = "Hola";
                                return PartialView("_Modal");
                            }
                        }
                        else
                        {
                            //Vista Parcial (vuelve a cargar el archivo, porque ya existe)
                            ViewBag.ErrorMensaje = "Vuelva a cargar el archivo, porque ya existe";
                            return PartialView("_Modal");
                        }
                    }
                    else
                    {
                        //Vista Parcial (El archivo no es un Excel)
                        mensaje = "El archivo no es un excel";
                        ViewBag.Mensaje = mensaje;
                        return PartialView("_Modal");

                    }
                }
                else
                {
                    //Vistas parciales (No me diste ningún archivo)
                    mensaje = "No seleccionó ningún archivo";
                    ViewBag.Mensaje = mensaje;
                    return PartialView("_Modal");
                }
            }
            else
            {
                //Ya leí y validé un excel
                //INSERTAR
                string cadenaConexion = ConfigurationManager.ConnectionStrings["OleDbConnection"] + Session["RutaExcel"].ToString();

                ML.Result resultLeer = BL.Usuario.LeerExcel(cadenaConexion);

                if (resultLeer.Objects.Count > 0)
                {
                    //Todo lo leyó bien
                    foreach (ML.Usuario usuario in resultLeer.Objects)
                    {
                        ML.Result resultInsertar = BL.Usuario.Add(usuario);
                        if (!resultInsertar.Correct)
                        {
                            Session["RutaExcel"] = null;
                            //mostrar el error que salió
                            mensaje = "El archivo contiene datos duplicados, no se insertará";
                            ViewBag.Mensaje = mensaje;
                            return PartialView("_Modal");
                        }
                    }
                    //cuantos Insertes fueron Correctos y cuantos Incorrectos
                    //Cuales estuvieron mal
                }
                else
                {
                    //error
                    mensaje = "Ocurrió un error y no se insertó";
                    ViewBag.Mensaje = mensaje;
                    return PartialView("_Modal");
                }
            }

            Session["RutaExcel"] = null; //antes de mandar a la vista, Session dejarla en null para poder usar el método nuevamente

            //Devolver a la vista GetAll
            return RedirectToAction("GetAll");
        }
    }
}