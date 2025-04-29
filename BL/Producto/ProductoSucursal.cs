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
                                 select new
                                 {
                                     IdProductoSucursal = ProductoSucursal.IdProductoSucursal,
                                     IdProducto = ProductoSucursal.IdProducto,
                                     IdSucursal = ProductoSucursal.IdSucursal,
                                     Stock = ProductoSucursal.Stock
                                 }
                                ).ToList();
                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in query)
                        {
                            ML.Producto.ProductoSucursal productoSucursal = new ML.Producto.ProductoSucursal();

                            productoSucursal.IdProductoSucursal = item.IdProductoSucursal;
                            productoSucursal.IdProducto = item.IdProducto;
                            productoSucursal.IdSucursal = item.IdSucursal;
                            productoSucursal.Stock = item.Stock.Value;

                            result.Objects.Add(item);
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
