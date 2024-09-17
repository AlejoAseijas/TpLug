using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.models
{
    public class ProductoElectronico : Producto
    {
        public string Consumo { get; set; }

        public ProductoElectronico(string categoria, string subcategoria, string nombre, Proveedor proveedor, float precioCosto, string consumo)
        {
            base.Categoria = categoria;
            base.Categoria = subcategoria;
            base.Nombre = nombre;
            base.Proveedor = proveedor;
            base.PrecioCosto = precioCosto;
            this.Consumo = consumo;
        }

        public ProductoElectronico(int id, string categoria, string subcategoria, string nombre, Proveedor proveedor, float precioCosto, string consumo)
        {
            base.Id = id;
            base.Categoria = categoria;
            base.Categoria = subcategoria;
            base.Nombre = nombre;
            base.Proveedor = proveedor;
            base.PrecioCosto = precioCosto;
            this.Consumo = consumo;
        }

        public override float ObtenerPrecio()
        {
            throw new NotImplementedException();
        }
    }
}
