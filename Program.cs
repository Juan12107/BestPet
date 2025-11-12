using BESTPET_DEFINITIVO.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- SERVICIOS DE AUTENTICACIÓN ---
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
    options.Cookie.Name = "MyCookieAuth";
    options.LoginPath = "/Login";
});

builder.Services.AddRazorPages();

// --- DbContext con MySQL ---
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

// --- GESTIÓN DE ERRORES CORRECTA ---
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Muestra errores detallados en desarrollo
}
else
{
    app.UseExceptionHandler("/Error"); // Usa la página de error para producción
    app.UseHsts();
}
// Esta línea es para los errores 404
app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// --- EJECUTAR EL SEEDER DE DATOS ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

app.Run();