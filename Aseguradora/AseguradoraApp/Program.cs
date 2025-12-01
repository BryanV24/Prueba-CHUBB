using Entidades.Interfaces;
using Microsoft.OpenApi;
using Repositorio.AccesoDatos;
using Repositorio.Dbconexion;
using Servicios;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

//Obtencion de la cadena de conexion a la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddSingleton(new ConexionDB(connectionString!));

builder.Services.AddScoped<IAutenticacionRepositorio, AutenticacionRepositorio>();
builder.Services.AddScoped<IAseguradoRepositorio, AseguradoRepositorio> ();
builder.Services.AddScoped<ISeguroRepositorio, SeguroRepositorio> ();
builder.Services.AddScoped<IAsignacionSegurosAseguradosRepositorio, AsignacionSeguroAseguradoRepositorio>();

builder.Services.AddScoped<AutenticacionServicio>();
builder.Services.AddScoped<AseguradoServicio>();
builder.Services.AddScoped<SeguroServicio>();
builder.Services.AddScoped<AsignarSegurosAseguradoServicio>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Aseguradora APP", Version = "v1" });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAngular");

app.MapControllers();

app.Run();
