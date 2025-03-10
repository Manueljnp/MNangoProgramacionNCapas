using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Municipio
    {
        public static ML.Result GetByIdEstado(int idEstado)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {   //Aunque sea GetById, en realidad traeremos N Municipios, por eso .ToList()
                    var query = context.MunicipioGetByIdEstado(idEstado).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var objBD in query)
                        {
                            ML.Municipio municipio = new ML.Municipio();
                            municipio.IdMunicipio = objBD.IdMunicipio;
                            municipio.Nombre = objBD.Nombre;

                            result.Objects.Add(municipio);

                            /*ML.Usuario usuario = new ML.Usuario();
                            usuario.Estado.Municipio.IdMunicipio = objBD.IdMunicipio;
                            usuario.Estado.Municipio.Nombre = objBD.Nombre;*/
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
