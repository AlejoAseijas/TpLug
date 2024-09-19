using Abstraccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.models
{
    public class Venta : AbstracEntity
    {
        public string Producto { get; set; }
        public int Qty {  get; set; }
        public float PrecioVenta {  get; set; } 
    }
}
