using BE.models;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProductoService
    {
        private InventarioMapper inventarioMapper = new InventarioMapper();
        public List<Inventario> GetAll() 
        {
            return inventarioMapper.GetAll();
        }
    }
}
