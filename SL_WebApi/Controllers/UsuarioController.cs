using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SL_WebApi.Controllers
{
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {
        [HttpPost]
        [Route("suma")]
        public IHttpActionResult Suma(int a, int b)
        {
            return Content(HttpStatusCode.OK, a+b);
        }

        //
        [HttpPost]
        [Route("GetAll")]    //No sé que ruta deba ir aquí
        public IHttpActionResult GetAll([FromBody] ML.Usuario usuario) //[FromBody]: Permite recibir parámetros por BODY
        {

            usuario.Rol = new ML.Rol();

            //NO ES NECESARIO => el JSON del BODY en postman ya le envía esos parámetros vacíos
            //usuario.Nombre = "";
            //usuario.ApellidoPaterno = "";
            //usuario.ApellidoMaterno = "";
            //usuario.Rol.IdRol = 0;

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
        [Route("GetById/{idUsuario}")]
        public IHttpActionResult GetById(int idUsuario)
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
        [Route("Add")]
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
        [Route("Update")]
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
        [Route("Delete/{idUsuario}")]
        public IHttpActionResult Delete(int idUsuario)
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
