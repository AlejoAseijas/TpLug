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

        public VentaService() 
        {
            base.Mapper = clienteVentaMapper;
        }

        public List<Venta> GetVentasByIdCliente(int IdCliente)
        {
            return clienteVentaMapper.GetVentasByIdCliente(IdCliente);
        }

    }
}
