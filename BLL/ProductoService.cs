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
        private ProductoMapper productoMapper = new ProductoMapper();

        public List<Inventario> GetAll() 
        {
            return inventarioMapper.GetAll();
        }

        public int Create(Inventario inventario) 
        {
            return inventarioMapper.Create(inventario); 
        }

        public List<Proveedor> GetProveedores() 
        {
            return productoMapper.ObtenerProveedores();
        }

        public void Update(Inventario inventario, Inventario inventario1) {
        inventarioMapper.Update(inventario, inventario1);
        }


        public void Delete(Inventario inventario) 
        {
            inventarioMapper.DeleteById(inventario.Id);
        }


    }
}
