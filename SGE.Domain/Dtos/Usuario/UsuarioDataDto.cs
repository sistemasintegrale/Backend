using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Domain.Dtos.Usuario
{
    public class UsuarioDataDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Estado { get; set; }
        public string Token { get; set; } = null!;
        public int CodigoClienteNG { get; set; }
        public int CodigoClienteNM { get; set; }
    }
}
