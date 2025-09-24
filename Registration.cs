using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROEL2D_MIDTERM
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        string connectionString = @"Data Source=DESKTOP-4HKFQIA;Initial Catalog=AbadDB;Integrated Security=True";
        private void guna2btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(guna2txtFname.Text) ||
              string.IsNullOrWhiteSpace(guna2txtLname.Text) ||
              string.IsNullOrWhiteSpace(guna2txtAge.Text) ||
              string.IsNullOrWhiteSpace(cmbGender.Text) ||
              string.IsNullOrWhiteSpace(guna2txtPnum.Text) ||
              string.IsNullOrWhiteSpace(guna2txtAddress.Text) ||
              string.IsNullOrWhiteSpace(guna2txtEmail.Text))
            {
                MessageBox.Show("Please fill in all the required fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Profiles WHERE Email = @Email", connection);
                    checkCmd.Parameters.AddWithValue("@Email", guna2txtEmail.Text);

                    int userCount = (int)checkCmd.ExecuteScalar();
                    if (userCount > 0)
                    {
                        MessageBox.Show("This email is already registered. Please use a different one.",
                                         "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string plainPassword = HashPassword(guna2txtEmail.Text);
                    string hashedPassword = HashPassword(plainPassword);

                    SqlCommand cmd = new SqlCommand("Registration_SP", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FirstName", guna2txtFname.Text);
                    cmd.Parameters.AddWithValue("@LastName", guna2txtLname.Text);
                    cmd.Parameters.AddWithValue("@Age", int.Parse(guna2txtAge.Text));
                    cmd.Parameters.AddWithValue("@Gender", cmbGender.Text);
                    cmd.Parameters.AddWithValue("@Phone", guna2txtPnum.Text);
                    cmd.Parameters.AddWithValue("@Address", guna2txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Email", guna2txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);

                    SqlParameter usernameParam = new SqlParameter("@Username", SqlDbType.NVarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(usernameParam);

                    cmd.ExecuteNonQuery();

                    string generatedUsername = usernameParam.Value.ToString();

                    MessageBox.Show($"Registration successful!\n\n" +
                                    $"Your Username: {generatedUsername}\n" +
                                    $"Your Password: {generatedUsername}\n" +
                                    $"Wait for the Admin to approve your Account.",
                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private string HashPassword(string plainPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(plainPassword);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hash)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        private void label18_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
