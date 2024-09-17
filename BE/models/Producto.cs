using Abstraccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.models
{
    public abstract class Producto : AbstracEntity
    {
        public string Categoria { get; set; }
        public string SubCategoria { get; set; }
        public string Nombre { get; set; }
        //Agregacion
        public Proveedor Proveedor { get; set; }
        public float PrecioCosto { get; set; }

        public abstract float ObtenerPrecio();

    }
}
