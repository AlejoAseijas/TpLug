using Abstraccion;
using BE.models;
using MPP;
using System;
using System.Collections;
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

        public ProductoService() 
        {
            base.Mapper = mapper;
        }

        public DataTable GetProveedores()
        {
            return persistibleService.getTable("Proveedores");
        }

        public override Hashtable GetData(Producto producto) 
        {
            Hashtable data = base.GetData(producto);

            data.Add("IdProveedor", producto.Proveedor.Id);

            if (producto is ProductoAlimenticio) 
            {
                ProductoAlimenticio productoAlimenticio = (ProductoAlimenticio)producto;
                data.Add("FechaDeVencimiento", productoAlimenticio.FechaDeVencimiento);
            }

            if(producto is ProductoElectronico) 
            {
                ProductoElectronico productoElectronico = (ProductoElectronico)producto;
                data.Add("Consumo", productoElectronico.Consumo);
            }

            return data;
        }

    }
}
