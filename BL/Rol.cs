using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Rol
    {
        //Usar LinQ para GetAll que nos servirá para hacer DropDownList
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var query = (from Rol in context.Rols
                                 select new
                                 {
                                     IdRol = Rol.IdRol,
                                     Nombre = Rol.Nombre
                                 }).ToList();

                    if(query.Count > 0 )
                    {
                        //trae registros
                        result.Objects = new List<object>();
                        //la instancia para poder guardar información

                        foreach(var item in query)
                        {
                            ML.Rol rol = new ML.Rol();
                            rol.Nombre = item.Nombre;
                            rol.IdRol = item.IdRol;

                            result.Objects.Add(rol);
                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct= false;
                    }
                }
            }
            catch(Exception e)
            {
                result.Correct = false;
                result.ErrorMessage = e.Message;
                result.Ex = e;
            }

            return result;
        }
    }
}
