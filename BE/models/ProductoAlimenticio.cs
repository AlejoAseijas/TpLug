using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.models
{
    public class ProductoAlimenticio : Producto
    {
        public DateTime FechaDeVencimiento { get; set; }

        public ProductoAlimenticio() { }    

        public ProductoAlimenticio(string categoria, string subcategoria, string nombre, Proveedor proveedor, float precioCosto, DateTime fechaDeVencimiento)
        {
            base.Categoria = categoria;
            base.SubCategoria = subcategoria;
            base.Nombre = nombre;
            base.Proveedor = proveedor;
            base.PrecioCosto = precioCosto;
            this.FechaDeVencimiento = fechaDeVencimiento;
        }

        public ProductoAlimenticio(int id, string categoria, string subcategoria, string nombre, Proveedor proveedor, float precioCosto, DateTime fechaDeVencimiento)
        {
            base.Id = id;
            base.Categoria = categoria;
            base.SubCategoria = subcategoria;
            base.Nombre = nombre;
            base.Proveedor = proveedor;
            base.PrecioCosto = precioCosto;
            this.FechaDeVencimiento = fechaDeVencimiento;
        }

        public override float ObtenerPrecio()
        {
            return (float)(this.PrecioCosto * 0.8 + this.PrecioCosto);
        }
    }
}
