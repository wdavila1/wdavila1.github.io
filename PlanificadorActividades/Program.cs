using PlanificadorActividades.EntityModels;

var builder = WebApplication.CreateBuilder(args);

// Agregar paginas Razer
builder.Services.AddRazorPages();

// Agregar controladores API
builder.Services.AddControllers();  // Permite usar controladores API

builder.Logging.AddConsole();

// Configurar la base de datos
builder.Services.AddDbContext<PlanificadorActividadesContext>();

var app = builder.Build();

// Configura el pipeline de la aplicación
app.UseHttpsRedirection();
app.UseStaticFiles();  // Sirve archivos estáticos como CSS, imágenes, etc.
app.UseRouting();

// Mapea las paginas razer
app.MapRazorPages();

// Mapea los controladores de API (esto permite que las rutas API funcionen)
app.MapControllers();  // Esto mapea las rutas de los controladores API

app.Run();


