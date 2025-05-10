using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PL_Web.Controllers
{
    public class CorreoController : Controller
    {
        // GET: Correo
        public ActionResult Enviar()
        {
            ML.Usuario usuario = new ML.Usuario();
            ML.Result result = BL.Usuario.GetById(3);
            usuario = (ML.Usuario)result.Object;

            try
            {
                //Ya que el correo y contraseña dificilmente pueden cambiar, guardarlo en el Web.config
                string correo = ConfigurationManager.AppSettings["Correo"].ToString(); //Correo desde el que se manda
                string password = ConfigurationManager.AppSettings["PasswordCorreo"].ToString(); //Contraseña de aplicacion

                //*******************************************************

                //Si vamos a mandar correo con HTML como Body:
                
                string body = ""; //Declarar la variable que contendrá las etiquetas HTML
                string path = Server.MapPath("~/Content/Correo/PlantillaCorreo.html"); //Ruta del archivo HTML (alt + 126 => ~)

                StreamReader leer = new StreamReader(path); //Leer el archivo HTML

                body = leer.ReadToEnd(); //El body será todo el texto (etiquetas) del Html (LeerHastaElFinal)

                //Para reemplazar texto del HTML por valores (como Nombre o Enlaces especificos)
                body = body.Replace("{{NombreUsuario}}", usuario.Nombre);
                body = body.Replace("{{LINK}}", Url.Action("Index", "Home"));
                
                //********************************************************

                //Crear el SMTP Client
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    //Estos valores, NO DEJARlos aquí, ponerlos en Web.config
                    Port = Convert.ToInt16(ConfigurationManager.AppSettings["Puerto"]),
                    UseDefaultCredentials = false, //No usamos credenciales por default, usamos Contraseña de aplicación
                    Credentials = new NetworkCredential(correo, password), //Le mandamos el correo y contraseña de aplicación
                    EnableSsl = true //Habilitar SSL, de lo contrario se manda como spam
                };

                //Crear el mensaje
                var mensaje = new MailMessage
                {
                    From = new MailAddress(correo, "Manuel Nango"), //Nombre a Mostrar de quien envía
                    Subject = "Ofertas Especiales para ti", //Asunto
                    Body = body, //Enviar la variable body que ya tiene todas las etiquetas HTML
                    IsBodyHtml = true //El cuerpo SI es un HTML
                };

                mensaje.To.Add("manueljnp@hotmail.com"); //A quien vamos a mandar el correo
                smtpClient.Send(mensaje); //Enviar el correo
            }
            catch (Exception ex)
            {
                //MODAL
            }
            return View();
        }

        //Correo solo texto
        /*
        public ActionResult Enviar()
        {
            try
            {
                //Ya que el correo y contraseña dificilmente pueden cambiar, guardarlo en el Web.config
                string correo = ConfigurationManager.AppSettings["Correo"].ToString(); //Correo desde el que se manda
                string password = ConfigurationManager.AppSettings["PasswordCorreo"].ToString(); //Contraseña de aplicacion

                //*******************************************************

                //Si vamos a mandar correo con HTML como Body:
                /*
                string body = ""; //Declarar la variable que contendrá las etiquetas HTML
                string path = Server.MapPath(""); //Ruta del archivo HTML

                StreamReader leer = new StreamReader(path); //Leer el archivo HTML

                body = leer.ReadToEnd(); //El body será todo el texto (etiquetas) del Html (LeerHastaElFinal)

                //Para reemplazar texto del HTML por valores (como Nombre o Enlaces especificos)
                body = body.Replace("", "Pepe");
                body = body.Replace("{{LINK}}", Url.Action("Index", "Home"));
                
                //********************************************************

                //Crear el SMTP Client
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    //Estos valores, NO DEJARlos aquí, ponerlos en Web.config
                    Port = Convert.ToInt16(ConfigurationManager.AppSettings["Puerto"]),
                    UseDefaultCredentials = false, //No usamos credenciales por default, usamos Contraseña de aplicación
                    Credentials = new NetworkCredential(correo, password), //Le mandamos el correo y contraseña de aplicación
                    EnableSsl = true //Habilitar SSL, de lo contrario se manda como spam
                };

                //Crear el mensaje
                var mensaje = new MailMessage
                {
                    From = new MailAddress(correo, "Nombre a Mostrar"), //Nombre a Mostrar de quien envía
                    Subject = "Asunto", //Asunto
                    Body = "Primer Test, Solo texto", //Texto a enviar por correo
                    IsBodyHtml = false //El cuerpo no es un HTML
                };

                mensaje.To.Add("manueljnp@hotmail.com"); //A quien vamos a mandar el correo
                smtpClient.Send(mensaje); //Enviar el correo
            }
            catch (Exception ex)
            {
                //MODAL
            }
            return View();
        }
        */
    }
}