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

        public async Task<EstudianteInscripcionME> CrearDatosEstudios(string tipoIngreso, string modalidad, string nivelIngreso, string programaInteres, string escuela, string periodo, string sede)
        {

            try
            {
                var parameters = new { TipoIngreso = tipoIngreso, Modalidad = modalidad, NivelIngreso = nivelIngreso, ProgramaInteres = programaInteres, Escuela = escuela, Periodo = periodo, Sede = sede };
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string programaSql = "INSERT INTO Est_inscripcion (TipoIngreso, Modalidad, NivelIngreso, ProgramaInteres, Escuela, Periodo, Sede) " +
                        "VALUES (@TipoIngreso, @Modalidad, @NivelIngreso, @ProgramaInteres, @Escuela, @Periodo, @Sede)";
                    return await connection.ExecuteScalarAsync<EstudianteInscripcionME>(programaSql, parameters);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error al insertar el programa", ex);
            }
        }

        public async Task<List<SexoEstudianteME>> ObtenerSexoEstudiante()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string programaSql = "SELECT Sexo_est FROM SexoEstudiante";
                    var result = await connection.QueryAsync<SexoEstudianteME>(programaSql);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error al obtener el sexo del estudiante", ex);
            }
        }


        public async Task CrearInformacionEstudiante(string primerNombre, string segundoNombre, string primerApellido, string segundoApellido, string fechaNacimiento,
                                                   string paisNacimiento, string departamentoNacimiento, string ciudadNacimiento, string direccion, string grupoSanguineo, string tipoDocumento, 
                                                   string numeroDocumento, string fechaExpedicion, string paisExpedicion, string departamentoExpedicion, string ciudadExpedicion,
                                                   string telefonoPrincipal, string telefonoSecundario, string correo, string sexoEstudiante, string estadoCivil)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string estudianteSql = @"INSERT INTO Estudiante (RolID, Estatus_Est_ID, Prog_ID, Est_PrimerNombre, Est_SegundoNombre, Est_PrimerApellido,
                                            Est_SegundoApellido, Est_FechaNacimiento, Est_PaisNacimiento, Est_DepartamentoNacimiento, Est_CiudadNacimiento, 
                                            Est_Direccion, Est_GrupoSanguineo, Est_TipoDocumento, Est_NumeroDocumento, Est_FechaExpedicionDoc, Est_PaisExpedicion,
                                            Est_DepartamentoExpedicion, Est_CiudadExpedicion, Est_Sexo, Est_EstadoCivil, Est_TelefonoPrincipal, 
                                            Est_TelefonoSecundario, Est_CorreoElectronico, Est_FechaInscripcion)
                                                VALUES (@RolID, @Estatus_Est_ID, @Prog_ID, @Est_PrimerNombre, @Est_SegundoNombre, @Est_PrimerApellido, @Est_SegundoApellido, @Est_FechaNacimiento,
                                                    @Est_PaisNacimiento, @Est_DepartamentoNacimiento, @Est_CiudadNacimiento, @Est_Direccion, @Est_GrupoSanguineo, @Est_TipoDocumento, @Est_NumeroDocumento,
                                                    @Est_FechaExpedicionDoc, @Est_PaisExpedicion, @Est_DepartamentoExpedicion, @Est_CiudadExpedicion, @Est_Sexo, @Est_EstadoCivil, @Est_TelefonoPrincipal,
                                                    @Est_TelefonoSecundario, @Est_CorreoElectronico, @Est_FechaInscripcion)";

                    var estudiante = new
                    {
                        RolID = 2, // Suponiendo que el RolID es fijo para los estudiantes
                        Estatus_Est_ID = 1, // Ajusta esto según corresponda
                        Prog_ID = 1, // Ajusta esto según corresponda
                        Est_PrimerNombre = primerNombre,
                        Est_SegundoNombre = segundoNombre,
                        Est_PrimerApellido = primerApellido,
                        Est_SegundoApellido = segundoApellido,
                        Est_FechaNacimiento = fechaNacimiento,
                        Est_PaisNacimiento = paisNacimiento,
                        Est_DepartamentoNacimiento = departamentoNacimiento,
                        Est_CiudadNacimiento = ciudadNacimiento,
                        Est_Direccion = direccion,
                        Est_GrupoSanguineo = grupoSanguineo,
                        Est_TipoDocumento = tipoDocumento,
                        Est_NumeroDocumento = numeroDocumento,
                        Est_FechaExpedicionDoc = fechaExpedicion,
                        Est_PaisExpedicion = paisExpedicion,
                        Est_DepartamentoExpedicion = departamentoExpedicion,
                        Est_CiudadExpedicion = ciudadExpedicion,
                        Est_Sexo = sexoEstudiante,
                        Est_EstadoCivil = estadoCivil,
                        Est_TelefonoPrincipal = telefonoPrincipal,
                        Est_TelefonoSecundario = telefonoSecundario,
                        Est_CorreoElectronico = correo,
                        Est_FechaInscripcion = DateTime.Now // Suponiendo que la fecha de inscripción es la fecha actual
                    };

                    await connection.ExecuteAsync(estudianteSql, estudiante);
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
