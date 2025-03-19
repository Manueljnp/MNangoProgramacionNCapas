using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace ML
{
    public class Direccion
    {
        public int IdDireccion {  get; set; }

        [DisplayName ("Calle")]
        [Required (ErrorMessage = "Calle es un campo obligatorio")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ]+(?:\s+[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ]+){0,2}$", ErrorMessage = "Calle inválida")]
        public string Calle { get; set; }

        [DisplayName ("Numero Interior")]
        //puede ser NULL
        [RegularExpression("^([0-9]{1,5})$", ErrorMessage = "Número interior inválido")]
        public string NumeroInterior { get; set; }

        [DisplayName ("Numero Exterior")]
        [Required (ErrorMessage = "Número Exterior es un campo obligatorio")]
        [RegularExpression("^([0-9]{1,5})$", ErrorMessage = "Número Exterior inválido")]
        public string NumeroExterior { get; set; }

        public List<Object> Direcciones { get; set; }

        public ML.Colonia Colonia { get; set; }
    }
}
