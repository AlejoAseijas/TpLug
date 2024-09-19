using BE.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace Presentacion.views
{
    public partial class LogInView : Form
    {
        private AuthService authService = new AuthService();
        public LogInView()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AuthDTO authDTO = new AuthDTO();
            authDTO.DNI = this.textBox1.Text;
            authDTO.password = authService.GenerateHash(this.textBox2.Text);

            if (authService.LogIn(authDTO))
            {
                Form menu = new MenuView();
                menu.Show(this);
                this.Hide();
            } else
            {
                MessageBox.Show("La contra es incorrecta");
            }
            Clear();

        }

        public void Clear()
        {
            this.textBox1.Text = string.Empty;
            this.textBox2.Text = string.Empty;
        }

    }
}
