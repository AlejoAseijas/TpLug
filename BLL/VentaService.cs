using BE.execptions;
using BE.models;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class VentaService
    {
        private InventarioMapper inventarioMapper = new InventarioMapper();

        public void Alta(Cliente cliente, Inventario inventario, int qty)
        {
            if (inventario.Stock >= qty)
            {
                Venta venta = new Venta();
                ClienteVenta clienteVenta = new ClienteVenta();

                venta.Qty = qty;
                venta.PrecioVenta = inventario.Producto.ObtenerPrecio();
                venta.Producto = inventario.Producto;

                
               
                //ventaMapper.Alta(venta);

                Inventario inventarioNuevo = (Inventario)inventario.Clone();
                inventarioNuevo.Stock -= qty;

                inventarioMapper.Update(inventario, inventarioNuevo);
            }
            else
            {
                throw new StockException("No hay stock suficiente");
            }
        }
    }
}
