using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Infraestructure.Context
{
    public class NovaMotosDbContext : DbContext
    {
        public NovaMotosDbContext(DbContextOptions<NovaMotosDbContext> options) : base(options)
        {
        }
    }
}
