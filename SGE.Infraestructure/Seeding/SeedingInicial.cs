using Microsoft.EntityFrameworkCore;
using SGE.Domain.Entities;
using SGE.Utilities.Codec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Infraestructure.Seeding
{
    public class SeedingInicial
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(new Usuario()
            {
                Id = 1,
                Nombre = "SISTEMA",
                Apellidos = "",
                Email = "side@gmail.com",
                Password = Codec.Encriptar("rogola2012"),
                FechaCreacion = DateTime.Now,
            });

            var listSubMenuDashboard = new List<SubMenu>();

            var SubMenuMain = new SubMenu()
            {
                Id = 1,
                Titulo = "Main",
                Url = "/dashboard"
            };
            listSubMenuDashboard.Add(SubMenuMain);

            var SubMenuReportAutos = new SubMenu()
            {
                Id = 2,
                Titulo = "Reporte Historial Autos",
                Url = "reporte-historial-autos"
            };
            listSubMenuDashboard.Add(SubMenuReportAutos);

            var SubMenuReportMotos = new SubMenu()
            {
                Id = 3,
                Titulo = "Reporte Historial Motos",
                Url = "reporte-historial-motos"
            };
            listSubMenuDashboard.Add(SubMenuReportMotos);

            var menuDashboard = new Menu()
            {
                Titulo = "Dashboard",
                Icono = "mdi mdi-gauge",
                subMenus = listSubMenuDashboard

            };





        }
    }
}
