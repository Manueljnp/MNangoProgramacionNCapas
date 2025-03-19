using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Municipio
    {
        [DisplayName ("Municipio")]
        [Required (ErrorMessage = "Debe seleccionar un Municipio")]
        public int IdMunicipio { get; set; }

        public string Nombre { get; set; }
        public List<Object> Municipios { get; set; }
        public ML.Estado Estado { get; set; }
    }
}
