using Abstraccion;
using BE.models;
using MPP;
using System;
using System.Collections;
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
            base.Mapper = new InventarioMapper();
        }

        public override int Create(Inventario entity)
        {
            int id = -1;
            try 
            {
                Hashtable dataProducto = productoService.GetData(entity.Producto);

                Hashtable dataInventario = dataProducto;
                dataInventario.Add("Stock", entity.Stock);

                int IdProducto = productoService.persistibleService.save(productoService.Mapper.TABLE_NAME, dataProducto, productoService.Mapper.ID_COLUMN);

                dataInventario.Remove("IdProducto");
                dataInventario.Add("IdProducto", IdProducto);

                id = base.persistibleService.save(base.Mapper.TABLE_NAME, dataInventario, base.Mapper.ID_COLUMN);
            }
            catch (Exception ex) 
            { 
            }

            return id;
        }
    }
}
