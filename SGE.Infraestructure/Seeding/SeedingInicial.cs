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

             





        }
    }
}
