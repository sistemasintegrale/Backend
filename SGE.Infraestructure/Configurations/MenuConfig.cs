using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGE.Domain.Entities;

namespace SGE.Infraestructure.Configurations
{
    public class MenuConfig : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.Property(e => e.Titulo).HasMaxLength(100);
            builder.Property(e => e.Icono).HasMaxLength(100);
        }
    }
}
