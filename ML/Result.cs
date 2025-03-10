using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Result //Si todo está bien o hubo un error, para mostrarselo al usuario
    {
        public bool Correct { get; set; } //FALSE = Error / TRUE = Proceso correcto
        public string ErrorMessage { get; set; } //Cuál es el error ESPECIFICO
        public Exception Ex { get; set; } //Traer TODO el error a DETALLE
        public object Object { get; set; } //Mostrar 1 Registro (object)
        public List<object> Objects { get; set; } //Mostrar N RegistroS (objectS)
    }
}
