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
        private VentaMapper ventaMapper = new VentaMapper();

        public VentaService() 
        {
            base.Mapper = new ClienteVentaMapper();
        }

        public override int Create(ClienteVenta entity)
        {
            return 1;
        }

    }
}
