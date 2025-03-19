using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Estado
    {
        [DisplayName("Estado")]
        [Required (ErrorMessage = "Debe seleccionar un Estado")]
        public int IdEstado { get; set; }
        public string Nombre { get; set; }
        public List<Object> Estados { get; set; }
        //public ML.Municipio Municipio { get; set; }
    }
}
