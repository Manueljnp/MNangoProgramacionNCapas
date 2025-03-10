using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    //1. Definir la clase Usuario
    public class Usuario
    {
        //Define la estructura del usuario que se enviará a la BD
        public int IdUsuario { get; set; } //En BD se inicia en 1 y autoincrementa en 1
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Telefono { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string Celular { get; set; }
        public bool Estatus { get; set; }
        public string CURP { get; set; }
        public byte[] Imagen { get; set; }
       
        //public int ?IdRol { get; set; } //Para que al mostrar en AGREGAR, no venga el texbox con un 0
                                        //(por el if de BL>GetAll=>  IdRol == null )
        //public string RolNombre {  get; set; }

        //Aplicar Propiedad de Navegación
        public ML.Rol Rol {  get; set; }
        public List<object> Usuarios { get; set; }

        //COSAS QUE ESTOY AGREGANDO .............................................
        public ML.Direccion Direccion { get; set; }
    }
}
