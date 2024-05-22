using InscripcionesApp.Controllers;
using InscripcionesApp.DataAccess.DataEstudiante;
using InscripcionesApp.DataAccess.DataFuncionario;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession();
// Agregar la configuraci�n desde el archivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false);

// Obtener la cadena de conexi�n desde la configuraci�n
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Agregar la cadena de conexi�n como servicio
builder.Services.AddSingleton(connectionString);
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<IEstudianteRepository, EstudianteRepository>();


// Agregar otros servicios necesarios
builder.Services.AddControllersWithViews();

//builder.Services.AddSession(options =>
//{
//    options.Cookie.Name = ".YourApp.Session";
//    options.IdleTimeout = TimeSpan.FromMinutes(30); // Puedes ajustar el tiempo de expiraci�n seg�n tus necesidades
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true; // Hace que la sesi�n est� disponible durante la duraci�n de la solicitud
//});

var app = builder.Build();

// Configurar el middleware y el enrutamiento
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();

app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.None
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
