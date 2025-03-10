using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient; //ESTO DEBE IR?????????

namespace PL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //int opcion;

            Usuario.CargaMasiva();
            Console.ReadKey();

            /*do
            {
                Console.WriteLine("Selecciona una opción\n");
                Console.WriteLine("1. Agregar\n2. Eliminar\n3. Actualizar\n4. Mostrar TODOS los Registros\n5. Mostrar por ID\n6. Cerrar\n");
                opcion = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("\n");

                switch (opcion)
                {
                    case 1:
                        Usuario.Add();
                        break;
                    case 2:
                        Usuario.Delete();
                        break;
                    case 3:
                        Usuario.Update();
                        break;
                    case 4:
                        Usuario.GetAll();
                        Console.ReadKey();
                        break;
                    case 5:
                        Usuario.GetById();
                        Console.ReadKey();
                        break;
                    case 6:
                        Console.WriteLine("Presiona ENTER para cerrar");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Opcion no valida");
                        break;
                }
            }
            while( opcion != 6 );
            //Usuario.Add();
            //Usuario.Delete();
            //Usuario.Update();*/
        }
    }
}
