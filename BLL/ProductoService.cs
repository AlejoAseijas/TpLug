using BE.models;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProductoService : AbstractService<Producto>
    {
        private ProductoMapper mapper = new ProductoMapper();

        public ProductoService() 
        {
            base.Mapper = mapper;
        }

        public List<Proveedor> GetProveedores()
        {
            return mapper.ObtenerProveedores();
        }

    }
}
