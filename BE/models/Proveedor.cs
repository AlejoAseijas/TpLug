using Abstraccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.models
{
    public class Proveedor : AbstracEntity
    {
        public string Nombre { get; set; }

        public override string ToString()
        {
            return this.Nombre;
        }
    }
}
