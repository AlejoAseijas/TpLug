using Abstraccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.models
{
    public class User : AbstracEntity
    {
        public string DNI { get; set; }
        public string Password { get; set; }
    }
}
