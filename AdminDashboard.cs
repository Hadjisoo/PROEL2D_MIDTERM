using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROEL2D_MIDTERM
{
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure you want to logout?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            this.Hide();
            Login login = new Login();
            login.Show();

        }
    }
}
