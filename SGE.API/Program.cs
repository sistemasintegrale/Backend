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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddAuthentication(Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
var mapperConfig = new MapperConfiguration(m =>
{
    m.AddProfile(new UsuarioMappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddCors(options => options.AddPolicy("AllowWebapp",
                                    builder => builder.AllowAnyOrigin()
                                                    .AllowAnyHeader()
                                                    .AllowAnyMethod()));


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseCors("AllowWebapp"); 

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
