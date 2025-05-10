using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Producto
{
    public class ProductoSucursal
    {
        public static ML.Result GetProductoBySucursal(int idSucursal)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var query = (from ProductoSucursal in context.ProductoSucursals
                                 join Producto in context.Productoes on ProductoSucursal.IdProducto equals Producto.IdProducto
                                 join Sucursal in context.Sucursals on ProductoSucursal.IdSucursal equals Sucursal.IdSucursal
                                 where ProductoSucursal.IdSucursal == idSucursal
                                 select new
                                 {
                                     IdProductoSucursal = ProductoSucursal.IdProductoSucursal,
                                     IdProducto = Producto.IdProducto,
                                     Producto1 = Producto.Producto1,
                                     Imagen = Producto.Imagen,
                                     IdSucursal = Sucursal.IdSucursal,
                                     Sucursal = Sucursal.Nombre,
                                     Stock = ProductoSucursal.Stock
                                 }
                                ).ToList();

                    result.Objects = new List<object>();

                    if (query.Count > 0)
                    {
                        foreach (var item in query)
                        {
                            //Crear instancias de Producto y después de ProductoSucursal
                            ML.Producto.ProductoSucursal productoSucursal = new ML.Producto.ProductoSucursal();

                            productoSucursal.IdProductoSucursal = item.IdProductoSucursal;
                            productoSucursal.Stock = item.Stock.Value;

                            //Asignar valores a Productos
                            productoSucursal.IdProducto = item.IdProducto;
                            productoSucursal.Nombre = item.Producto1;
                            productoSucursal.Imagen = item.Imagen;

                            //Asignar valores a Sucursal
                            productoSucursal.IdSucursal = item.IdSucursal;
                            productoSucursal.Sucursal = item.Sucursal;

                            result.Objects.Add(productoSucursal);
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

        public static ML.Result ActualizarStock(int idProductoSucursal, int stock)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var query = (from ps in context.ProductoSucursals
                                 where ps.IdProductoSucursal == idProductoSucursal
                                 select ps).SingleOrDefault();

                    if (query != null)
                    {
                        query.Stock = stock;
                        context.SaveChanges();
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "ProductoSucursal no encontrado";
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
