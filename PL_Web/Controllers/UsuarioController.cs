using BL;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

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

            //Llenar Roles NORMAL
            ML.Result resultDDLe = BL.Rol.GetAll();

            //Consumir WebApi
            ML.Result result = GetAllREST(usuario);

            //Líneas para consumir método de WCF SOAP (Usando Service Reference)
            /*UsuarioReference.UsuarioClient objeto = new UsuarioReference.UsuarioClient();
            var result = objeto.GetAll(usuario);*/

            // Método con WebService (SOAP)
            /*string xmlResult = GetAllXMLSOAP(); // Llama al método SOAP para obtener el XML

            if (!string.IsNullOrEmpty(xmlResult))
            {
                // Usa el método GetAllXML para deserializar el XML
                var usuarioResult = GetAllXML(xmlResult);
                usuario.Usuarios = usuarioResult.Usuarios ?? new List<object>();
            }
            else
            {
                usuario.Usuarios = new List<object>();
            }*/


            //Agregar .ToList() => para Web Services (Me funcionó sin esto)
            usuario.Rol.Roles = resultDDLe.Objects.ToList();

            //ML.Result result = BL.Usuario.GetAll(usuario);

            //(En SOAP comenté esto)
            if (result.Correct)
            {
                //Obtuvo toda la información
                usuario.Usuarios = result.Objects.ToList();
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
            /*else
            {
                UsuarioReference.UsuarioClient objeto = new UsuarioReference.UsuarioClient();
                var result = objeto.GetById(IdUsuario.Value);

                //ML.Result result = BL.Usuario.GetById(IdUsuario.Value);   //Para llamar al método sin WCF
                usuario = (ML.Usuario)result.Object;

                ML.Result resultMunicipio = BL.Municipio.GetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                usuario.Direccion.Colonia.Municipio.Municipios = resultMunicipio.Objects;

                ML.Result resultColonia = BL.Colonia.GetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);
                usuario.Direccion.Colonia.Colonias = resultColonia.Objects;
            }*/
            else
            {
                // Usar el método SOAP para obtener el usuario por ID.
                //usuario = GetByIdXMLSOAP((int)IdUsuario);

                //Mandar a llamar Método para Api REST
                ML.Result respuestaREST = GetByIdREST((int) IdUsuario);

                // Si se encuentra el usuario, cargar Municipios y Colonias relacionados.
                if (respuestaREST != null)
                {
                    //Asignarle el valor de respuestaREST a usuario: (Comentar si no se usa la ApiREST)
                    usuario = (ML.Usuario)respuestaREST.Object;

                    ML.Result resultMunicipio = BL.Municipio.GetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                    usuario.Direccion.Colonia.Municipio.Municipios = resultMunicipio.Objects;

                    ML.Result resultColonia = BL.Colonia.GetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);
                    usuario.Direccion.Colonia.Colonias = resultColonia.Objects;
                }
                else
                {
                    ViewBag.Error = "No se pudo obtener información del usuario."; // Manejo de errores.
                }
            }


            //Obtener los Roles
            ML.Result resultRoles = BL.Rol.GetAll();
            usuario.Rol.Roles = resultRoles.Objects;

            ML.Result resultEstados = BL.Estado.GetAll();
            usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstados.Objects;

            //suario.Rol = new ML.Rol();
            return View(usuario);
        }

        //Este es para Agregar y Actualizar (Validaciones DATA ANNOTATION)
        /*public ActionResult Form(ML.Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Validation success.
            }
            else
            {/*
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

            string mensaje = "";
            //Obtener el archivo de la Imagen:      Nombre del input (o el id?)
            HttpPostedFileBase file = Request.Files["inptFileImagen"];
            if (file != null /*&& file.ContentLength > 0)
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
            }

            UsuarioReference.UsuarioClient objeto = new UsuarioReference.UsuarioClient();

            if (usuario.IdUsuario == 0)
            {
                //ADD CON DIRECCION (Cambiar a 'ADD' al finalizar pruebas) **************************************************************

                //Para usar método ADD sin WebService
                //BL.Usuario.Add(usuario);


                var resultDDL = objeto.Add(usuario);

                mensaje = "Usuario agregado correctamente";
            }
            else
            {
                //UPDATE  Para usar método Update sin WebService
                //BL.Usuario.Update(usuario);

                var resultDDL = objeto.Update(usuario);

                mensaje = "Usuario actualizado correctamente";

            }

            ViewBag.Mensaje = mensaje;
            return PartialView("_Partial");

        }*/

        [HttpPost]
        public ActionResult Form(ML.Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                string mensaje = "";

                // Obtener imagen desde el formulario
                HttpPostedFileBase file = Request.Files["inptFileImagen"];
                if (file != null && file.ContentLength > 0)
                {
                    usuario.Imagen = ConvertirAArrayBytes(file);
                }

                //Método Agregar/Actualizar con SOAP
                //ML.Result result = InsertUpdate(usuario);

                //WebApi REST:
                ML.Result result;
                if(usuario.IdUsuario == 0)
                {
                    result = AddREST(usuario);
                }
                else
                {
                    result = UpdateREST(usuario);
                }

                //Mensajes de error para el viewBag
                if (result.Correct)
                {
                    mensaje = usuario.IdUsuario == 0 ? "Usuario agregado correctamente" : "Usuario actualizado correctamente";
                }
                else
                {
                    mensaje = usuario.IdUsuario == 0 ? "Error al agregar el usuario" : "Error al actualizar el usuario";
                }

                ViewBag.Mensaje = mensaje;
                return PartialView("_Partial");
            }

            return View(usuario);
        }

        [HttpGet]
        public ActionResult Delete(int IdUsuario)
        {
            //SERVICE REFERENCE
            //UsuarioReference.UsuarioClient objeto = new UsuarioReference.UsuarioClient();
            //var result = objeto.Delete(IdUsuario);

            //Normal llamando a BL (capa de negocios)
            //BL.Usuario.Delete(IdUsuario);

            //Usando SOAP creando XML:
            //bool result = DeleteXMLSOAP(IdUsuario);

            ML.Result result = DeleteREST(IdUsuario);

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

                string extensionExcel = ConfigurationManager.AppSettings["ExtensionExcel"].ToString();

                string extensionPermitida = extensionExcel;

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


        //WEB SERVICES  XML

        //GET ALL
        //Crear XML:
        [NonAction]
        private string GetAllXMLSOAP()
        {
            string action = "http://tempuri.org/IUsuario/GetAll";
            string url = "http://localhost:58695/Usuario.svc";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("SOAPAction", action);
                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.Accept = "text/xml";
                request.Method = "POST";

                // Crear el sobre SOAP
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
        <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"" xmlns:ml=""http://schemas.datacontract.org/2004/07/ML"" xmlns:arr=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:GetAll>
         <!--Optional:-->
         <tem:usuarioObj>
            <!--Optional:-->
            <ml:ApellidoMaterno></ml:ApellidoMaterno>
            <!--Optional:-->
            <ml:ApellidoPaterno></ml:ApellidoPaterno>
            <!--Optional:-->
            <ml:Nombre></ml:Nombre>
            
            <ml:Rol>
               <!--Optional:-->
               <ml:IdRol>0</ml:IdRol>
            </ml:Rol>
            
         </tem:usuarioObj>
      </tem:GetAll>
   </soapenv:Body>
</soapenv:Envelope>";

                // Enviar solicitud
                using (Stream stream = request.GetRequestStream())
                {
                    byte[] content = Encoding.UTF8.GetBytes(soapEnvelope);
                    stream.Write(content, 0, content.Length);
                }

                // Obtener la respuesta
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string result = reader.ReadToEnd();
                        return result; // Devuelve el XML como string
                    }
                }
            }
            catch (WebException ex)
            {
                // Registrar el error o mostrarlo en la vista
                ViewBag.Error = ex.Message;
                return null;
            }
        }

        //Deserealizar:
        [HttpGet]
        [NonAction]
        private ML.Usuario GetAllXML(string xml)
        {
            var usuario1 = new ML.Usuario();
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            var xdoc = XDocument.Parse(xml);

            //Definir los namespaces
            XNamespace nsA = "http://schemas.datacontract.org/2004/07/SL_WCF";    //Por si acaso lo ocupo
            XNamespace nsB = "http://schemas.microsoft.com/2003/10/Serialization/Arrays";
            XNamespace nsC = "http://schemas.datacontract.org/2004/07/ML";

            // Acceder a los objetos dentro de "Objects"
            var objects = xdoc.Descendants(nsB + "anyType");

            foreach (var elem in objects)
            {
                var usuario = new ML.Usuario();

                // IdUsuario
                int.TryParse(elem.Element(nsC + "IdUsuario")?.Value, out int idUsuario);
                usuario.IdUsuario = idUsuario;

                //user
                usuario.UserName = (string)elem.Element(nsC + "UserName")?.Value ?? string.Empty;
                //Nombre
                usuario.Nombre = (string)(elem.Element(nsC + "Nombre")?.Value ?? string.Empty);

                // Apellido Paterno y Materno
                usuario.ApellidoPaterno = (string)(elem.Element(nsC + "ApellidoPaterno")?.Value ?? string.Empty);
                usuario.ApellidoMaterno = (string)(elem.Element(nsC + "ApellidoMaterno")?.Value ?? string.Empty);

                // Email
                usuario.Email = (string)(elem.Element(nsC + "Email")?.Value ?? string.Empty);

                // Password
                usuario.Password = (string)(elem.Element(nsC + "Password")?.Value ?? string.Empty);

                // Fecha de Nacimiento
                usuario.FechaNacimiento = (string)(elem.Element(nsC + "FechaNacimiento")?.Value ?? string.Empty);

                // Sexo
                usuario.Sexo = (string)(elem.Element(nsC + "Sexo")?.Value ?? string.Empty);

                // Teléfono
                usuario.Telefono = (string)(elem.Element(nsC + "Telefono")?.Value ?? string.Empty);

                // Celular
                usuario.Celular = (string)(elem.Element(nsC + "Celular")?.Value ?? string.Empty);

                // Estatus
                usuario.Estatus = true; // Si necesitas que sea dinámico, añade la lógica aquí.

                // CURP
                usuario.CURP = (string)(elem.Element(nsC + "CURP")?.Value ?? string.Empty);

                // Instanciar el objeto Rol antes de asignarle valores
                usuario.Rol = new ML.Rol();
                var rolElement = elem.Element(nsC + "Rol");
                usuario.Rol.Nombre = (string)(rolElement.Element(nsC + "Nombre")?.Value ?? string.Empty);

                //Inicializar los objetos para Dirección:
                usuario.Direccion = new ML.Direccion();
                usuario.Direccion.Colonia = new ML.Colonia();
                usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();

                //Dirección:
                var direccion = elem.Element(nsC + "Direccion");

                //Asignación de información de Dirección (Calle, Numero Interior/exterior)
                usuario.Direccion.Calle = (string)(direccion.Element(nsC + "Calle")?.Value ?? string.Empty);
                usuario.Direccion.NumeroInterior = (string)(direccion.Element(nsC + "NumeroInterior")?.Value ?? string.Empty);
                usuario.Direccion.NumeroExterior = (string)(direccion.Element(nsC + "NumeroExterior")?.Value ?? string.Empty);

                //Asignación de información de Colonia (Nombre y Codigo Postal)
                usuario.Direccion.Colonia.Nombre = (string)(direccion.Element(nsC + "Colonia")?.Element(nsC + "Nombre")?.Value ?? string.Empty);
                usuario.Direccion.Colonia.CodigoPostal = (string)(direccion.Element(nsC + "Colonia")?.Element(nsC + "CodigoPostal")?.Value ?? string.Empty);

                //Asignación de información de Municipio
                usuario.Direccion.Colonia.Municipio.Nombre = (string)(direccion.Element(nsC + "Colonia")?.Element(nsC + "Municipio")?.Element(nsC + "Nombre")?.Value ?? string.Empty);

                //Asignación de información de Estado
                usuario.Direccion.Colonia.Municipio.Estado.Nombre = (string)(direccion.Element(nsC + "Colonia")?.Element(nsC + "Municipio")?.Element(nsC + "Estado")?.Element(nsC + "Nombre")?.Value ?? string.Empty);

                // Añadir el objeto al resultado
                result.Objects.Add(usuario);
            }

            //FALATABA ESTO (Asignarle los valores a usuario1)
            usuario1.Usuarios = result.Objects;

            //Devuelve el objeto completo
            return usuario1;
        }


        //GETBYID
        [NonAction]
        private ML.Usuario GetByIdXMLSOAP(int idUsuario)
        {
            string action = "http://tempuri.org/IUsuario/GetById";
            string url = "http://localhost:58695/Usuario.svc";

            try
            {
                // Configuración del request SOAP
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("SOAPAction", action);
                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.Accept = "text/xml";
                request.Method = "POST";

                // Crear el sobre SOAP para obtener el usuario por ID
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"" xmlns:ml=""http://schemas.datacontract.org/2004/07/ML"" xmlns:arr=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:GetById>
         <tem:IdUsuario>{idUsuario}</tem:IdUsuario>
      </tem:GetById>
   </soapenv:Body>
</soapenv:Envelope>";


                // Enviar la solicitud
                using (Stream stream = request.GetRequestStream())
                {
                    byte[] content = Encoding.UTF8.GetBytes(soapEnvelope);
                    stream.Write(content, 0, content.Length);
                }

                // Obtener la respuesta
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string xmlResponse = reader.ReadToEnd();
                        return GetByIdXML(xmlResponse); //Llamar al método para deserializar la respuesta
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null; // Manejo de errores en caso de fallo
            }
        }

        [HttpGet]
        [NonAction]
        private ML.Usuario GetByIdXML(string xml)
        {
            //var usuario1 = new ML.Usuario();
            //ML.Result result = new ML.Result();
            //result.Object

            var xdoc = XDocument.Parse(xml);

            //Definir namespaces para acceder a los datos
            XNamespace nsA = "http://schemas.datacontract.org/2004/07/ML";

            // Buscar el elemento "Object" dentro del resultado
            //var usuarioElement = xdoc.Descendants(nsA + "Usuario").FirstOrDefault();

            var usuarioElement = xdoc.Descendants().FirstOrDefault(e =>
                e.Name.LocalName == "Object" &&
                e.GetDefaultNamespace().NamespaceName == "http://tempuri.org/");

            if (usuarioElement != null)
            {
                var usuario = new ML.Usuario
                {
                    IdUsuario = int.TryParse(usuarioElement.Element(nsA + "IdUsuario")?.Value, out int idUsuario) ? idUsuario : 0,
                    UserName = usuarioElement.Element(nsA + "UserName")?.Value ?? string.Empty,
                    Nombre = usuarioElement.Element(nsA + "Nombre")?.Value ?? string.Empty,
                    ApellidoPaterno = usuarioElement.Element(nsA + "ApellidoPaterno")?.Value ?? string.Empty,
                    ApellidoMaterno = usuarioElement.Element(nsA + "ApellidoMaterno")?.Value ?? string.Empty,
                    Email = usuarioElement.Element(nsA + "Email")?.Value ?? string.Empty,
                    Password = usuarioElement.Element(nsA + "Password")?.Value ?? string.Empty,
                    FechaNacimiento = usuarioElement.Element(nsA + "FechaNacimiento")?.Value ?? string.Empty,
                    Sexo = usuarioElement.Element(nsA + "Sexo")?.Value ?? string.Empty,
                    Telefono = usuarioElement.Element(nsA + "Telefono")?.Value ?? string.Empty,
                    Celular = usuarioElement.Element(nsA + "Celular")?.Value ?? string.Empty,
                    CURP = usuarioElement.Element(nsA + "CURP")?.Value ?? string.Empty,
                    Estatus = true // Esto lo puedes adaptar según los datos recibidos
                };

                // Deserializar el Rol del Usuario
                var rolElement = usuarioElement.Element(nsA + "Rol");
                if (rolElement != null)
                {
                    usuario.Rol = new ML.Rol
                    {
                        IdRol = int.TryParse(rolElement.Element(nsA + "IdRol")?.Value, out int idRol) ? idRol : 0,
                        Nombre = rolElement.Element(nsA + "Nombre")?.Value ?? string.Empty
                    };
                }

                var direccionElement = usuarioElement.Element(nsA + "Direccion");
                if (direccionElement != null)
                {
                    usuario.Direccion = new ML.Direccion
                    {
                        Calle = direccionElement.Element(nsA + "Calle")?.Value ?? string.Empty,
                        NumeroExterior = direccionElement.Element(nsA + "NumeroExterior")?.Value ?? string.Empty,
                        NumeroInterior = direccionElement.Element(nsA + "NumeroInterior")?.Value ?? string.Empty
                    };
                }

                //Dirección:
                var coloniaElement = usuarioElement.Element(nsA + "Direccion");
                if (coloniaElement != null)
                {
                    usuario.Direccion.Colonia = new ML.Colonia
                    {
                        IdColonia = int.TryParse(coloniaElement.Element(nsA + "Colonia")?.Element(nsA + "IdColonia")?.Value, out int idColonia) ? idColonia : 0
                    };
                }

                var municipioElement = usuarioElement.Element(nsA + "Direccion");
                if (municipioElement != null)
                {
                    usuario.Direccion.Colonia.Municipio = new ML.Municipio
                    {
                        IdMunicipio = int.TryParse(municipioElement.Element(nsA + "Colonia")?.Element(nsA + "Municipio")?.Element(nsA + "IdMunicipio")?.Value, out int idMunicipio) ? idMunicipio : 0
                    };
                }

                var estadoElement = usuarioElement.Element(nsA + "Direccion");
                if (estadoElement != null)
                {
                    usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado
                    {
                        IdEstado = int.TryParse(estadoElement.Element(nsA + "Colonia")?.Element(nsA + "Municipio")?.Element(nsA + "Estado")?.Element(nsA + "IdEstado")?.Value, out int idEstado) ? idEstado : 0
                    };
                }

                /*
                usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();

                //Asignación de información de Colonia (Nombre y Codigo Postal)
                usuario.Direccion.Colonia.Nombre = (string)(direccion.Element(nsA + "Colonia")?.Element(nsA + "Nombre")?.Value ?? string.Empty);
                usuario.Direccion.Colonia.CodigoPostal = (string)(direccion.Element(nsA + "Colonia")?.Element(nsA + "CodigoPostal")?.Value ?? string.Empty);

                //Asignación de información de Municipio
                usuario.Direccion.Colonia.Municipio.Nombre = (string)(direccion.Element(nsA + "Colonia")?.Element(nsA + "Municipio")?.Element(nsA + "Nombre")?.Value ?? string.Empty);

                //Asignación de información de Estado
                usuario.Direccion.Colonia.Municipio.Estado.Nombre = (string)(direccion.Element(nsA + "Colonia")?.Element(nsA + "Municipio")?.Element(nsA + "Estado")?.Element(nsA + "Nombre")?.Value ?? string.Empty);
                */

                return usuario;
            }

            return null; // En caso de que no haya datos válidos
        }


        //INSERT/UPDATE

        //Crear XML
        private string CrearXmlUsuario(ML.Usuario usuario, bool esInsertar)
        {
            string accion = esInsertar ? "AgregarUsuario" : "ActualizarUsuario";
            string etiqueta = esInsertar ? "Add" : "Update";

            string xml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
    <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"" xmlns:ml=""http://schemas.datacontract.org/2004/07/ML"">
       <soapenv:Header/>
       <soapenv:Body>
          <tem:{etiqueta}>
             <tem:usuario>
                <ml:ApellidoMaterno>{usuario.ApellidoMaterno}</ml:ApellidoMaterno>
                <ml:ApellidoPaterno>{usuario.ApellidoPaterno}</ml:ApellidoPaterno>
                <ml:CURP>{usuario.CURP}</ml:CURP>
                <ml:Celular>{usuario.Celular}</ml:Celular>
                <ml:Direccion>
                   <ml:Calle>{usuario.Direccion.Calle}</ml:Calle>
                   <ml:Colonia>
                      <ml:IdColonia>{usuario.Direccion.Colonia.IdColonia}</ml:IdColonia>
                      <ml:Municipio>
                         <ml:Estado>
                            <ml:IdEstado>{usuario.Direccion.Colonia.Municipio.Estado.IdEstado}</ml:IdEstado>
                         </ml:Estado>
                         <ml:IdMunicipio>{usuario.Direccion.Colonia.Municipio.IdMunicipio}</ml:IdMunicipio>
                      </ml:Municipio>
                   </ml:Colonia>
                   <ml:NumeroExterior>{usuario.Direccion.NumeroExterior}</ml:NumeroExterior>
                   <ml:NumeroInterior>{usuario.Direccion.NumeroInterior}</ml:NumeroInterior>
                </ml:Direccion>
                <ml:Email>{usuario.Email}</ml:Email>
                <ml:FechaNacimiento>{usuario.FechaNacimiento}</ml:FechaNacimiento>";

            if (!esInsertar)
            {
                xml += $"<ml:IdUsuario>{usuario.IdUsuario}</ml:IdUsuario>";
            }

            xml += $@"
                <ml:Imagen>{usuario.Imagen}</ml:Imagen>
                <ml:Nombre>{usuario.Nombre}</ml:Nombre>
                <ml:Password>{usuario.Password}</ml:Password>
                <ml:Rol>
                   <ml:IdRol>{usuario.Rol.IdRol}</ml:IdRol>
                </ml:Rol>
                <ml:Sexo>{usuario.Sexo}</ml:Sexo>
                <ml:Telefono>{usuario.Telefono}</ml:Telefono>
                <ml:UserName>{usuario.UserName}</ml:UserName>
             </tem:usuario>
          </tem:{etiqueta}>
       </soapenv:Body>
    </soapenv:Envelope>";

            return xml;
        }

        [NonAction]
        private ML.Result InsertUpdate(ML.Usuario usuario)
        {
            string url = "http://localhost:58695/Usuario.svc";
            bool esInsertar = usuario.IdUsuario == 0;
            string action = esInsertar ? "http://tempuri.org/IUsuario/Add"
                                       : "http://tempuri.org/IUsuario/Update";

            string soapEnvelope = CrearXmlUsuario(usuario, esInsertar);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("SOAPAction", action);
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Method = "POST";

            using (Stream stream = request.GetRequestStream())
            {
                byte[] content = Encoding.UTF8.GetBytes(soapEnvelope);
                stream.Write(content, 0, content.Length);
            }

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseString = reader.ReadToEnd();

                        // Leer respuesta SOAP
                        var xdoc = XDocument.Parse(responseString);
                        var resultElement = xdoc.Descendants()
                                                .FirstOrDefault(e => e.Name.LocalName == "Correct" &&
                                                                     e.GetDefaultNamespace().NamespaceName == "http://tempuri.org/");

                        ML.Result result = new ML.Result
                        {
                            Correct = resultElement != null && bool.Parse(resultElement.Value)
                        };

                        return result;
                    }
                }
            }
            catch (WebException ex)
            {
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    string errorResponse = reader.ReadToEnd();
                    throw new Exception($"Error en el servicio: {errorResponse}");
                }
            }
        }

        //DELETE
        [NonAction]
        private bool DeleteXMLSOAP(int idUsuario)
        {
            string action = "http://tempuri.org/IUsuario/Delete";
            string url = "http://localhost:58695/Usuario.svc";

            try
            {
                // Configuración del request SOAP
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("SOAPAction", action);
                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.Accept = "text/xml";
                request.Method = "POST";

                // Crear el sobre SOAP para la eliminación
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:Delete>
         <tem:idUsuario>{idUsuario}</tem:idUsuario>
      </tem:Delete>
   </soapenv:Body>
</soapenv:Envelope>";

                // Enviar la solicitud
                using (Stream stream = request.GetRequestStream())
                {
                    byte[] content = Encoding.UTF8.GetBytes(soapEnvelope);
                    stream.Write(content, 0, content.Length);
                }

                // Obtener la respuesta
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string xmlResponse = reader.ReadToEnd();
                        return true;
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false; // Manejo de errores en caso de fallo
            }
        }



        //Consumir WebApi en MVC con HttpClient
        //API REST

        //Primero Instalar en PL (web):
        //Json.NET => https://www.nuget.org/packages/Newtonsoft.Json/
        //Microsoft ASP.Net Web API 2.2 client Libraries => https://nugetmusthaves.com/Package/Microsoft.AspNet.WebApi.Client
        //System.Net.Http => https://www.nuget.org/packages/System.Net.Http/

        [NonAction]
        public ML.Result GetAllREST(ML.Usuario user)
        {
            ML.Result result = new ML.Result();
            
            /*ML.Usuario user = new ML.Usuario
            {
                Nombre = "",
                ApellidoPaterno = "",
                ApellidoMaterno = "",

                Rol = new ML.Rol
                {
                    IdRol = 0
                }
            };*/

            try
            {
                using (var cliente = new HttpClient())
                {
                    //URL vs URI
                    string endPoint = ConfigurationManager.AppSettings["UsuarioEndPoint"].ToString();

                    cliente.BaseAddress = new Uri(endPoint);

                    //Primero serializar el objeto 'user' a JSON:
                    //(YA NO LO USÉ, no serializo el modelo, envío directamente el modelo)
                    //var jsonUser = new StringContent(JsonConvert.SerializeObject(user));

                    //las solicitudes GET no pueden mandar información en body,
                    //usar POST - PUT - PATCH | usé Post y le envío el Modelo
                    var respuesta = cliente.PostAsJsonAsync<ML.Usuario>("GetAll", user);

                    respuesta.Wait(); //Esperar

                    var resultServicio = respuesta.Result; //Es otro Result

                    if(resultServicio.IsSuccessStatusCode)
                    {
                        //Leer Tarea = Leer el contenido de resultServicio como un Modelo de Result
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        //Inicializar la lista de Objetos
                        result.Objects = new List<object>();

                        //sacar los objetos de tipo JSON que contiene readTask
                        foreach(var item in readTask.Result.Objects)
                        {
                            ML.Usuario usuario = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(item.ToString());

                            result.Objects.Add(usuario);
                        }
                    }
                }
                result.Correct = true;

            }
            catch(Exception xd)
            {
                result.Correct = false;
                result.ErrorMessage = xd.Message;
                result.Ex = xd;
            }

            return result;
        }


        [NonAction]
        public static ML.Result GetByIdREST(int idUsuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (var cliente = new HttpClient())
                {
                    //URL vs URI
                    string endPoint = ConfigurationManager.AppSettings["UsuarioEndPoint"].ToString();

                    cliente.BaseAddress = new Uri(endPoint);

                    //Primero serializar el objeto 'user' a JSON:
                    //(YA NO LO USÉ, no serializo el modelo, envío directamente el modelo)
                    //var jsonUser = new StringContent(JsonConvert.SerializeObject(user));

                    //las solicitudes GET no pueden mandar información en body,
                    //usar POST - PUT - PATCH | usé Post y le envío el Modelo
                    var respuesta = cliente.GetAsync($"GetById/{idUsuario}");

                    respuesta.Wait(); //Esperar

                    var resultServicio = respuesta.Result; //Es otro Result

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        //Leer Tarea = Leer el contenido de resultServicio como un Modelo de Result
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        //Inicializar la lista de Objetos
                        result.Object = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(readTask.Result.Object.ToString());

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception xd)
            {
                result.Correct = false;
                result.ErrorMessage = xd.Message;
                result.Ex = xd;
            }

            return result;
        }

        [NonAction]
        public ML.Result AddREST(ML.Usuario usuario)
        {
            using (var client = new HttpClient())
            {
                string endPoint = ConfigurationManager.AppSettings["UsuarioEndPoint"];
                client.BaseAddress = new Uri(endPoint);

                var postTask = client.PostAsJsonAsync("Add", usuario);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return new ML.Result { Correct = true };
                }
                else
                {
                    return new ML.Result { Correct = false };
                }
            }
        }

        [NonAction]
        public ML.Result UpdateREST(ML.Usuario usuario)
        {
            using (var client = new HttpClient())
            {
                string endPoint = ConfigurationManager.AppSettings["UsuarioEndPoint"];
                client.BaseAddress = new Uri(endPoint);

                var postTask = client.PutAsJsonAsync("Update", usuario);
                postTask.Wait();

                var result = postTask.Result;

                if(result.IsSuccessStatusCode)
                {
                    return new ML.Result
                    {
                        Correct = true
                    };
                }
                else
                {
                    return new ML.Result
                    {
                        Correct = false,
                        ErrorMessage = "Error al actualizar usuario"
                    };
                }
            }
        }

        [NonAction]
        public ML.Result DeleteREST(int idUsuario)
        {
            using (var client = new HttpClient())
            {
                string endPoint = ConfigurationManager.AppSettings["UsuarioEndPoint"];
                client.BaseAddress = new Uri(endPoint);
                
                //Envíar el idUsuario
                var deleteTask = client.DeleteAsync($"Delete/{idUsuario}");
                deleteTask.Wait();

                var respuesta = deleteTask.Result;
                if(respuesta.IsSuccessStatusCode)
                {
                    return new ML.Result
                    {
                        Correct = true
                    };
                }
                else
                {
                    return new ML.Result
                    {
                        Correct = false,
                        ErrorMessage = "Error al eliminar Usuario"
                    };
                }
            }
        }
    }
}