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

namespace PROEL2D_MIDTERM
{
    public partial class Login : Form
    {
        private static int loginAttempts = 0;
        private const int MAX_ATTEMPTS = 3;
        private DateTime lockoutTime;

        public Login()
        {
            InitializeComponent();
            lockoutTime = DateTime.Now;
        }

        string connectionString = @"Data Source = DESKTOP-4HKFQIA;Initial catalog = AbadDB; Integrated Security = true";
        private void guna2btnLogin_Click(object sender, EventArgs e)
        {
            if (loginAttempts >= MAX_ATTEMPTS)
            {
                MessageBox.Show("Maximum login attempts reached. Please restart the application.");
                return;
            }
            else
            {
                loginAttempts++;

                if (loginAttempts >= MAX_ATTEMPTS)
                {
                    lockoutTime = DateTime.Now.AddMinutes(3);
                    MessageBox.Show($"Maximum login attempts exceeded. You are locked out for 3 minutes.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Invalid username or password. You have {MAX_ATTEMPTS - loginAttempts} attempts remaining.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            // 1. Correct Logic Order: Validate inputs BEFORE connecting to the database.
            if (string.IsNullOrWhiteSpace(guna2txtUname.Text) || string.IsNullOrWhiteSpace(guna2txtPass.Text))
            {
                MessageBox.Show("Username and password cannot be empty!");
                return; // Exit the method immediately
            }

            string plainPassword = guna2txtPass.Text;
            //string hashedPassword = HashPassword(plainPassword);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand("Login_SP", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        string username = guna2txtUname.Text.Trim();
                        string password = guna2txtPass.Text.Trim();

                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();




                        if (reader.Read())
                        {
                            loginAttempts = 0;
                            string roleName = reader["RoleName"].ToString();

                            if (roleName == "Admin")
                            {
                                MessageBox.Show("Welcome, Admin!");
                                AdminDashboard dashboard = new AdminDashboard();
                                dashboard.Show();
                            }
                            else if (roleName == "Teacher")
                            {
                                MessageBox.Show("Welcome, Teacher!");
                                TeacherDashboard teachlandp = new TeacherDashboard();
                                teachlandp.Show();
                            }

                            else if (roleName == "Student")
                            {
                                MessageBox.Show("Welcome, Student!");
                                StudentDashboard landp = new StudentDashboard();
                                landp.Show();
                            }



                            this.Hide();

                        }
                        else 
                        {
                            reader.Close();
                            SqlCommand checkPendingCmd = new SqlCommand(
                                "SELECT P.Status FROM Users U INNER JOIN Profiles P ON U.ProfileID = P.ProfileID WHERE U.Username = @Username",
                                conn);
                            checkPendingCmd.Parameters.AddWithValue("@Username", username);
                            var status = checkPendingCmd.ExecuteScalar() as string;
                            if (status == "Pending")
                            {
                                MessageBox.Show("Your account is pending activation by the admin.");
                            }
                            else
                            {
                                loginAttempts++;
                                int remainingAttempts = MAX_ATTEMPTS - loginAttempts;
                                MessageBox.Show($"Invalid username or password. You have {remainingAttempts} attempts remaining.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Login failed: " + ex.Message);

                }

            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Registration regForm = new Registration();
            regForm.Show();
            this.Hide();
        }
    }
}
