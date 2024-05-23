using Dapper;
using InscripcionesApp.DataAccess.DataFuncionario;
using InscripcionesApp.Helpers;
using InscripcionesApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace InscripcionesApp.DataAccess
{
    public class RepositoryWeb
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public RepositoryWeb(ApplicationDbContext context, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _context = context;
        }

        private int GetMaxIdUsuario()
        {
            // Consulta SQL para obtener el máximo ID de usuario
            string query = "SELECT ISNULL(MAX(ID_USUARIO), 0) FROM Usuarios";

            // Establecer la conexión con la base de datos y ejecutar la consulta utilizando Dapper
            using (var connection = new SqlConnection(_connectionString))
            {
                // Utilizamos Dapper para ejecutar la consulta SQL
                int maxId = connection.QuerySingle<int>(query);

                // Retornamos el máximo ID de usuario obtenido
                return maxId;
            }
        }


        private bool ExisteEmail(string email)
        {
            // Consulta SQL parametrizada
            string query = "SELECT COUNT(*) FROM Usuarios WHERE Email = @Email";

            // Ejecutar la consulta y obtener el resultado utilizando Dapper
            using (var connection = new SqlConnection(_connectionString))
            {
                // Utilizamos Dapper para ejecutar la consulta SQL parametrizada
                int count = connection.QuerySingleOrDefault<int>(query, new { Email = email });

                // Verificar si se encontró algún usuario con el correo electrónico especificado
                return count > 0;
            }
        }
        public bool RegistrarUsuario(string email, string password, string nombre, string apellidos, string tipo)
        {
            // Verificar si el correo electrónico ya existe
            bool existeEmail = ExisteEmail(email);
            if (existeEmail)
            {
                return false; // Retorna falso si el correo electrónico ya existe
            }

            // Generar el salt y encriptar la contraseña
            string salt = HelperCryptography.GenerateSalt();
            byte[] hashedPasswordBytes = HelperCryptography.EncriptarPassword(password, salt);
            string hashedPassword = Convert.ToBase64String(hashedPasswordBytes); // Convertir el hash a una representación de cadena adecuada

            // Crear la conexión y ejecutar la consulta para insertar el nuevo usuario
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta SQL parametrizada para insertar un nuevo usuario
                string query = @"INSERT INTO USUARIOS (EMAIL, PASS, SALT, NOMBRE, APELLIDO, TIPO)
                        VALUES (@Email, CONVERT(varbinary(max), @Password), CONVERT(varbinary(max), @Salt), @Nombre, @Apellidos, @Tipo)";

                // Parámetros para la consulta SQL
                var parameters = new
                {
                    Email = email,
                    Password = hashedPassword,
                    Nombre = nombre,
                    Apellidos = apellidos,
                    Tipo = tipo,
                    Salt = salt
                };

                // Ejecutar la consulta SQL utilizando Dapper
                connection.Execute(query, parameters);
            }

            return true; // Retorna verdadero si el usuario se registró con éxito
        }



        public UsuariosME LogInUsuario(string email, string password)
        {
            UsuariosME usuario = null;

            string connectionString = _connectionString;
            string query = "SELECT * FROM USUARIOS WHERE EMAIL = @Email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        usuario = new UsuariosME
                        {
                            IdUsuario = Convert.ToInt32(reader["ID_USUARIO"]),
                            Email = Convert.ToString(reader["EMAIL"]),
                            Password = reader["PASS"] as byte[],
                            Salt = Convert.ToString(reader["SALT"]),
                            Nombre = Convert.ToString(reader["NOMBRE"]),
                            Apellidos = Convert.ToString(reader["APELLIDO"]),
                            Tipo = Convert.ToString(reader["TIPO"])
                        };
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción aquí
                    Console.WriteLine("Error al intentar iniciar sesión: " + ex.Message);
                }
            }

            if (usuario != null)
            {
                // Verificar la contraseña
                byte[] passUsuario = usuario.Password;
                byte[] temporal = HelperCryptography.EncriptarPassword(password, usuario.Salt);

                bool respuesta = HelperCryptography.compareArrays(passUsuario, temporal);
                if (!respuesta)
                {
                    usuario = null; // Si la contraseña no coincide, establece usuario a null
                }
            }

            return usuario;
        }

        public List<UsuariosME> GetUsuarios()
        {
            // Consulta SQL para seleccionar todos los usuarios
            string query = "SELECT * FROM Usuarios";

            // Establecer la conexión con la base de datos
            using (var connection = new SqlConnection(_connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Ejecutar la consulta SQL utilizando Dapper
                var usuarios = connection.Query<UsuariosME>(query).ToList();

                return usuarios; // Retorna la lista de usuarios
            }
        }
    }
}
