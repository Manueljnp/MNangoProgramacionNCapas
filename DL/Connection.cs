using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration; //ESTO DEBE IR?????????

namespace DL
{
    public class Connection
    {
        public static string GetConnection()
        {
            string conexion = ConfigurationManager.ConnectionStrings["MNangoProgramacionNCapasBD"].ToString();            
            return conexion;
        }
    }
}
