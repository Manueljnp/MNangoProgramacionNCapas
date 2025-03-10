using ML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO; //Para poder usar 'FILE'

namespace PL
{
    public class Usuario
    {
        public static void Add()
        {
            ML.Usuario usuario = new ML.Usuario();

            Console.WriteLine("Escribe el Nombre de Usuario: ");
            usuario.UserName = Console.ReadLine();
            Console.WriteLine("Escribe el nombre: ");
            usuario.Nombre = Console.ReadLine();
            Console.WriteLine("Escribe el apellido paterno: ");
            usuario.ApellidoPaterno = Console.ReadLine();
            Console.WriteLine("Escribe el apellido materno: ");
            usuario.ApellidoMaterno = Console.ReadLine();
            Console.WriteLine("Escribe el email: ");
            usuario.Email = Console.ReadLine();
            Console.WriteLine("Escribe el password: ");
            usuario.Password = Console.ReadLine();
            Console.WriteLine("Escribe la fecha de nacimiento: ");
            usuario.FechaNacimiento = Console.ReadLine();
            Console.WriteLine("Escribe el sexo: ");
            usuario.Sexo = Console.ReadLine();
            Console.WriteLine("Escribe el telefono: ");
            usuario.Telefono = Console.ReadLine();
            Console.WriteLine("Escribe el celular: ");
            usuario.Celular = Console.ReadLine();
            
            //Console.WriteLine("Escribe el Estatus: ");
            //usuario.Estatus = Convert.ToBoolean(Console.ReadLine());

            Console.WriteLine("Escribe la CURP: ");
            usuario.CURP = Console.ReadLine();

            //Console.WriteLine("Inserte Imagen: ");                  //Para que funcione:
            //usuario.Imagen = File.ReadAllBytes(Console.ReadLine()); //using System.IO;

            Console.WriteLine("Escribe el IdRol: ");
            usuario.Rol.IdRol = Convert.ToInt32(Console.ReadLine());

            //BL.Usuario.AddEF(usuario); //Método AddEF (Mejor lo nombré bien)
            BL.Usuario.Add(usuario);
        }

        public static void Delete()
        {

            Console.WriteLine("Escribe el Id a eliminar: ");
            int idUsuario = Convert.ToInt32(Console.ReadLine());

            BL.Usuario.Delete(idUsuario);
        }

        public static void Update()
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();

            Console.WriteLine("Escribe el ID a actualizar: ");
            usuario.IdUsuario = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Escribe el Nuevo Nombre de Usuario: ");
            usuario.UserName = Console.ReadLine();
            Console.WriteLine("Escribe el Nuevo nombre: ");
            usuario.Nombre = Console.ReadLine();
            Console.WriteLine("Escribe el Nuevo apellido paterno: ");
            usuario.ApellidoPaterno = Console.ReadLine();
            Console.WriteLine("Escribe el Nuevo apellido materno: ");
            usuario.ApellidoMaterno = Console.ReadLine();
            Console.WriteLine("Escribe el Nuevo email: ");
            usuario.Email = Console.ReadLine();
            Console.WriteLine("Escribe el Nuevo password: ");
            usuario.Password = Console.ReadLine();
            Console.WriteLine("Escribe la Nueva fecha de nacimiento: ");
            usuario.FechaNacimiento = Console.ReadLine();
            Console.WriteLine("Escribe el Nuevo sexo: ");
            usuario.Sexo = Console.ReadLine();
            Console.WriteLine("Escribe el Nuevo telefono: ");
            usuario.Telefono = Console.ReadLine();
            Console.WriteLine("Escribe el Nuevo celular: ");
            usuario.Celular = Console.ReadLine();
            Console.WriteLine("Escribe el Nuevo Estatus: ");
            usuario.Estatus = Convert.ToBoolean(Console.ReadLine());
            Console.WriteLine("Escribe la CURP: ");
            usuario.CURP = Console.ReadLine();

            //Console.WriteLine("Actualice Imagen: ");                  //Para que funcione:
            //usuario.Imagen = File.ReadAllBytes(Console.ReadLine()); //using System.IO;

