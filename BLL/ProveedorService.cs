using BE.models;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProveedorService : AbstractService<Proveedor>
    {
        public ProveedorService() 
        {
            base.Mapper = new ProveedorMapper();
        }
    }
}
