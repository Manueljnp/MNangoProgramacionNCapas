using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Producto
{
    public class Producto
    {
        public static ML.Result GetAll(int idSubcategoria)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var query = context.ProductoGetAll(idSubcategoria).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in query)
                        {
                            ML.Producto.Producto producto = new ML.Producto.Producto();
                            producto.Subcategoria = new ML.Producto.Subcategoria();

                            producto.IdProducto = item.IdProducto;
                            producto.Nombre = item.Producto;
                            producto.Descripcion = item.Descripcion;
                            producto.Precio = item.Precio;
                            //producto.Imagen = item.Imagen;
                            producto.Subcategoria.IdSubcategoria = item.IdSubcategoria.Value;

                            result.Objects.Add(producto);
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

        public static ML.Result GetById(int idCategoria)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var query = context.ProductoGetById(idCategoria).FirstOrDefault();

                    if (query != null)
                    {
                        ML.Producto.Producto producto = new ML.Producto.Producto();
                        producto.Subcategoria = new ML.Producto.Subcategoria();


                        producto.IdProducto = query.IdProducto;
                        producto.Nombre = query.Producto;
                        producto.Descripcion = query.Descripcion;
                        producto.Precio = query.Precio;
                        producto.Imagen = query.Imagen;
                        producto.Subcategoria.IdSubcategoria = query.IdSubcategoria.Value;


                        result.Correct = true;
                        result.Object = producto;
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

        public static ML.Result Add(ML.Producto.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    int filasAfectadas = context.ProductoAdd
                        (producto.Nombre, producto.Descripcion, producto.Precio, producto.Imagen, 
                        producto.Subcategoria.IdSubcategoria);

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

        public static ML.Result Update(ML.Producto.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    int filasAfectadas = context.ProductoUpdate
                        (producto.IdProducto, producto.Nombre, producto.Descripcion, producto.Precio, producto.Imagen, producto.Subcategoria.IdSubcategoria);

                    if(filasAfectadas > 0)
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
        public static ML.Result Delete(int idProducto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    int filasAfectadas = context.ProductoDelete(idProducto);

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
    }
}
