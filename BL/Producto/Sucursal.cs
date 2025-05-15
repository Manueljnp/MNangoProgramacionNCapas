using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Producto
{
    public class Sucursal
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var query = (from Sucursal in context.Sucursals
                                 select new
                                 {
                                     IdSucursal = Sucursal.IdSucursal,
                                     Nombre = Sucursal.Nombre,
                                     Latitud = Sucursal.Latitud,
                                     Longitud = Sucursal.Longitud
                                 }).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in query)
                        {
                            ML.Producto.Sucursal sucursal = new ML.Producto.Sucursal();

                            sucursal.IdSucursal = item.IdSucursal;
                            sucursal.Nombre = item.Nombre;
                            sucursal.Latitud = item.Latitud;
                            sucursal.Longitud = item.Longitud;

                            result.Objects.Add(sucursal);
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

        public static ML.Result Add(ML.Producto.Sucursal sucursal)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    //Crear nuevo objeto de tipo entidad
                    DL_EF.Sucursal sucursalEntity = new DL_EF.Sucursal();

                    //Asignar las propiedades desde el modelo ML al modelo de la base de datos
                    sucursalEntity.Nombre = sucursal.Nombre;
                    sucursalEntity.Latitud = sucursal.Latitud;
                    sucursalEntity.Longitud = sucursal.Longitud;

                    //Agregar al contexto y guardar cambios
                    context.Sucursals.Add(sucursalEntity);

                    int filasAfectadas = context.SaveChanges();

                    if (filasAfectadas > 0)
                    {
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

        public static ML.Result GetById(int idSucursal)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var query = (from Sucursal in context.Sucursals
                                 where Sucursal.IdSucursal == idSucursal
                                 select Sucursal).SingleOrDefault();

                    if (query != null)
                    {
                        ML.Producto.Sucursal sucursal = new ML.Producto.Sucursal();

                        sucursal.IdSucursal = query.IdSucursal;
                        sucursal.Nombre = query.Nombre;
                        sucursal.Latitud = query.Latitud;
                        sucursal.Longitud = query.Longitud;

                        result.Object = sucursal;

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

        public static ML.Result Update(ML.Producto.Sucursal sucursal)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var sucursalExistente = context.Sucursals.FirstOrDefault(s => s.IdSucursal == sucursal.IdSucursal);

                    if (sucursalExistente != null)
                    {
                        sucursalExistente.Nombre = sucursal.Nombre;
                        sucursalExistente.Latitud = sucursal.Latitud;
                        sucursalExistente.Longitud = sucursal.Longitud;

                        context.SaveChanges();
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

        public static ML.Result Delete(int idSucursal)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var sucursal = context.Sucursals.FirstOrDefault(s => s.IdSucursal == idSucursal);

                    if (sucursal != null)
                    {
                        context.Sucursals.Remove(sucursal);
                        context.SaveChanges();
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Sucursal no encontrada.";
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