            Console.WriteLine("Escribe el Nuevo IdRol: ");
            usuario.Rol.IdRol = Convert.ToInt32(Console.ReadLine());

            BL.Usuario.Update(usuario);
        }

        /*public static void GetAll()
        {
            ML.Result result = BL.Usuario.GetAll();

            if (result.Correct)
            {
                //Mostrar los datos con un ciclo
                foreach (ML.Usuario usuario in result.Objects)
                {
                    Console.WriteLine("ID: " + usuario.IdUsuario);
                    Console.WriteLine("UserName: " + usuario.UserName);
                    Console.WriteLine("Nombre: " + usuario.Nombre);
                    Console.WriteLine("Apeliido Paterno: " + usuario.ApellidoPaterno);
                    Console.WriteLine("Apellido Materno: " + usuario.ApellidoMaterno);
                    Console.WriteLine("Email: " + usuario.Email);
                    Console.WriteLine("Password: " + usuario.Password);
                    Console.WriteLine("Fecha de Nacimiento: " + usuario.FechaNacimiento);
                    Console.WriteLine("Sexo: " + usuario.Sexo);
                    Console.WriteLine("Telefono: " + usuario.Telefono);
                    Console.WriteLine("Celular: " + usuario.Celular);
                    Console.WriteLine("Estatus: " + usuario.Estatus);
                    Console.WriteLine("CURP: " + usuario.CURP);
                    //Console.WriteLine("Imagen: " + usuario.Imagen);
                    Console.WriteLine("IdRol: " + usuario.Rol.IdRol);
                    Console.WriteLine("\n\n");
                }
            }
            else
            {
                Console.WriteLine("Error: " + result.ErrorMessage);
            }
        }*/

        public static void GetById() //Logica para perdir información
        {
            Console.WriteLine("Id para buscar: ");
            int idUsuario = Convert.ToInt32(Console.ReadLine());

            ML.Result result = BL.Usuario.GetById(idUsuario);

            if (result.Correct)
            {
                //Imprimir información de la materia
                ML.Usuario usuario = (ML.Usuario)result.Object;

                Console.WriteLine("ID: " + usuario.IdUsuario);
                Console.WriteLine("UserName: " + usuario.UserName);
                Console.WriteLine("Nombre: " + usuario.Nombre);
                Console.WriteLine("Apeliido Paterno: " + usuario.ApellidoPaterno);
                Console.WriteLine("Apellido Materno: " + usuario.ApellidoMaterno);
                Console.WriteLine("Email: " + usuario.Email);
                Console.WriteLine("Password: " + usuario.Password);
                Console.WriteLine("Fecha de Nacimiento: " + usuario.FechaNacimiento);
                Console.WriteLine("Sexo: " + usuario.Sexo);
                Console.WriteLine("Telefono: " + usuario.Telefono);
                Console.WriteLine("Celular: " + usuario.Celular);
                Console.WriteLine("Estatus: " + usuario.Estatus);
                Console.WriteLine("CURP: " + usuario.CURP);
                //Console.WriteLine("Imagen: " + usuario.Imagen);
                Console.WriteLine("IdRol: " + usuario.Rol.IdRol);
                Console.WriteLine("\n\n");
   
            }
            else
            {
                Console.WriteLine("Error: " + result.ErrorMessage);
            }
        }

        public static ML.Result CargaMasiva()
        {
            ML.Result result = new ML.Result();
            Console.WriteLine("Carga Masiva");
            string ruta = @"C:\Users\digis\Documents\Manuel de Jesus Nango Ponce\datosPrueba.txt";
            try
            {
                StreamReader streamReader = new StreamReader(ruta);
                string fila = "";
                while((fila = streamReader.ReadLine()) != null)
                {
                    Console.WriteLine(fila);
                }
            }
            catch (Exception e)
            {
                result.Correct = false;
                result.ErrorMessage = e.Message;
                result.Ex = e;
            }
            return result;
        }
    }
}
