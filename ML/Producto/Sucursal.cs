using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Producto
{
    public class Sucursal
    {
        public int IdSucursal { get; set; }
        public string Nombre { get; set; }
        public string Latitud {  get; set; }
        public string Longitud { get; set; }
        public List<Object> Sucursales { get; set; }
    }
}
