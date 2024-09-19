using Abstraccion;
using BE.models;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class InventarioService : AbstractService<Inventario>
    {
        private ProductoService productoService = new ProductoService();
        public InventarioService() 
        {
            base.Mapper = (IGestor<Inventario>)new InventarioMapper();
        }
    }
}
