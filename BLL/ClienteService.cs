using Abstraccion;
using BE.models;
using BLL;
using MPP;
using System.Collections.Generic;

namespace BLL
{
    public class ClienteService : AbstractService<Cliente>
    {
        public ClienteService() 
        {
            base.Mapper = new ClienteMapper();
        }


    }

}