using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Rol
    {
        [DisplayName ("Seleccione un Rol")]
        //puede ser NULL
        public int IdRol {  get; set; }
        public string Nombre { get; set; }
        public List<Object> Roles { get; set; }
    }
}
