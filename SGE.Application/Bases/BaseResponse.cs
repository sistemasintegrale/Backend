using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Application.Bases
{
    public class BaseResponse<T>
    {
        public bool IsSucces { get; set; }
        public T? Data { get; set; }
        public string Mensaje { get; set; } = null!;
        public object? innerExeption { get; set; }
        public BaseResponse()
        {
            IsSucces = true;
        }
    }
}
