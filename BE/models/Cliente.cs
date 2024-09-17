using Abstraccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.models
{
    public class Cliente : AbstracEntity, IComparable<Cliente>
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }

        public Cliente() { }
        public Cliente(string nombre, string apellido, string DNI)
        {
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.DNI = DNI;
        }

        public Cliente(int id, string nombre, string apellido, string DNI)
        {
            base.Id = id;
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.DNI = DNI;
        }

        public int CompareTo(Cliente obj)
        {
            return obj == null ? 1 : this.Nombre.CompareTo(obj.Nombre);
        }
    }
}
