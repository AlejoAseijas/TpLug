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
    public partial class ClienteView : Form
    {
        public ClienteView()
        {
            InitializeComponent();
        }
        private ClienteService clienteService = new ClienteService();

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Cliente cliente = getClienteFromUI();

            try
            {
                clienteService.Create(cliente);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            refreshDataSource();
        }

        private void refreshDataSource()
        {
            List<Cliente> clientes = clienteService.GetAll();

            clientes.Sort();
            Clear();

            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = clientes;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }


        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            Cliente cliente = (Cliente)this.dataGridView1.CurrentRow.DataBoundItem;

            if (cliente != null)
            {
                //List<VentaDTO> ventasDelCliente = null;

                this.txtNombreCliente.Text = cliente.Nombre;
                this.txtApellidoCliente.Text = cliente.Apellido;
                this.txtDniCliente.Text = cliente.DNI;

                try
                {
                    //ventasDelCliente = ventaService.GetVentasPorCiente(cliente) != null ? ventaService.GetVentasPorCiente(cliente).ventas : new List<VentaDTO>();
                }
                catch (Exception ex) when (ex.Message == "La clave proporcionada no se encontró en el diccionario.")
                {
                    MessageBox.Show("El cliente no tiene ventas");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                //refreshVentas(ventasDelCliente);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Cliente clienteAModificar = (Cliente)this.dataGridView1.CurrentRow.DataBoundItem;

            if (clienteAModificar != null)
            {
                Cliente clienteNuevosDatos = getClienteFromUI();
                clienteService.Update(clienteAModificar, clienteNuevosDatos);
            }
            refreshDataSource();
        }

        private Cliente getClienteFromUI()
        {
            return new Cliente(this.txtNombreCliente.Text, this.txtApellidoCliente.Text, this.txtDniCliente.Text);
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            Cliente cliente = (Cliente)this.dataGridView1.CurrentRow.DataBoundItem;
            clienteService.Delete(cliente);
            refreshDataSource();
        }

        private void ClienteView_Load(object sender, EventArgs e)
        {
            refreshDataSource();
        }

        public void Clear()
        {
            this.txtNombreCliente.Text = string.Empty;
            this.txtApellidoCliente.Text = string.Empty;
            this.txtDniCliente.Text = string.Empty;
        }
    }
}
