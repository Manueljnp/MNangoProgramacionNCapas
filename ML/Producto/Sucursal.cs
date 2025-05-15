using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Producto
{
    public class Sucursal
    {
        public int IdSucursal { get; set; }

        [DisplayName("Nombre Sucursal")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ]+(?:\s+[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ]+){0,5}$", ErrorMessage = "Nombre inválido")]
        [Required(ErrorMessage = "Agregue un nombre")]
        public string Nombre { get; set; }

        [DisplayName("Latitud")]
        [RegularExpression(@"^(\+|-)?(?:90(?:(?:\.0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\.[0-9]{1,6})?))$", ErrorMessage = "Latitud inválida")]
        [Required(ErrorMessage = "Agregue una Latitud")]
        public string Latitud {  get; set; }

        [DisplayName("Longitud")]
        [RegularExpression(@"^(\+|-)?(?:180(?:(?:\.0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\.[0-9]{1,6})?))$", ErrorMessage = "Longitud inválida")]
        [Required(ErrorMessage = "Agregue una Longitud")]
        public string Longitud { get; set; }
        public List<Object> Sucursales { get; set; }
    }
}
