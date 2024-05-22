using Dapper;
using inscripcionesApp.Models;
using InscripcionesApp.Models;
using System.Data.SqlClient;

namespace InscripcionesApp.DataAccess.DataEstudiante
{
    public class EstudianteRepository : IEstudianteRepository
    {
        private readonly string _connectionString;

        public EstudianteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public List<ProgramaME> ObtenerModalidades()
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT DISTINCT Modalidad FROM Programas";
                connection.Open();
                var modalidades = connection.Query<string>(query).ToList();
                return modalidades.Select(m => new ProgramaME { Modalidad = m }).ToList();
            }
        }


        public List<NivelIngresoME> ObtenerNivelIngreso()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT DISTINCT Nivel_Ingreso FROM nivelIngreso";
                connection.Open();
                var modalidades = connection.Query<string>(query).ToList();
                return modalidades.Select(m => new NivelIngresoME { Nivel_Ingreso = m }).ToList();
            }
        }


        //public async Task<int> CrearDatosEstudios(ProgramaME programa)
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(_connectionString))
        //        {
        //            await connection.OpenAsync();

        //            string programaSql = "INSERT INTO Programas (Modalidad, Nivel_Ingreso, Sede, Periodo, Estado) VALUES (@Modalidad, @NivelIngreso, @Sede, @Periodo, @Estado); SELECT SCOPE_IDENTITY();";
        //            return await connection.ExecuteScalarAsync<int>(programaSql, programa);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw new Exception("Error al insertar el programa", ex);
        //    }
    //}

    public async Task CrearInformacionEstudiante(EstudianteME estudiante, int programaId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string estudianteSql = "INSERT INTO Estudiante (RolID, Estatus_Est_ID, Prog_ID, Est_PrimerNombre, Est_SegundoNombre, Est_PrimerApellido, Est_SegundoApellido, Est_FechaNacimiento," +
                        " Est_PaisNacimiento, Est_DepartamentoNacimiento, Est_CiudadNacimiento, Est_Direccion, Est_GrupoSanguineo, Est_TipoDocumento, Est_NumeroDocumento, Est_FechaExpedicionDoc," +
                        " Est_PaisExpedicion, Est_DepartamentoExpedicion, Est_CiudadExpedicion, Est_Sexo, Est_EstadoCivil, Est_TelefonoPrincipal, Est_TelefonoSecundario, Est_CorreoElectronico," +
                        " Est_FechaInscripcion)" +
                        " VALUES (@RolID, @Estatus_Est_ID, @Prog_ID, @Est_PrimerNombre, @Est_SegundoNombre, @Est_PrimerApellido, @Est_SegundoApellido, @Est_FechaNacimiento," +
                        " @Est_PaisNacimiento, @Est_DepartamentoNacimiento, @Est_CiudadNacimiento, @Est_Direccion, @Est_GrupoSanguineo, @Est_TipoDocumento, @Est_NumeroDocumento," +
                        " @Est_FechaExpedicionDoc, @Est_PaisExpedicion, @Est_DepartamentoExpedicion, @Est_CiudadExpedicion, @Est_Sexo, @Est_EstadoCivil, @Est_TelefonoPrincipal," +
                        " @Est_TelefonoSecundario, @Est_CorreoElectronico, @Est_FechaInscripcion)";
                    await connection.ExecuteAsync(estudianteSql, new
                    {
                        RolID = 2, // Suponiendo que el RolID es fijo para los estudiantes
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
                        estudiante.Est_FechaInscripcion
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error al insertar el estudiante", ex);
            }
        }

    }
}
