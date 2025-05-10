using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Producto
{
    public class Producto
    {
        public int IdProducto { get; set; }

        [DisplayName("Nombre del Producto")]
        [Required(ErrorMessage = "Nombre de Producto es un campo obligatorio")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ]+(?:\s+[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ]+){0,2}$", ErrorMessage = "Nombre inválido")]
        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [DisplayName("Precio")]
        [Required(ErrorMessage = "Precio es un campo obligatorio")]
        [RegularExpression(@"^\d{0,8}(\.\d{1,4})?$", ErrorMessage = "Precio inválido")]
        public decimal Precio { get; set; }

        [DisplayName("Imágen")]
        public byte[] Imagen { get; set; }
        public string ImagenBase64 { get; set; }

        public List<object> Productos { get; set; }
        public ML.Producto.Subcategoria Subcategoria { get; set; }
        public ML.Producto.Sucursal Sucursal { get; set; }
        public ML.Producto.ProductoSucursal ProductoSucursal {get; set;}
    }
}
