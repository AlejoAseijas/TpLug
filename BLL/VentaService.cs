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
    public class VentaService : AbstractService<ClienteVenta>
    {
        private ClienteVentaMapper clienteVentaMapper = new ClienteVentaMapper();
        private InventarioMapper inventarioMapper = new InventarioMapper();

        public VentaService() 
        {
            base.Mapper = clienteVentaMapper;
        }

        public List<Venta> GetVentasByIdCliente(int IdCliente)
        {
            return clienteVentaMapper.GetVentasByIdCliente(IdCliente);
        }

        public bool checkVenta(Inventario inventario, int qtyVender)
        {
            bool valid = false;

            if (inventario.Stock - qtyVender > 0) 
            {
                inventario.Stock -= qtyVender;
                inventarioMapper.Update(inventario, inventario);
                valid = true;
            }

            return valid;
        }

    }
}
