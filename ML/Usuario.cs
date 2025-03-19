using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

//https://www.aspsnippets.com

namespace ML
{
    //1. Definir la clase Usuario
    public class Usuario
    {
        //Define la estructura del usuario que se enviará a la BD
        public int IdUsuario { get; set; } //En BD se inicia en 1 y autoincrementa en 1

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "Nombre es un campo obligatorio")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ]+(?:\s+[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ]+){0,2}$", ErrorMessage = "Nombre inválido")]
        public string Nombre { get; set; }

        [DisplayName("Apellido Paterno")]
        [Required (ErrorMessage = "Apellido Paterno es un campo obligatorio")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Apellido Paterno inválido")]
        public string ApellidoPaterno { get; set; }

        [DisplayName("Apellido Materno")]
        //puede ser NULL
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Apellido Materno inválido")]
        public string ApellidoMaterno { get; set; }

        [DisplayName("Teléfono")]
        //puede ser NULL
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Teléfono inválido")]
        public string Telefono { get; set; }

        [DisplayName("User Name")]
        [Required (ErrorMessage = "User Name es un campo obligatorio")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Usuario inválido")]
        public string UserName { get; set; }

        [DisplayName("Email")]
        [Required (ErrorMessage = "Email es un campo obligatorio")]
        [RegularExpression(@"^[\w\-\.]+@([\w-]+\.)+[\w-]{2,}$", ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [DisplayName ("Password")]
        [Required (ErrorMessage = "Password es un campo obligatorio")]
        //[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Password inválido")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_]).{8}$", ErrorMessage = "Password inválido")]
        public string Password { get; set; }

        [DisplayName ("Fecha de Nacimiento")]
        [Required (ErrorMessage = "Fecha de Nacimiento es un campo obligatorio")]
        public string FechaNacimiento { get; set; }

        [DisplayName ("Sexo")]
        [Required(ErrorMessage = "Sexo es un campo obligatorio")]
        //[AllowEmptyStrings(false)]
        public string Sexo { get; set; }

        [DisplayName ("Celular")]
        //puede ser NULL
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Celular inválido")]
        public string Celular { get; set; }

        public bool Estatus { get; set; }

        [DisplayName ("CURP")]
        //puede ser NULL
        [RegularExpression("^([A-Z][AEIOUX][A-Z]{2}\\d{2}(?:0\\d|1[0-2])(?:[0-2]\\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\\d])(\\d)$", ErrorMessage = "CURP inválida")]
        public string CURP { get; set; }

        [DisplayName ("Imágen")]
        //puede ser NULL
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
