using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class ResultExcel
    {
        public int NumeroRegistro { get; set; }
        public string ErrorMessage { get; set; }

        public List<object> Errores { get; set; }
    }
}
