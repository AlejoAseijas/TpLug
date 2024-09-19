using BE.models;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion.views
{
    public partial class VentasView : Form
    {
        public VentasView()
        {
            InitializeComponent();
        }
        private ClienteService clienteService = new ClienteService();
        private ProductoService productoService = new ProductoService();

        private void VentasView_Load(object sender, EventArgs e)
        {
            refreshClientes();
            refreshProductos();
        }

        private void refreshClientes()
        {
            this.dataGridViewClientes.DataSource = null;
            this.dataGridViewClientes.DataSource = clienteService.GetAll();
        }

        private void refreshProductos()
        {

            List<Producto> productos = new List<Producto>();
            this.dataGridViewProductos.DataSource = null;
            this.dataGridViewProductos.DataSource = productoService.GetAll();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Cliente cliente = (Cliente)this.dataGridViewClientes.CurrentRow.DataBoundItem;
            Inventario inventario = (Inventario)this.dataGridViewProductos.CurrentRow.DataBoundItem;
            int qty = Convert.ToInt32(this.txtQty.Text);

            if (cliente != null && inventario != null)
            {
                try
                {
                   // ventaService.Alta(cliente, inventario, qty);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
