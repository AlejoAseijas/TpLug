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
    public partial class UserView : Form
    {
        private AuthService authService = new AuthService();
        
        public UserView()
        {
            InitializeComponent();
        }

        private void UserView_Load(object sender, EventArgs e)
        {
            refresh();
        }

        private void refresh() 
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = authService.GetAll();

            userAuthForm1.DNI = string.Empty;
            userAuthForm1.Password = string.Empty;

            groupBox1.Visible = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            groupBox1.Visible = true;

        }

        public static Boolean IsNumeric(string valor)
        {
            int result;
            return int.TryParse(valor, out result);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            User user = new User();

            user.DNI = userAuthForm1.DNI;
            user.Password = authService.GenerateHash(userAuthForm1.Password);

            authService.Create(user);

            groupBox1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataGridViewRow data = (DataGridViewRow)this.dataGridView1.CurrentRow;

            if (data != null) 
            {
                int IdUser = Convert.ToInt32(data.Cells["IdUser"].Value.ToString());
                authService.DeleteById(IdUser);
                refresh();
            }

        }
    }
}
