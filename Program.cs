using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data.Repositories;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data.UnitofWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LinqExampleContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 41))
    ));
    
// Registro de UnitOfWork y repositorio genérico
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// CORS opcional si lo deseas usar con frontend más adelante
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}



app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowAll"); // Opcional

app.UseAuthorization();

app.MapControllers();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyChamba API V1");
    c.RoutePrefix = string.Empty;
});

app.Run();