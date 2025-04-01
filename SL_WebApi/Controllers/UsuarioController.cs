using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SL_WebApi.Controllers
{
    [RoutePrefix("api")]
    public class UsuarioController : ApiController
    {
        [HttpPost]
        [Route("usuario/suma")]
        public IHttpActionResult Suma(int a, int b)
        {
            return Content(HttpStatusCode.OK, a+b);
        }

        //Intento de llamar al BL
        [HttpGet]
        [Route("usuario/GetAll")]    //No sé que ruta deba ir aquí
        public IHttpActionResult GetAll([FromBody] ML.Usuario usuario) //[FromBody]: Permite recibir parámetros por BODY
        {

            usuario.Rol = new ML.Rol();

            usuario.Nombre = "";
            usuario.ApellidoPaterno = "";
            usuario.ApellidoMaterno = "";
            usuario.Rol.IdRol = 0;

            ML.Result result = BL.Usuario.GetAll(usuario);

            if(result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, result.ErrorMessage);
            }

        }

        [HttpGet]
        [Route("usuario/GetById")]
        public IHttpActionResult GetById([FromBody] int idUsuario)
        {
            ML.Result result = BL.Usuario.GetById(idUsuario);

            if(result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, result.ErrorMessage);
            }
        }

        [HttpPost]
        [Route("usuario/Add")]
        public IHttpActionResult Add([FromBody] ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.Add(usuario);

            if(result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, result.ErrorMessage);
            }

        }

        [HttpPut]
        [Route("usuario/Update")]
        public IHttpActionResult Update([FromBody] ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.Update(usuario);

            if(result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, result.ErrorMessage);
            }
        }

        [HttpDelete]
        [Route("usuario/Delete")]
        public IHttpActionResult Delete([FromBody] int idUsuario)
        {
            ML.Result result = BL.Usuario.Delete(idUsuario);

            if(result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, result.ErrorMessage);
            }
        }
    }
}
