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
    public partial class MenuView : Form
    {
        public MenuView()
        {
            InitializeComponent();
        }
        private SyncService syncService = new SyncService();

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manageMdi(new UserView());
        }


        private void manageMdi(Form form)
        {
            form.MdiParent = this;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manageMdi(new ClienteView());
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manageMdi(new ProductosView());
        }

        private void eXITToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in this.MdiChildren)
            {
                form.Close();
            }
            new LogInView().Show();
            this.Hide();
        }

        private void ventasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manageMdi(new VentasView());
        }

        private void MenuView_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Quiere sincronizar los cambios con la base de datos?", "Confirmar sincronización", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool status = syncService.sync();

                if (status)
                {
                    MessageBox.Show("Sincronización completada con éxito.", "Éxito", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("La sincronización fallo.", "Error", MessageBoxButtons.OK);
                }
            }
        }

    }
}
