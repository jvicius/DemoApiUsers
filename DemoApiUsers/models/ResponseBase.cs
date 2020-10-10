using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApiUsers.models
{
    public class ResponseBase<T>
    {
        public bool TieneResultado { set; get; }
        public string Mensaje { set; get; }
        public T Modelo { set; get; }
    }
}
