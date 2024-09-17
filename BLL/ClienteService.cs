using BE.models;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ClienteService
    {
        private ClienteMapper clienteMapper = new ClienteMapper();

        public List<Cliente> GetAll()
        {
            return clienteMapper.GetAll();
        }

        public void Create(Cliente cliente) 
        {
            clienteMapper.Create(cliente);
        }

        public void Update(Cliente cliente, Cliente cliente1)
        {
            clienteMapper.Update(cliente, cliente1);
        }

        public void Delete(Cliente cliente)
        {
            clienteMapper.DeleteById(cliente.Id);
        }
    }
}
