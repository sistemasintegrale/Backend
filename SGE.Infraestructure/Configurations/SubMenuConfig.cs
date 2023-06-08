using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Infraestructure.Configurations
{
    public class SubMenuConfig : IEntityTypeConfiguration<SubMenu>
    {
        public void Configure(EntityTypeBuilder<SubMenu> builder)
        {
            builder.Property(e => e.Titulo).HasMaxLength(100);
            builder.Property(e => e.Url).HasMaxLength(100);
        }
    }
}
