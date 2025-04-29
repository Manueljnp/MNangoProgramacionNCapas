using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace BL.Producto
{
    public class Subcategoria
    {
        public static ML.Result GetByIdCategoria(int idCategoria)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var query = context.SubcategoriaGetByIdCategoria(idCategoria).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in query)
                        {
                            ML.Producto.Subcategoria subcategoria = new ML.Producto.Subcategoria();
                            subcategoria.IdSubcategoria = item.IdSubcategoria;
                            subcategoria.Nombre = item.Subcategoria;

                            result.Objects.Add(subcategoria);
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
