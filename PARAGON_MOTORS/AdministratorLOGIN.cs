using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class AdministratorLOGIN : Form
    {
        private SqlConnection conn = null;

        public AdministratorLOGIN()
        {
            InitializeComponent();
            conn = DatabaseConnectivity.getConnection();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void lblCLOSE_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            Hide();
        }

        private void btnLOGIN_Click(object sender, EventArgs e)
        {
            // Assuming you have text boxes txtUsername and txtPassword for username and password input
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Add your authentication logic here
            bool isAuthenticated = AuthenticateUser(username, password);

            if (isAuthenticated)
            {
                AddQuantityForm quantityForm = new AddQuantityForm();
                quantityForm.Show();
                Dashboard db = new Dashboard();
                db.Hide();
            }
            else
            {
                // Login failed
                MessageBox.Show("Incorrect Username or Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsername.Clear();
                txtPassword.Clear();
                txtUsername.Focus();
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            string query = "SELECT * FROM admin_login where username = @v1 and password = @v2";
            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@v1", txtUsername.Text);
                command.Parameters.AddWithValue("@v2", txtPassword.Text);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private void AdministratorLOGIN_Load(object sender, EventArgs e)
        {
        }
    }
}
