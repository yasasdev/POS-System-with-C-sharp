using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class ChangeAccountSetting : Form
    {
        private SqlConnection conn = null;
        public ChangeAccountSetting()
        {
            InitializeComponent();
            conn = DatabaseConnectivity.getConnection();
        }

        private void ChangeAccountSetting_Load(object sender, EventArgs e)
        {
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string oldPassword = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM admin_login WHERE Username = @val1 AND Password = @val2", conn);

                cmd.Parameters.AddWithValue("@val1", txtUsername.Text);
                cmd.Parameters.AddWithValue("@val2", txtOldPassword.Text);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reader.Close();
                    if (newPassword == confirmPassword)
                    {
                        try
                        {
                            SqlCommand cmd1 = new SqlCommand("UPDATE admin_login SET Username = @val1, Password = @val2", conn);
                            cmd1.Parameters.AddWithValue("@val1", txtUsername.Text);
                            cmd1.Parameters.AddWithValue("@val2", txtConfirmPassword.Text);
                            int result = cmd1.ExecuteNonQuery();

                            if (result == 1)
                            {
                                MessageBox.Show("Password updated successfully!");
                                clearData();
                                txtUsername.Focus();
                            }
                            else
                            {
                                MessageBox.Show("Something went wrong!");
                                clearData();
                                txtUsername.Focus();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            clearData();
                            txtUsername.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry, password does not match. Try again!");
                        clearData();
                        txtUsername.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Sorry, Username or Password Incorrect", "Error", MessageBoxButtons.OK);
                    clearData();
                    txtUsername.Focus();
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clearData();
                txtUsername.Focus();
            }
        }

        public void clearData()
        {
            txtUsername.Clear();
            txtOldPassword.Clear();
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();
        }

        private void ChangeAccountSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }

        private void pictureboxHOME_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            Hide();
        }

        private void lblCLOSE_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
            Hide();
        }

        private void pictureboxHOTLINE_Click(object sender, EventArgs e)
        {
            ContactUS contactUS = new ContactUS();
            contactUS.Show();
            Hide();
        }
    }
}
