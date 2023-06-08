using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Domain.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Icono { get; set; } = null!;
        public List<SubMenu>? subMenus { get; set; } 
    }
}
