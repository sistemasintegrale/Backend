using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Domain.Dtos.Token
{
    public class TokenRequestDto
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
