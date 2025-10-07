using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PROEL2D_MIDTERM
{
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
            LoadStudentChart();
            LoadTeacherChart();
            LoadCount();
        }
        string connectionString = Database.ConnectionString;

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Close();
            }

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

        private void guna2btnDashboard_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {

        }
        private void LoadStudentChart()
        {
            string sqlQuery = "SELECT Status, COUNT(*) AS TotalCount " +
                      "FROM Profiles " +
                      "WHERE ProfileID IN (SELECT ProfileID FROM Users WHERE RoleID = (SELECT RoleID FROM Roles WHERE RoleName = 'Student')) " +
                      "GROUP BY Status";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuery, conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    chartStudentStatus.Series.Clear();
                    chartStudentStatus.ChartAreas.Clear();

                    chartStudentStatus.ChartAreas.Add(new ChartArea("MainChart"));

                    Series series = new Series("StudentStatus");
                    series.ChartType = SeriesChartType.Column;
                    series.IsValueShownAsLabel = true;

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string status = row["Status"].ToString();
                        int count = Convert.ToInt32(row["TotalCount"]);
                        series.Points.AddXY(status, count);
                    }

                    chartStudentStatus.Series.Add(series);

                    chartStudentStatus.Titles.Clear();
                    chartStudentStatus.Titles.Add("Student");
                    chartStudentStatus.ChartAreas["MainChart"].AxisX.Title = "Status";
                    chartStudentStatus.ChartAreas["MainChart"].AxisY.Title = "Number of Students";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading the chart: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void LoadTeacherChart()
        {
            string sqlQuery = "SELECT Status, COUNT(*) AS TotalCount " +
                      "FROM Profiles " +
                      "WHERE ProfileID IN (SELECT ProfileID FROM Users WHERE RoleID = (SELECT RoleID FROM Roles WHERE RoleName = 'Instructor')) " +
                      "GROUP BY Status";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuery, conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    chartTeacher.Series.Clear();
                    chartTeacher.ChartAreas.Clear();

                    chartTeacher.ChartAreas.Add(new ChartArea("MainChart"));

                    Series series = new Series("TeacherStatus");
                    series.ChartType = SeriesChartType.Column;
                    series.IsValueShownAsLabel = true;

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string status = row["Status"].ToString();
                        int count = Convert.ToInt32(row["TotalCount"]);
                        series.Points.AddXY(status, count);
                    }

                    chartTeacher.Series.Add(series);

                    chartTeacher.Titles.Clear();
                    chartTeacher.Titles.Add("Teacher");
                    chartTeacher.ChartAreas["MainChart"].AxisX.Title = "Status";
                    chartTeacher.ChartAreas["MainChart"].AxisY.Title = "Number of Teacher";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading the chart: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void LoadCount()
        {

            string sqlQuery_TotalStudentCount = "SELECT COUNT(p.ProfileID) " +
                                          "FROM Profiles AS p " +
                                          "INNER JOIN Users AS u ON p.ProfileID = u.ProfileID " +
                                          "INNER JOIN Roles AS r ON u.RoleID = r.RoleID " +
                                          "WHERE r.RoleName = 'Student' AND p.Status = 'Active'";

            string sqlQuery_TotalTeacherCount = "SELECT COUNT(p.ProfileID) " +
                                          "FROM Profiles AS p " +
                                          "INNER JOIN Users AS u ON p.ProfileID = u.ProfileID " +
                                          "INNER JOIN Roles AS r ON u.RoleID = r.RoleID " +
                                          "WHERE r.RoleName = 'Instructor' AND p.Status = 'Active'";

            string sqlQuery_TotalSubjectCount = "SELECT COUNT(CourseID)" +
                                                "FROM Courses " +
                                                "WHERE Status = 'Active'";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();

                SqlCommand countStudentcmd = new SqlCommand(sqlQuery_TotalStudentCount, conn);
                int StudentCount = (int)countStudentcmd.ExecuteScalar();
                lblActStudCount.Text = StudentCount.ToString();

                SqlCommand countTeachercmd = new SqlCommand(sqlQuery_TotalTeacherCount, conn);
                int TeacherCount = (int)countTeachercmd.ExecuteScalar();
                lblActTeacCount.Text = TeacherCount.ToString();

                SqlCommand countSubjectcmd = new SqlCommand(sqlQuery_TotalSubjectCount, conn);
                int SubjectCount = (int)countSubjectcmd.ExecuteScalar();
                lblSubjectTotal.Text = SubjectCount.ToString();
            }
        }

    }
}
