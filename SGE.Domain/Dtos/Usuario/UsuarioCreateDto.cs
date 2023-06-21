using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Domain.Dtos.Usuario
{
    public class UsuarioCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int CodigoClienteNG { get; set; }
        public int CodigoClienteNM { get; set; }
        public string? Rol { get; set; }
    }
}
