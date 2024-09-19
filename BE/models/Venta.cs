using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.models
{
    public class Venta
    {
        public Producto Producto { get; set; }
        public int Qty {  get; set; }
        public float PrecioVenta {  get; set; } 
    }
}
