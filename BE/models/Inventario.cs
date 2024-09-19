using Abstraccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.models
{
    public class Inventario : AbstracEntity, ICloneable
    {
        public Producto Producto { get; set; }
        public int Stock { get; set; }

        public object Clone()
        {
            return (Inventario)this.MemberwiseClone();
        }
    }
}
