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
    public partial class AdminTeacher : Form
    {
        public AdminTeacher()
        {
            InitializeComponent();
        }

        private void guna2btnDashboard_Click(object sender, EventArgs e)
        {
            AdminDashboard AdminDash = new AdminDashboard();
            AdminDash.Show();
         
        }

        private void guna2btnStudent_Click(object sender, EventArgs e)
        {
            AdminStudent AdminStud = new AdminStudent();
            AdminStud.Show();
          
        }

        private void guna2btnTeacher_Click(object sender, EventArgs e)
        {
            AdminTeacher AdminTeach = new AdminTeacher();
            AdminTeach.Show();
          
        }

        private void guna2btnSubject_Click(object sender, EventArgs e)
        {
            AdminSubject AdminSub = new AdminSubject();
            AdminSub.Show();
        
        }

        private void guna2btnReports_Click(object sender, EventArgs e)
        {
            AdminReports AdminRep = new AdminReports();
            AdminRep.Show();
         
        }

        private void guna2btnLogs_Click(object sender, EventArgs e)
        {
            AdminLogs AdminLogs = new AdminLogs();
            AdminLogs.Show();
           
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure you want to logout?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            Login login = new Login();
            login.Show();
           
        }
    }
}
