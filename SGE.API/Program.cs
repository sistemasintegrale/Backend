using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGE.API.Extensions;
using SGE.Application.Interfaces;
using SGE.Application.Mappers;
using SGE.Application.Services;
using SGE.Infraestructure.Context;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
var connectionStringNovaGlass = builder.Configuration.GetConnectionString("ConnectionNovaGlass");
var connectionStringNovaMotos = builder.Configuration.GetConnectionString("ConnectionNovaMotos");


builder.Services.AddAuthentication(Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionStringNovaGlass));
builder.Services.AddDbContext<NovaMotosDbContext>(options => options.UseSqlServer(connectionStringNovaMotos));

var mapperConfig = new MapperConfiguration(m =>
{
    m.AddProfile(new UsuarioMappingProfile());
    m.AddProfile(new ReporteHistorialMappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IReportesRepository, ReportesRepository>();
builder.Services.AddScoped<IOrdenReparacionRepository, OrdenReparacionRepository>();

builder.Services.AddCors(options => options.AddPolicy("AllowWebapp",
                                    builder => builder.AllowAnyOrigin()
                                                    .AllowAnyHeader()
                                                    .AllowAnyMethod()));

 
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

var app = builder.Build();

//Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowWebapp");
app.UseAuthorization();

app.MapControllers();

app.Run();