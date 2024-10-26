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
                if (!ModesOfPersistible.DB.Equals(mode)) 
                {
                    Hashtable dataProducto = productoService.GetData(entity.Producto);

                    Hashtable dataInventario = dataProducto;
                    dataInventario.Add("Stock", entity.Stock);

                    int IdProducto = productoService.persistibleService.save(productoService.Mapper.TABLE_NAME, dataProducto, productoService.Mapper.ID_COLUMN);

                    dataInventario.Remove("IdProducto");
                    dataInventario.Add("IdProducto", IdProducto);

                    id = base.persistibleService.save(base.Mapper.TABLE_NAME, dataInventario, base.Mapper.ID_COLUMN);
                } else 
                {
                    base.Create(entity);
                }
            }
            catch (Exception ex) 
            { 
            }

            return id;
        }

        public void Update(Inventario oldData, Inventario newData)
        {

            if (!ModesOfPersistible.DB.Equals(mode)) 
            {
                Hashtable dataProducto = productoService.GetData(newData.Producto);

                Hashtable dataInventario = dataProducto;
                dataInventario.Add("Stock", newData.Stock);

                this.productoService.persistibleService.update(this.productoService.Mapper.TABLE_NAME, this.productoService.Mapper.ID_COLUMN, newData.Producto.Id, dataProducto);
                base.persistibleService.update(base.Mapper.TABLE_NAME, base.Mapper.ID_COLUMN, newData.Id, dataInventario);
            } else 
            {
                base.Update(oldData, newData);
            }
 

        }
    }
}
