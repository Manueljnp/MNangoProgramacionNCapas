using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Estado
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var query = (from Estado in context.Estadoes
                                 select new
                                 {
                                     IdEstado = Estado.IdEstado,
                                     Nombre = Estado.Nombre
                                 }).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach(var objBD in query)
                        {
                            ML.Estado estado = new ML.Estado();
                            estado.IdEstado = objBD.IdEstado;
                            estado.Nombre = objBD.Nombre;
                            result.Objects.Add(estado);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
    }
}
