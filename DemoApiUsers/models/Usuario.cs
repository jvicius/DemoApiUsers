using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApiUsers.models
{
    public class Usuario
    {
        public int IdUsuario { set; get; }
        public string nombredeusuario { set; get; }
        public string nombres { set; get; }
        public string apellidos { set; get; }
        public string contrasenia { set; get; }
        public bool Activo { set; get; }
    }
}
