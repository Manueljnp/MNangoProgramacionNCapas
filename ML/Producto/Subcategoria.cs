using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Producto
{
    public class Subcategoria
    {
        [DisplayName("Subcategoría")]
        [Required(ErrorMessage = "Debe seleccionar una Subcategoría")]
        public int IdSubcategoria { get; set; }
        public string Nombre { get; set; }
        public List<Object> Subcategorias { get; set; }
        public ML.Producto.Categoria Categoria { get; set; }
    }
}
