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

        private void sYNCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            syncService.sync();
        }
    }
}
