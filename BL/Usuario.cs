using DL_EF;
using ML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        //MÉTODOS ORIGINALES (SQL CLIENT)
        /*public static void Add(ML.Usuario usuario) //3. Método para validar antes de enviar los datos a DL
        {
            //Abrir la cadena de conexión con la BD (SqlClient)
            try //Intentar hacerlo, sino va al CATCH
            {
                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {
                    string query = "INSERT INTO Usuario(UserName, Nombre, ApellidoPaterno, ApellidoMaterno, " +
                        "Email, Password, FechaNacimiento, Sexo, Telefono, Celular, CURP, IdRol)" +
                        "VALUES (@UserName, @Nombre, @ApellidoPaterno, @ApellidoMaterno, " +
                        "@Email, @Password, @FechaNacimiento, @Sexo, @Telefono, @Celular, @CURP, @IdRol)";

                    //Pasar el QUERY (Instrucción) y el CONTEXT (conexión)
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = context;

                    //Parametros
                    cmd.Parameters.AddWithValue("@UserName", usuario.UserName);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@ApellidoPaterno", usuario.ApellidoPaterno);
                    cmd.Parameters.AddWithValue("@ApellidoMaterno", usuario.ApellidoMaterno);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Password", usuario.Password);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@Celular", usuario.Celular);
                    //cmd.Parameters.AddWithValue("@Estatus", usuario.Estatus);
                    cmd.Parameters.AddWithValue("@CURP", usuario.CURP);
                    //cmd.Parameters.AddWithValue("@Imagen", usuario.Imagen);
                    cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);

                    context.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if(filasAfectadas > 0)
                    {
                        Console.WriteLine("El registro se hizo correctamente");
                    }
                    else
                    {
                        Console.WriteLine("Error al insertar");
                    }
                }
            } catch(Exception)
            {
                Console.WriteLine("Error de Conexión");
            
            }
        }

        public static void Delete(int Id)
        {
            //Abrir la cadena de conexión con la BD (SqlClient)
            try //Intentar hacerlo, sino va al CATCH
            {
                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {
                    string query = "DELETE FROM Usuario WHERE Id = @Id";

                    //Pasar el QUERY (Instrucción) y el CONTEXT (conexión)
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = context;

                    //Parametros
                    cmd.Parameters.AddWithValue("@Id", Id);

                    context.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        Console.WriteLine("La eliminación se hizo correctamente");
                    }
                    else
                    {
                        Console.WriteLine("Error al eliminar");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error de Conexión");

            }
        }

        public static void Update(ML.Usuario usuario)
        {
            try
            {
                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {
                    string query = "UPDATE Usuario SET UserName=@UserName, Nombre=@Nombre, ApellidoPaterno=@ApellidoPaterno, " +
                        "ApellidoMaterno=@ApellidoMaterno, Email=@Email, Password=@Password, FechaNacimiento=@FechaNacimiento, " +
                        "Sexo=@Sexo, Telefono=@Telefono, Celular=@Celular, Estatus=@Estatus, CURP=@CURP, IdRol=@IdRol " +
                        "WHERE Id=@Id";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = context;

                    //Parametros
                    cmd.Parameters.AddWithValue("@Id", usuario.Id);
                    cmd.Parameters.AddWithValue("@UserName", usuario.UserName);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@ApellidoPaterno", usuario.ApellidoPaterno);
                    cmd.Parameters.AddWithValue("@ApellidoMaterno", usuario.ApellidoMaterno);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Password", usuario.Password);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@Celular", usuario.Celular);
                    cmd.Parameters.AddWithValue("@Estatus", usuario.Estatus);
                    cmd.Parameters.AddWithValue("@CURP", usuario.CURP);
                    //cmd.Parameters.AddWithValue("@Imagen", usuario.Imagen);
                    cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);

                    
                    context.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        Console.WriteLine("Actualización correcta");
                    }
                    else
                    {
                        Console.WriteLine("Error de actualización");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error de conexión");
            }
        }

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {
                    string query = "SELECT * FROM Usuario";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = context;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        //Si trae registros
                        result.Objects = new List<object>();

                        foreach (DataRow row in dataTable.Rows)
                        {
                            ML.Usuario usuario = new ML.Usuario();

                            usuario.Id = Convert.ToInt32(row[0].ToString());
                            usuario.UserName = row[5].ToString();
                            usuario.Nombre = row[1].ToString();
                            usuario.ApellidoPaterno = row[2].ToString();
                            usuario.ApellidoMaterno = row[3].ToString();
                            usuario.Email = row[6].ToString();
                            usuario.Password = row[7].ToString();
                            usuario.FechaNacimiento = Convert.ToDateTime(row[8].ToString());
                            usuario.Sexo = row[9].ToString();
                            usuario.Telefono = row[4].ToString();
                            //usuario.Celular = row[10].ToString();
                            usuario.Estatus = Convert.ToBoolean(row[11].ToString());
                            //usuario.CURP = row[12].ToString();
                            //usuario.Imagen = Convert.ToSByte(row[13].ToString());

                            //Hacer condición para IdRol porque es un INT y si es NULL traería STRING
                            if (row[14].ToString() == "") //El valor viene como "", en la condición poner "" en vez de NULL
                            {
                                usuario.IdRol = 0;
                            }
                            else
                            {
                                usuario.IdRol = Convert.ToInt16(row[14].ToString());
                            }

                            result.Objects.Add(usuario);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        //Si no hay registros:
                        result.Correct = false;
                        result.ErrorMessage = "No hay datos o registros";
                    }
                }
            }
            catch (Exception ex)
            {
                //Hubo un error
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result GetById(int Id)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {
                    string query = "SELECT * FROM Usuario WHERE Id=@Id";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = context;

                    cmd.Parameters.AddWithValue("Id", Id);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0 )
                    {
                        DataRow row = dataTable.Rows[0];

                        ML.Usuario usuario = new ML.Usuario();

                        usuario.Id = Convert.ToInt32(row[0].ToString());
                        usuario.UserName = row[5].ToString();
                        usuario.Nombre = row[1].ToString();
                        usuario.ApellidoPaterno = row[2].ToString();
                        usuario.ApellidoMaterno = row[3].ToString();
                        usuario.Email = row[6].ToString();
                        usuario.Password = row[7].ToString();
                        usuario.FechaNacimiento = Convert.ToDateTime(row[8].ToString());
                        usuario.Sexo = row[9].ToString();
                        usuario.Telefono = row[4].ToString();
                        usuario.Celular = row[10].ToString();
                        usuario.Estatus = Convert.ToBoolean(row[11].ToString());
                        usuario.CURP = row[12].ToString();
                        //usuario.Imagen = Convert.ToSByte(row[13].ToString());

                        //Hacer condición para IdRol porque es un INT y si es NULL traería STRING
                        if (row[14].ToString() == "") //El valor viene como "", en la condición poner "" en vez de NULL
                        {
                            usuario.IdRol = 0;
                        }
                        else
                        {
                            usuario.IdRol = Convert.ToInt16(row[14].ToString());
                        }
                       
                        result.Object = usuario; //UNBOXING

                        result.Correct = true;
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
        }*/

        //MÉTODOS con STORED PROCEDURES
        /*public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            //Abrir la cadena de conexión con la BD (SqlClient)
            try //Intentar hacerlo, sino va al CATCH
            {
                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {
                    string query = "UsuarioAdd";

                    //Pasar el QUERY (Instrucción) y el CONTEXT (conexión)
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = context;
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Parametros
                    cmd.Parameters.AddWithValue("@UserName", usuario.UserName);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@ApellidoPaterno", usuario.ApellidoPaterno);
                    cmd.Parameters.AddWithValue("@ApellidoMaterno", usuario.ApellidoMaterno);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Password", usuario.Password);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@Celular", usuario.Celular);
                    //cmd.Parameters.AddWithValue("@Estatus", usuario.Estatus);
                    cmd.Parameters.AddWithValue("@CURP", usuario.CURP);
                    //cmd.Parameters.AddWithValue("@Imagen", usuario.Imagen);
                    cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);

                    context.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        result.Correct = true;
                        Console.WriteLine("El registro se hizo correctamente\n");
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo insertar";
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

        public static ML.Result Delete(int Id)
        {
            ML.Result result = new ML.Result();
            //Abrir la cadena de conexión con la BD (SqlClient)
            try //Intentar hacerlo, sino va al CATCH
            {
                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {
                    string query = "UsuarioDelete";

                    //Pasar el QUERY (Instrucción) y el CONTEXT (conexión)
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = context;
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Parametros
                    cmd.Parameters.AddWithValue("@Id", Id);

                    context.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        Console.WriteLine("La eliminación se hizo correctamente");
                    }
                    else
                    {
                        Console.WriteLine("Error al eliminar");
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

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {

                    string query = "UsuarioUpdate";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = context;
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Parametros
                    cmd.Parameters.AddWithValue("@Id", usuario.Id);
                    cmd.Parameters.AddWithValue("@UserName", usuario.UserName);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@ApellidoPaterno", usuario.ApellidoPaterno);
                    cmd.Parameters.AddWithValue("@ApellidoMaterno", usuario.ApellidoMaterno);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Password", usuario.Password);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@Celular", usuario.Celular);
                    cmd.Parameters.AddWithValue("@Estatus", usuario.Estatus);
                    cmd.Parameters.AddWithValue("@CURP", usuario.CURP);
                    //cmd.Parameters.AddWithValue("@Imagen", usuario.Imagen);
                    cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);


                    context.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        result.Correct = true;
                        Console.WriteLine("Actualización correcta");
                    }
                    else
                    {
                        Console.WriteLine("Error de actualización");
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

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {
                    string query = "UsuarioGetAll";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = context;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        //Si trae registros
                        result.Objects = new List<object>();

                        foreach (DataRow row in dataTable.Rows)
                        {
                            ML.Usuario usuario = new ML.Usuario();

                            usuario.Id = Convert.ToInt32(row[0].ToString());
                            usuario.UserName = row[5].ToString();
                            usuario.Nombre = row[1].ToString();
                            usuario.ApellidoPaterno = row[2].ToString();
                            usuario.ApellidoMaterno = row[3].ToString();
                            usuario.Email = row[6].ToString();
                            usuario.Password = row[7].ToString();
                            usuario.FechaNacimiento = Convert.ToDateTime(row[8].ToString());
                            usuario.Sexo = row[9].ToString();
                            usuario.Telefono = row[4].ToString();
                            usuario.Celular = row[10].ToString();
                            usuario.Estatus = Convert.ToBoolean(row[11].ToString());
                            usuario.CURP = row[12].ToString();
                            //usuario.Imagen = row[13].ToString();

                            //Hacer condición para IdRol porque es un INT y si es NULL traería STRING
                            if (row[13].ToString() == "") //El valor viene como "", en la condición poner "" en vez de NULL
                            {
                                usuario.IdRol = 0;
                            }
                            else
                            {
                                usuario.IdRol = Convert.ToInt16(row[13].ToString());
                            }

                            result.Objects.Add(usuario);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        //Si no hay registros:
                        result.Correct = false;
                        result.ErrorMessage = "No hay datos o registros";
                    }
                }
            }
            catch (Exception ex)
            {
                //Hubo un error
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result GetById(int Id)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {
                    string query = "UsuarioGetById";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = context;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("Id", Id);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow row = dataTable.Rows[0];

                        ML.Usuario usuario = new ML.Usuario();

                        usuario.Id = Convert.ToInt32(row[0].ToString());
                        usuario.UserName = row[5].ToString();
                        usuario.Nombre = row[1].ToString();
                        usuario.ApellidoPaterno = row[2].ToString();
                        usuario.ApellidoMaterno = row[3].ToString();
                        usuario.Email = row[6].ToString();
                        usuario.Password = row[7].ToString();
                        usuario.FechaNacimiento = Convert.ToDateTime(row[8].ToString());
                        usuario.Sexo = row[9].ToString();
                        usuario.Telefono = row[4].ToString();
                        usuario.Celular = row[10].ToString();
                        usuario.Estatus = Convert.ToBoolean(row[11].ToString());
                        usuario.CURP = row[12].ToString();
                        //usuario.Imagen = Convert.ToSByte(row[13].ToString());

                        //Hacer condición para IdRol porque es un INT y si es NULL traería STRING
                        if (row[13].ToString() == "") //El valor viene como "", en la condición poner "" en vez de NULL
                        {
                            usuario.IdRol = 0;
                        }
                        else
                        {
                            usuario.IdRol = Convert.ToInt16(row[13].ToString());
                        }

                        result.Object = usuario; //UNBOXING

                        result.Correct = true;
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
        }*/

        //MÉTODOS con ENTITY FRAMEWORK
        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var nuevoUsuario = context.UsuarioAdd
                       (usuario.UserName, usuario.Nombre, usuario.ApellidoPaterno,
                       usuario.ApellidoMaterno, usuario.Email, usuario.Password, DateTime.Parse(usuario.FechaNacimiento.ToString()),
                       usuario.Sexo, usuario.Telefono, usuario.Celular, usuario.CURP, usuario.Rol.IdRol, usuario.Direccion.Calle,
                       usuario.Direccion.NumeroInterior, usuario.Direccion.NumeroExterior, usuario.Direccion.Colonia.IdColonia, usuario.Imagen).FirstOrDefault(); //Capturar el Id retornado por el SP

                    if (nuevoUsuario != null && nuevoUsuario > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo agregar el usuario";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result Delete(int IdUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    int filasAfectadas = context.UsuarioDelete(IdUsuario);

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

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            //usuario.Rol = new ML.Rol();   //Sobreescribe IdRol, por eso lo quité

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    int filasAfectadas = context.UsuarioUpdate
                        (usuario.UserName, usuario.Nombre, usuario.ApellidoPaterno,
                        usuario.ApellidoMaterno, usuario.Email, usuario.Password,
                        DateTime.Parse(usuario.FechaNacimiento.ToString()),
                        usuario.Sexo, usuario.Telefono, usuario.Celular,
                        usuario.CURP, usuario.Rol.IdRol, usuario.Imagen, usuario.Direccion.Calle,
                        usuario.Direccion.NumeroInterior, usuario.Direccion.NumeroExterior,
                        usuario.Direccion.Colonia.IdColonia, usuario.IdUsuario);

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

        //Método de prueba GetAllConBusqueda
        public static ML.Result GetAll(ML.Usuario usuarioObj) //El parámetro será un Modelo de usuario
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var query = context.UsuarioGetAllConBusquedaView(usuarioObj.Nombre, usuarioObj.ApellidoPaterno, 
                        usuarioObj.ApellidoMaterno, usuarioObj.Rol.IdRol).ToList();

                    if(query.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach(var objBD in query)
                        {
                            ML.Usuario usuario = new ML.Usuario();

                            usuario.Direccion = new ML.Direccion();
                            usuario.Direccion.Colonia = new ML.Colonia();
                            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();

                            usuario.Rol = new ML.Rol();

                            usuario.IdUsuario = objBD.IdUsuario;
                            usuario.UserName = objBD.UserName;
                            usuario.Nombre = objBD.Nombre;
                            usuario.ApellidoPaterno = objBD.ApellidoPaterno;
                            usuario.ApellidoMaterno = objBD.ApellidoMaterno;
                            usuario.Email = objBD.Email;
                            usuario.Password = objBD.Password;
                            //usuario.FechaNacimiento = DateTime.Parse(objBD.FechaNacimiento.ToString());
                            usuario.FechaNacimiento = objBD.FechaNacimiento.ToString();
                            usuario.Sexo = objBD.Sexo;
                            usuario.Telefono = objBD.Telefono;
                            usuario.Celular = objBD.Celular;
                            usuario.Estatus = objBD.Estatus;
                            usuario.CURP = objBD.CURP;
                            usuario.Imagen = objBD.Imagen;

                            //Asigarle el valor a traveés de usuario entrar a Rol y luego al atributo
                            usuario.Rol.Nombre = objBD.NombreRol;//Y el objBD es como aparece en la BD, en SQL le puse NombreRol al ALIAS en mi SP

                            //PARA DIRECCIÓN:
                            usuario.Direccion.Calle = objBD.Calle;
                            usuario.Direccion.NumeroInterior = objBD.NumeroInterior;
                            usuario.Direccion.NumeroExterior = objBD.NumeroExterior;
                            usuario.Direccion.Colonia.Nombre = objBD.NombreColonia;
                            usuario.Direccion.Colonia.CodigoPostal = objBD.CodigoPostal;
                            usuario.Direccion.Colonia.Municipio.Nombre = objBD.NombreMunicipio;
                            usuario.Direccion.Colonia.Municipio.Estado.Nombre = objBD.NombreEstado;

                            result.Objects.Add(usuario);
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
                result.ErrorMessage=ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result GetById(int IdUsuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    //var query = context.UsuarioGetById(IdUsuario).SingleOrDefault();
                    var query = context.UsuarioGetById(IdUsuario).FirstOrDefault();

                    if (query != null)
                    {

                        //si trajo registro
                        ML.Usuario usuario = new ML.Usuario();

                        //Inicializar cada una, Comenzar de Direccion, para poder llegar a Estado (sino no funciona)
                        usuario.Direccion = new ML.Direccion();
                        usuario.Direccion.Colonia = new ML.Colonia();
                        usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                        usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();   //Abrir para Estado

                        //PARA WEB
                        usuario.Rol = new ML.Rol(); //Abrir la puerta
                                                    //PARA WEB
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.UserName = query.UserName;
                        usuario.Nombre = query.Nombre;
                        usuario.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.Email = query.Email;
                        usuario.Password = query.Password;
                        //usuario.FechaNacimiento = DateTime.Parse(objBD.FechaNacimiento.ToString());
                        usuario.FechaNacimiento = query.FechaNacimiento.ToString();
                        usuario.Sexo = query.Sexo;
                        usuario.Telefono = query.Telefono;
                        usuario.Celular = query.Celular;
                        usuario.Estatus = query.Estatus;
                        usuario.CURP = query.CURP;
                        usuario.Imagen = query.Imagen;

                        /*
                        //Hacer condición para IdRol porque es un INT y si es NULL traería STRING
                        if (query.IdRol == null) //El valor viene como NULL, en la condición poner NULL en vez de ""
                        {
                            //usuario.IdRol = 0;
                            usuario.Rol.IdRol = 0;
                        }
                        else
                        {
                            //usuario.IdRol = query.IdRol.Value;
                            usuario.Rol.IdRol = query.IdRol.Value;
                        }
                        */

                        if (query.IdRol != null)
                        {
                            //Trae un id
                            usuario.Rol.IdRol = query.IdRol.Value;
                        }
                        else
                        {
                            usuario.Rol.IdRol = 0;
                        }

                        //Agregando atributos de DIRECCIÓN*************************************************

                        //Atributos Direccion
                        usuario.Direccion.Calle = query.Calle;
                        usuario.Direccion.NumeroInterior = query.NumeroInterior;
                        usuario.Direccion.NumeroExterior = query.NumeroExterior;

                        //Estado
                        if (query.IdEstado != null)
                        {
                            usuario.Direccion.Colonia.Municipio.Estado.IdEstado = query.IdEstado.Value;
                        }
                        else
                        {
                            usuario.Direccion.Colonia.Municipio.Estado.IdEstado = 0;
                        }

                        //Municipio
                        if (query.IdMunicipio != null)
                        {
                            usuario.Direccion.Colonia.Municipio.IdMunicipio = query.IdMunicipio.Value;
                        }
                        else
                        {
                            usuario.Direccion.Colonia.Municipio.IdMunicipio = 0;
                        }

                        //Colonia
                        if (query.IdColonia != null)
                        {
                            usuario.Direccion.Colonia.IdColonia = query.IdColonia.Value;
                        }
                        else
                        {
                            usuario.Direccion.Colonia.IdColonia = 0;
                        }
                        //*********************************************************************************

                        result.Object = usuario;    //Boxing
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

        public static ML.Result CambioEstatus(int IdUsuario, bool Estatus)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    int filasAfectadas = context.CambioEstatus(IdUsuario, Estatus);
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

        //Métodos con LINQ
        /*public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    DL_EF.Usuario usuarioBD = new DL_EF.Usuario();

                    usuarioBD.UserName = usuario.UserName;
                    usuarioBD.Nombre = usuario.Nombre;
                    usuarioBD.ApellidoPaterno = usuario.ApellidoPaterno;
                    usuarioBD.ApellidoMaterno = usuario.ApellidoMaterno;
                    usuarioBD.Email = usuario.Email;
                    usuarioBD.Password = usuario.Password;
                    usuarioBD.FechaNacimiento = DateTime.Parse(usuario.FechaNacimiento.ToString());
                    usuarioBD.Sexo = usuario.Sexo;
                    usuarioBD.Telefono = usuario.Telefono;
                    usuarioBD.Celular = usuario.Celular;
                    usuarioBD.CURP = usuario.CURP;
                    usuarioBD.IdRol = usuario.IdRol;

                    context.Usuarios.Add(usuarioBD); //Genera el INSERT => INSERT INTO Usuario VALUES()

                    int filasAfectadas = context.SaveChanges();
                    if(filasAfectadas > 0 )
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al insertar con LINQ";
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

        public static ML.Result Delete(int IdUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using(DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    //1. Busca => SELECT
                    var query = (from usuario in context.Usuarios
                                  where usuario.IdUsuario == IdUsuario
                                 select usuario).SingleOrDefault();
                    //SELECT * FROM Materia (aquí si se usa así, pero SOLO para DELETE)

                    if(query != null )
                    {
                        //2. Remove = DELETE
                        context.Usuarios.Remove(query); //Genera el query

                        int filasAfectadas = context.SaveChanges(); //Ejecutar el query

                        if(filasAfectadas > 0)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct= false;
                            result.ErrorMessage = "Error eliminar con LINQ";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct= false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    //SELECT * FROM Usuario WHERE 
                    var query = (from usuarioBD in context.Usuarios
                                 where usuarioBD.IdUsuario == usuario.IdUsuario
                                 select usuarioBD).SingleOrDefault();

                    if (query != null)
                    {   //Encontró algo

                        //SET Nombre = @Nombre
                        query.UserName = usuario.UserName;
                        query.Nombre = usuario.Nombre;
                        query.ApellidoPaterno = usuario.ApellidoPaterno;
                        query.ApellidoMaterno = usuario.ApellidoMaterno;
                        query.Email = usuario.Email;
                        query.Password = usuario.Password;
                        query.FechaNacimiento = Convert.ToDateTime(usuario.FechaNacimiento);
                        query.Sexo = usuario.Sexo;
                        query.Telefono = usuario.Telefono;
                        query.Celular = usuario.Celular;
                        query.Estatus = usuario.Estatus;
                        query.CURP = usuario.CURP;
                        query.IdRol = usuario.IdRol;

                        int filasAfectadas = context.SaveChanges();
                        if(filasAfectadas > 0 )
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "Error, no se actualizó con LINQ";
                        }
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

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var query = (from usuarioBD in context.Usuarios
                                 select usuarioBD).ToList();

                    if (query.Count > 0)
                    {
                        //si trajo registros
                        result.Objects = new List<object>();

                        foreach (var objBD in query) // Iterar sobre la lista
                        {
                            ML.Usuario usuario = new ML.Usuario();

                            usuario.IdUsuario = objBD.IdUsuario;
                            usuario.UserName = objBD.UserName;
                            usuario.Nombre = objBD.Nombre;
                            usuario.ApellidoPaterno = objBD.ApellidoPaterno;
                            usuario.ApellidoMaterno = objBD.ApellidoMaterno;
                            usuario.Email = objBD.Email;
                            usuario.Password = objBD.Password;
                            //usuario.FechaNacimiento = DateTime.Parse(objBD.FechaNacimiento.ToString());
                            usuario.FechaNacimiento = objBD.FechaNacimiento.ToString();
                            usuario.Sexo = objBD.Sexo;
                            usuario.Telefono = objBD.Telefono;
                            usuario.Celular = objBD.Celular;
                            usuario.Estatus = objBD.Estatus;
                            usuario.CURP = objBD.CURP;
                            //usuario.Imagen = objBD.Imagen;

                            //Hacer condición para IdRol porque es un INT y si es NULL traería STRING
                            if (objBD.IdRol == null) //El valor viene como NULL, en la condición poner NULL en vez de ""
                            {
                                usuario.IdRol = 0;
                            }
                            else
                            {
                                usuario.IdRol = objBD.IdRol.Value;
                            }
                            result.Objects.Add(usuario);
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

        public static ML.Result GetById(int Id)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    //var query = context.UsuarioGetById(Id).SingleOrDefault();
                    var query = (from usuarioBD in context.Usuarios
                                 where usuarioBD.IdUsuario == Id
                         select usuarioBD).SingleOrDefault();

                    result.Object = new List<object>();

                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();

                        usuario.IdUsuario = query.IdUsuario;
                        usuario.UserName = query.UserName;
                        usuario.Nombre = query.Nombre;
                        usuario.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.Email = query.Email;
                        usuario.Password = query.Password;
                        usuario.FechaNacimiento = query.FechaNacimiento.ToString();
                        usuario.Sexo = query.Sexo;
                        usuario.Telefono = query.Telefono;
                        usuario.Celular = query.Celular;
                        usuario.Estatus = query.Estatus;
                        usuario.CURP = query.CURP;
                        //usuario.Imagen = objBD.Imagen;
                        //Hacer condición para IdRol porque es un INT y si es NULL traería STRING
                        if (query.IdRol == null) 
                        {
                            usuario.IdRol = 0;
                        }
                        else
                        {
                            usuario.IdRol = query.IdRol.Value;
                        }
                        result.Object = usuario;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ese ID no está registrado";
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
        }*/

        //Métodos Entity Framework ORIGINALES (antes de Dirección + Busqueda Abierta + Vista + Dinamico)
        /*public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            //Instanciar el nuevo modelo Rol
            //usuario.Rol = new ML.Rol();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    //Modificar el SP porque estamos agregando un IdRol que ahora es FK
                    int filasAfectadas = context.UsuarioAdd
                       (usuario.UserName, usuario.Nombre, usuario.ApellidoPaterno,
                       usuario.ApellidoMaterno, usuario.Email, usuario.Password, DateTime.Parse(usuario.FechaNacimiento.ToString()),
                       usuario.Sexo, usuario.Telefono, usuario.Celular, usuario.CURP, /*usuario.IdRol usuario.Rol.IdRol); //de 'usuario' entramos a Rol y luego accedemos a IdRol

                    if (filasAfectadas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo agregar el usuario";
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
        }*/

        /*public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.MNangoProgramacionNCapasBDEntities context = new DL_EF.MNangoProgramacionNCapasBDEntities())
                {
                    var query = context.UsuarioGetAll().ToList(); //Obtener TODOS los registros como una LISTA

                    if (query.Count > 0)
                    {
                        //si trajo registros
                        result.Objects = new List<object>();

                        foreach (var objBD in query) // Iterar sobre la lista
                        {

                            //si trajo registro
                            ML.Usuario usuario = new ML.Usuario();

                            //Inicializar cada una, Comenzar de Direccion, para poder llegar a Estado (sino no funciona)
                            usuario.Direccion = new ML.Direccion();
                            usuario.Direccion.Colonia = new ML.Colonia();
                            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();   //Abrir para Estado

                            //PARA WEB
                            usuario.Rol = new ML.Rol(); //Abrir la puerta
                                                        //PARA WEB

                            usuario.IdUsuario = objBD.IdUsuario;
                            usuario.UserName = objBD.UserName;
                            usuario.Nombre = objBD.Nombre;
                            usuario.ApellidoPaterno = objBD.ApellidoPaterno;
                            usuario.ApellidoMaterno = objBD.ApellidoMaterno;
                            usuario.Email = objBD.Email;
                            usuario.Password = objBD.Password;
                            //usuario.FechaNacimiento = DateTime.Parse(objBD.FechaNacimiento.ToString());
                            usuario.FechaNacimiento = objBD.FechaNacimiento.ToString();
                            usuario.Sexo = objBD.Sexo;
                            usuario.Telefono = objBD.Telefono;
                            usuario.Celular = objBD.Celular;
                            usuario.Estatus = objBD.Estatus;
                            usuario.CURP = objBD.CURP;
                            usuario.Imagen = objBD.Imagen;


                            //Hacer condición para IdRol porque es un INT y si es NULL traería STRING
                            /*if (objBD.IdRol == null) //El valor viene como NULL, en la condición poner NULL en vez de ""
                            {
                                usuario.IdRol = 0;
                            }
                            else
                            {
                                usuario.IdRol = objBD.IdRol.Value;
                            }
                            result.Objects.Add(usuario);

                            //Asigarle el valor a traveés de usuario entrar a Rol y luego al atributo
                            usuario.Rol.Nombre = objBD.NombreRol;//Y el objBD es como aparece en la BD, en SQL le puse NombreRol al ALIAS en mi SP

                            //PARA DIRECCIÓN:
                            usuario.Direccion.Calle = objBD.Calle;
                            usuario.Direccion.NumeroInterior = objBD.NumeroInterior;
                            usuario.Direccion.NumeroExterior = objBD.NumeroExterior;
                            usuario.Direccion.Colonia.Nombre = objBD.NombreColonia;
                            usuario.Direccion.Colonia.CodigoPostal = objBD.CodigoPostal;
                            usuario.Direccion.Colonia.Municipio.Nombre = objBD.NombreMunicipio;
                            usuario.Direccion.Colonia.Municipio.Estado.Nombre = objBD.NombreEstado;

                            result.Objects.Add(usuario);
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
        }*/
    }
}
