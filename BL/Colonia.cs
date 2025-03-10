using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Colonia
    {
        public static ML.Result GetByIdMunicipio(int idMunicipio)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {   //Aunque sea GetById, en realidad traeremos N Municipios, por eso .ToList()
                    var query = context.ColoniaGetByIdMunicipio(idMunicipio).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var objBD in query)
                        {
                            ML.Colonia colonia = new ML.Colonia();
                            //ML.Usuario usuario = new ML.Usuario();
                            //usuario.Estado.Municipio.Colonia.IdColonia = objBD.IdColonia;
                            colonia.IdColonia = objBD.IdColonia;
                            colonia.Nombre = objBD.Nombre;
                            //usuario.Estado.Municipio.Colonia.Nombre = objBD.Nombre;
                            result.Objects.Add(colonia);
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
