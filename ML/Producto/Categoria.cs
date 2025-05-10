using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Producto
{
    public class Categoria
    {

        [DisplayName("Categoría")]
        [Required(ErrorMessage = "Debe seleccionar una Categoría")]
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public List<Object> Categorias { get; set; }

    }
}
