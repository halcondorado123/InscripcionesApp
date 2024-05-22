using Dapper;
using inscripcionesApp.Models;
using InscripcionesApp.Models;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InscripcionesApp.DataAccess.DataEstudiante
{
    public class EstudianteRepository : IEstudianteRepository
    {
        private readonly string _connectionString;

        public EstudianteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IngresosME ObtenerTipoIngreso(string tipoIngreso)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new { TipoIngreso = tipoIngreso };

                // La consulta ahora también selecciona el ID_Ingreso para el mapeo correcto del modelo
                string query = "SELECT ID_Ingreso, Tipo_Ingreso FROM Ingresos WHERE Tipo_Ingreso = @TipoIngreso";
                connection.Open();

                var ingreso = connection.QueryFirstOrDefault<IngresosME>(query, parameters);

                if (ingreso == null)
                {
                    // Log o manejar el caso donde no se encuentra ningún ingreso
                    Console.WriteLine($"No se encontró ningún ingreso con Tipo_Ingreso = {tipoIngreso}");
                }

                return ingreso;
            }
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
                var nivelIngreso = connection.Query<string>(query).ToList();
                return nivelIngreso.Select(m => new NivelIngresoME { Nivel_Ingreso = m }).ToList();
            }
        }
        public List<ProgramaME> ObtenerProgramaInteres(string modalidad, string nivelIngreso)
        {

            var nuevoNivelIngreso = AjustarNivelIngreso(nivelIngreso);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Escuela, Nombre_Programa, Modalidad, Nivel_Ingreso, Sede " +
                               "FROM Programas " +
                               "WHERE Modalidad = @Modalidad AND Nivel_Ingreso = @NivelIngreso";

                // Define el valor de nivelIngreso basado en la lógica de la modalidad solo si no está vacío
                

                var parameters = new { Modalidad = modalidad, NivelIngreso = nuevoNivelIngreso };

                connection.Open();

                var programas = connection.Query<ProgramaME>(query, parameters).ToList();

                return programas;
            }
        }

        private string AjustarNivelIngreso(string nivelIngreso)
        {
            if (nivelIngreso == "Bachillerato")
            {
                return "Técnico";
            }
            else if (nivelIngreso == "Técnico")
            {
                return "Tecnólogo";
            }
            else if (nivelIngreso == "Tecnólogo")
            {
                return "Profesional";
            }
            else
            {
                return nivelIngreso; // Mantener el mismo nivel de ingreso si es Profesional
            }
        }

        public string ObtenerEscuelaPorPrograma(string modalidad, string nivelIngreso, string nombrePrograma)
        {
            var nuevoNivelIngreso = AjustarNivelIngreso(nivelIngreso);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT DISTINCT Escuela " +
                               "FROM Programas " +
                               "WHERE Modalidad = @Modalidad " +
                               "AND Nivel_Ingreso = @NivelIngreso " +
                               "AND Nombre_Programa = @NombrePrograma";

                var parameters = new { Modalidad = modalidad, NivelIngreso = nuevoNivelIngreso, NombrePrograma = nombrePrograma };

                connection.Open();

                var escuela = connection.QueryFirstOrDefault<string>(query, parameters);

                return escuela;
            }
        }

        public List<ProgramaME> ObtenerSede(string modalidad, string nivelIngreso, string nombrePrograma)
        {
            var nuevoNivelIngreso = AjustarNivelIngreso(nivelIngreso);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT DISTINCT Sede " +
                                "FROM Programas " +
                                "WHERE MODALIDAD = @Modalidad " +
                                "AND Nivel_Ingreso = @NivelIngreso " +
                                "AND Nombre_Programa = @NombrePrograma";

                // Define el valor de nivelIngreso basado en la lógica de la modalidad solo si no está vacío


                var parameters = new { Modalidad = modalidad, NivelIngreso = nuevoNivelIngreso, NombrePrograma = nombrePrograma };

                connection.Open();

                var sedes = connection.Query<ProgramaME>(query, parameters).ToList();

                return sedes;
            }
        }

        public List<ProgramaME> ObtenerPeriodoActivo(string modalidad, string nivelIngreso, string nombrePrograma)
        {
            var nuevoNivelIngreso = AjustarNivelIngreso(nivelIngreso);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * " +
                                "FROM Programas " +
                                "WHERE MODALIDAD = @Modalidad " +
                                "AND Nivel_Ingreso = @NivelIngreso " +
                                "AND Nombre_Programa = @NombrePrograma";

                // Define el valor de nivelIngreso basado en la lógica de la modalidad solo si no está vacío


                var parameters = new { Modalidad = modalidad, NivelIngreso = nuevoNivelIngreso, NombrePrograma = nombrePrograma };

                connection.Open();

                var programas = connection.Query<ProgramaME>(query, parameters).ToList();

                return programas;
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
