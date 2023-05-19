using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Infraestructure.Configurations
{
    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.Property(e => e.FechaCreacion).HasColumnType("smalldatetime");
            builder.Property(e => e.FechaCreacion).HasDefaultValueSql("getdate()");
            builder.Property(e => e.FechaModificacion).HasColumnType("smalldatetime");
            builder.Property(e => e.FechaEliminacion).HasColumnType("smalldatetime");
            builder.Property(e => e.Flag).HasDefaultValueSql("1");
            builder.Property(e => e.Estado).HasDefaultValueSql("1");
            builder.Property(e => e.Nombre).HasMaxLength(100);
            builder.Property(e => e.Apellidos).HasMaxLength(200);
        }
    }
}
