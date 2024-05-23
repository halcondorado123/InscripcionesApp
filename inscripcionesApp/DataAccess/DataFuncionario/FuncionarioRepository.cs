using Dapper;
using inscripcionesApp.Models;
using System.Data.SqlClient;

namespace InscripcionesApp.DataAccess.DataFuncionario
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly string _connectionString;

        public FuncionarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<EstudianteME>> ObtenerEstudiantes()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT * FROM Estudiante";
                    return await connection.QueryAsync<EstudianteME>(sql);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error al obtener los estudiantes", ex);
            }
        }

        public async Task<EstudianteME> ObtenerEstudiantePorId(int id)
        {
            EstudianteME? estudiante = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT * FROM Estudiantes WHERE Est_ID = @EstudianteId";
                    estudiante = await connection.QueryFirstOrDefaultAsync<EstudianteME>(sql, new { EstudianteId = id });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error al obtener el estudiante por ID", ex);
            }

            return estudiante;
        }

        public async Task<EstudianteME> ObtenerEstudiantesPorApellidos(string primerApellido, string segundoApellido)
        {
            EstudianteME? estudiante = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT * FROM Estudiantes WHERE Est_PrimerApellido = @PrimerApellido AND Est_SegundoApellido = @SegundoApellido";
                    estudiante = await connection.QueryFirstOrDefaultAsync<EstudianteME>(sql, new { PrimerApellido = primerApellido, SegundoApellido = segundoApellido });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error al obtener el estudiante por apellidos", ex);
            }

            return estudiante;
        }


        public async Task<EstudianteME> ObtenerEstudiantePorIdentificacion(string identificacion)
        {
            EstudianteME estudiante = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT * FROM Estudiantes WHERE Est_NumeroDocumento = @Identificacion";
                    estudiante = await connection.QueryFirstOrDefaultAsync<EstudianteME>(sql, new { Identificacion = identificacion });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error al obtener el estudiante por identificación", ex);
            }

            return estudiante;
        }


        // ACTUALIZAR MODELO
        public async Task CrearEstudiante(EstudianteME estudiante, ProgramaME programa)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insertar el programa educativo y obtener su ID
                        string programaSql = "INSERT INTO Programas (Escuela, Modalidad, Nivel_Ingreso, Sede, Periodo, Estado) VALUES (@Escuela, @Modalidad, @NivelIngreso, @Sede, @Periodo, @Estado); SELECT SCOPE_IDENTITY();";
                        int programaId = await connection.ExecuteScalarAsync<int>(programaSql, programa, transaction);

                        // Insertar el estudiante con el ID del programa educativo
                        string estudianteSql = "INSERT INTO Estudiante (RolID, Estatus_Est_ID, Prog_ID, Est_PrimerNombre, Est_SegundoNombre, Est_PrimerApellido, Est_SegundoApellido, Est_FechaNacimiento," +
                            " Est_PaisNacimiento, Est_DepartamentoNacimiento, Est_CiudadNacimiento, Est_Direccion, Est_GrupoSanguineo, Est_TipoDocumento, Est_NumeroDocumento, Est_FechaExpedicionDoc," +
                            " Est_PaisExpedicion, Est_DepartamentoExpedicion, Est_CiudadExpedicion, Est_Sexo, Est_EstadoCivil, Est_TelefonoPrincipal, Est_TelefonoSecundario, Est_CorreoElectronico," +
                            " Est_FechaInscripcion, Prog_ID)" +
                            " VALUES (@RolID, @Estatus_Est_ID, @Prog_ID, @Est_PrimerNombre, @Est_SegundoNombre, @Est_PrimerApellido, @Est_SegundoApellido, @Est_FechaNacimiento," +
                            " @Est_PaisNacimiento, @Est_DepartamentoNacimiento, @Est_CiudadNacimiento, @Est_Direccion, @Est_GrupoSanguineo, @Est_TipoDocumento, @Est_NumeroDocumento," +
                            " @Est_FechaExpedicionDoc, @Est_PaisExpedicion, @Est_DepartamentoExpedicion, @Est_CiudadExpedicion, @Est_Sexo, @Est_EstadoCivil, @Est_TelefonoPrincipal," +
                            " @Est_TelefonoSecundario, @Est_CorreoElectronico, @Est_FechaInscripcion, @Prog_ID)";
                        await connection.ExecuteAsync(estudianteSql, new
                        {
                            RolID = 2,
                            Estatus_Est_ID = estudiante.Estatus_Est_ID,
                            estudiante.Est_PrimerNombre,
                            estudiante.Est_SegundoNombre,
                            estudiante.Est_PrimerApellido,
                            estudiante.Est_SegundoApellido,
                            estudiante.Est_FechaNacimiento,
                            estudiante.Est_PaisNacimiento,
                            estudiante.Est_DepartamentoNacimiento,
                            estudiante.Est_CiudadNacimiento,
                            estudiante.Est_Direccion,
                            estudiante.Est_GrupoSanguineo,
                            estudiante.Est_TipoDocumento,
                            estudiante.Est_NumeroDocumento,
                            estudiante.Est_FechaExpedicionDoc,
                            estudiante.Est_PaisExpedicion,
                            estudiante.Est_DepartamentoExpedicion,
                            estudiante.Est_CiudadExpedicion,
                            estudiante.Est_Sexo,
                            estudiante.Est_EstadoCivil,
                            estudiante.Est_TelefonoPrincipal,
                            estudiante.Est_TelefonoSecundario,
                            estudiante.Est_CorreoElectronico,
                            estudiante.Est_FechaInscripcion,
                            Prog_ID = programaId
                        }, transaction);

                        // Confirmar la transacción
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Si se produce un error, deshacer la transacción
                        transaction.Rollback();
                        Console.WriteLine(ex.Message);
                        throw new Exception("Error al crear el estudiante", ex);
                    }
                }
            }
        }


        //public async Task CrearEstudiante(EstudianteME estudiante)
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(_connectionString))
        //        {
        //            await connection.OpenAsync();
        //            string sql = "INSERT INTO Estudiante (RolID, Estatus_Est_ID, Prog_ID, Est_PrimerNombre, Est_SegundoNombre, Est_PrimerApellido, Est_SegundoApellido, Est_FechaNacimiento," +
        //                " Est_PaisNacimiento, Est_DepartamentoNacimiento, Est_CiudadNacimiento, Est_Direccion, Est_GrupoSanguineo, Est_TipoDocumento, Est_NumeroDocumento, Est_FechaExpedicionDoc," +
        //                " Est_PaisExpedicion, Est_DepartamentoExpedicion, Est_CiudadExpedicion, Est_Sexo, Est_EstadoCivil, Est_TelefonoPrincipal, Est_TelefonoSecundario, Est_CorreoElectronico," +
        //                " Est_FechaInscripcion, Est_FechaActualizacion)" +
        //                " VALUES (@RolID, @Estatus_Est_ID, @Prog_ID, @Est_PrimerNombre, @Est_SegundoNombre, @Est_PrimerApellido, @Est_SegundoApellido, @Est_FechaNacimiento," +
        //                " @Est_PaisNacimiento, @Est_DepartamentoNacimiento, @Est_CiudadNacimiento, @Est_Direccion, @Est_GrupoSanguineo, @Est_TipoDocumento, @Est_NumeroDocumento," +
        //                " @Est_FechaExpedicionDoc, @Est_PaisExpedicion, @Est_DepartamentoExpedicion, @Est_CiudadExpedicion, @Est_Sexo, @Est_EstadoCivil, @Est_TelefonoPrincipal," +
        //                " @Est_TelefonoSecundario, @Est_CorreoElectronico, @Est_FechaInscripcion, @Est_FechaActualizacion)";
        //            await connection.ExecuteAsync(sql, estudiante);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw new Exception("Error al crear el estudiante", ex);
        //    }
        //}

        // ACTUALIZAR MODELO
        public async Task ActualizarEstudiante(int id, EstudianteME estudiante)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sql = @"UPDATE Estudiante SET 
                                    RolID = @RolID, 
                                    Estatus_Est_ID = @Estatus_Est_ID, 
                                    Prog_ID = @Prog_ID, 
                                    Est_PrimerNombre = @Est_PrimerNombre, 
                                    Est_SegundoNombre = @Est_SegundoNombre, 
                                    Est_PrimerApellido = @Est_PrimerApellido, 
                                    Est_SegundoApellido = @Est_SegundoApellido, 
                                    Est_FechaNacimiento = @Est_FechaNacimiento, 
                                    Est_PaisNacimiento = @Est_PaisNacimiento, 
                                    Est_DepartamentoNacimiento = @Est_DepartamentoNacimiento, 
                                    Est_CiudadNacimiento = @Est_CiudadNacimiento, 
                                    Est_Direccion = @Est_Direccion, 
                                    Est_GrupoSanguineo = @Est_GrupoSanguineo, 
                                    Est_TipoDocumento = @Est_TipoDocumento, 
                                    Est_NumeroDocumento = @Est_NumeroDocumento, 
                                    Est_FechaExpedicionDoc = @Est_FechaExpedicionDoc, 
                                    Est_PaisExpedicion = @Est_PaisExpedicion, 
                                    Est_DepartamentoExpedicion = @Est_DepartamentoExpedicion, 
                                    Est_CiudadExpedicion = @Est_CiudadExpedicion, 
                                    Est_Sexo = @Est_Sexo, 
                                    Est_EstadoCivil = @Est_EstadoCivil, 
                                    Est_TelefonoPrincipal = @Est_TelefonoPrincipal, 
                                    Est_TelefonoSecundario = @Est_TelefonoSecundario, 
                                    Est_CorreoElectronico = @Est_CorreoElectronico, 
                                    Est_FechaInscripcion = @Est_FechaInscripcion 
                                WHERE Est_ID = @id";
                    ;
                    await connection.ExecuteAsync(sql, new
                    {
                        id,
                        estudiante.RolID,
                        estudiante.Estatus_Est_ID,
                        estudiante.Prog_ID,
                        estudiante.Est_PrimerNombre,
                        estudiante.Est_SegundoNombre,
                        estudiante.Est_PrimerApellido,
                        estudiante.Est_SegundoApellido,
                        estudiante.Est_FechaNacimiento,
                        estudiante.Est_PaisNacimiento,
                        estudiante.Est_DepartamentoNacimiento,
                        estudiante.Est_CiudadNacimiento,
                        estudiante.Est_Direccion,
                        estudiante.Est_GrupoSanguineo,
                        estudiante.Est_TipoDocumento,
                        estudiante.Est_NumeroDocumento,
                        estudiante.Est_FechaExpedicionDoc,
                        estudiante.Est_PaisExpedicion,
                        estudiante.Est_DepartamentoExpedicion,
                        estudiante.Est_CiudadExpedicion,
                        estudiante.Est_Sexo,
                        estudiante.Est_EstadoCivil,
                        estudiante.Est_TelefonoPrincipal,
                        estudiante.Est_TelefonoSecundario,
                        estudiante.Est_CorreoElectronico,
                        estudiante.Est_FechaInscripcion
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error al modificar el estudiante", ex);
            }
        }


        public async Task BorrarEstudiante(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sql = "DELETE FROM Estudiante WHERE Est_ID = @ID";
                    await connection.ExecuteAsync(sql, new { ID = id });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error al eliminar el estudiante", ex);
            }
        }

    }
}
