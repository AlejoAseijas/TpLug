using Abstraccion;
using BE.models;
using MPP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProductoService : AbstractService<Producto>
    {
        private ProductoMapper mapper = new ProductoMapper();
        private PersistibleService persistibleService = new PersistibleService();

        public ProductoService() 
        {
            base.Mapper = mapper;
        }

        public DataTable GetProveedores()
        {
            return persistibleService.getTable("Proveedores");
        }

    }
}
