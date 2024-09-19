using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.models
{
    public class ClienteVenta
    {
        public Cliente Cliente { get; set; }
        public List<Venta> Ventas { get; set; }
    }
}
