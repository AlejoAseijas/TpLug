using Abstraccion;
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
        private ClienteService clienteService = new ClienteService();
        public ClienteView()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Cliente cliente = getClienteFromUI();

            try
            {
                string[] nombresInvalidos = { "Test", "prueba", "adolf" };

                if (nombresInvalidos.Contains(cliente.Nombre, StringComparer.OrdinalIgnoreCase))
                {
                    MessageBox.Show("El nombre ingresado no es valido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.txtNombreCliente.Text = string.Empty;
                    this.txtApellidoCliente.Text = string.Empty;
                    this.txtDniCliente.Text = string.Empty;
                } else
                {
                    clienteService.Create(cliente);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            refreshDataSource();
        }

        private void refreshDataSource()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = clienteService.GetAll();
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Clear();
        }


        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            Cliente cliente = GetClienteByDataGridView();

            if (cliente != null)
            {

                this.txtNombreCliente.Text = cliente.Nombre;
                this.txtApellidoCliente.Text = cliente.Apellido;
                this.txtDniCliente.Text = cliente.DNI;

                try
                {

                }
                catch (Exception ex) when (ex.Message == "La clave proporcionada no se encontró en el diccionario.")
                {
                    MessageBox.Show("El cliente no tiene ventas");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        public Cliente GetClienteByDataGridView() 
        {
            Cliente cliente = new Cliente();
            DataGridViewRow data = (DataGridViewRow)this.dataGridView1.CurrentRow;

            if (data != null) 
            {
                cliente.Id = Convert.ToInt32(data.Cells["IdCliente"].Value.ToString());
                cliente.Nombre = data.Cells["Nombre"].Value.ToString();
                cliente.Apellido = data.Cells["Apellido"].Value.ToString();
                cliente.DNI = data.Cells["DNI"].Value.ToString();
            }

            return cliente;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Cliente clienteAModificar = GetClienteByDataGridView();

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
            Cliente cliente = GetClienteByDataGridView();
            clienteService.DeleteById(cliente.Id);
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
