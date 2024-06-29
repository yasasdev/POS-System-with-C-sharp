using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class DeleteUserForm : Form
    {
        private SqlConnection conn = null;
        public DeleteUserForm()
        {
            InitializeComponent();
            conn = DatabaseConnectivity.getConnection();
        }

        private void lblCLOSE_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            Hide();
        }

        private void pictureboxHOME_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            Hide();
        }

        private void DeleteUserForm_Load(object sender, EventArgs e)
        {
        }

        private void DeleteUserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("DELETE FROM employee WHERE First_Name = @val1", conn);
                cmd.Parameters.AddWithValue("@val1", txtFirstName.Text);
                int result = cmd.ExecuteNonQuery();

                if (result == 1)
                {
                    MessageBox.Show("User deleted successfully!");
                }
                else
                {
                    MessageBox.Show("Something went wrong!");
                }
                txtFirstName.Clear();
                txtFirstName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
