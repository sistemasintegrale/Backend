using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Domain.Entities
{
    public class SubMenu
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Url { get; set; } = null!;
        public int? IdMenu { get; set; }
        public Menu? Menu { get; set; }
    }
}
