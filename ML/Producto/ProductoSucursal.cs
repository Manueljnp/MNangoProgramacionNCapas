using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Producto
{
    public class ProductoSucursal
    {
        public int IdProductoSucursal { get; set; }
        public int IdProducto {  get; set; }
        public int IdSucursal { get; set; }
        public int Stock {  get; set; }
        public List<object> ProductosSucursales { get; set; }
    }
}
