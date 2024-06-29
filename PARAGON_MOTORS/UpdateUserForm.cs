using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class UpdateUserForm : Form
    {
        private SqlConnection conn = null;
        public UpdateUserForm()
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
        private void UpdateUserForm_Load(object sender, EventArgs e)
        {
        }

        private bool IsAllFilled()
        {
            if (string.IsNullOrEmpty(txtFirstName.Text.Trim()))
            {
                errorProvider1.SetError(txtFirstName, "Name is required!");
                return false;
            }
            else
            {
                errorProvider1.SetError(txtFirstName, string.Empty);
            }

            if (string.IsNullOrEmpty(txtNIC.Text.Trim()))
            {
                errorProvider2.SetError(txtNIC, "NIC is required!");
                return false;
            }
            else
            {
                errorProvider2.SetError(txtNIC, string.Empty);
            }

            if (rdbMale.Checked == false && rdbFemale.Checked == false)
            {
                errorProvider3.SetError(rdbFemale, "Gender is required!");
                return false;
            }
            else
            {
                errorProvider3.SetError(rdbFemale, string.Empty);
            }

            if (string.IsNullOrEmpty(txtAddress.Text.Trim()))
            {
                errorProvider4.SetError(txtAddress, "Address is required!");
                return false;
            }
            else
            {
                errorProvider4.SetError(txtAddress, string.Empty);
            }

            if (string.IsNullOrEmpty(txtMobileNumber.Text.Trim()))
            {
                errorProvider7.SetError(txtMobileNumber, "Mobile Number is required!");
                return false;
            }
            else
            {
                errorProvider7.SetError(txtMobileNumber, string.Empty);
            }
            return true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            String Gender = null;
            if (rdbMale.Checked == true)
            {
                Gender = "Male";
            }
            else if (rdbFemale.Checked == true)
            {
                Gender = "Female";
            }

            if (IsAllFilled() == true)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("UPDATE employee SET NIC = @val2, DOB = @val3, Gender = @val4, Address = @val5, Street = @val6, City = @val7, MobileNumber = @val8, OfficeNumber = @val9, HomeNumber = @val10, Email = @val11 WHERE FirstName = @val1", conn);
                    cmd.Parameters.AddWithValue("@val1", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@val2", txtNIC.Text);
                    cmd.Parameters.AddWithValue("@val3", dateTimePicker2.Value);
                    cmd.Parameters.AddWithValue("@val4", Gender);
                    cmd.Parameters.AddWithValue("@val5", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@val6", txtStreet.Text);
                    cmd.Parameters.AddWithValue("@val7", txtCity.Text);
                    cmd.Parameters.AddWithValue("@val8", txtMobileNumber.Text);
                    cmd.Parameters.AddWithValue("@val9", txtOfficeNumber.Text);
                    cmd.Parameters.AddWithValue("@val10", txtHomeNumber.Text);
                    cmd.Parameters.AddWithValue("@val11", txtEmail.Text);
                    int result = cmd.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("User updated successfully!");
                        clearData();
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReset_MouseEnter(object sender, EventArgs e)
        {
            btnReset.ForeColor = Color.Red;
        }

        private void btnReset_MouseLeave(object sender, EventArgs e)
        {
            btnReset.ForeColor = Color.Black;
        }

        public void clearData()
        {
            txtFirstName.Clear();
            txtNIC.Clear();
            rdbMale.Checked = false;
            rdbFemale.Checked = false;
            txtAddress.Clear();
            txtStreet.Clear();
            txtCity.Clear();
            txtMobileNumber.Clear();
            txtOfficeNumber.Clear();
            txtHomeNumber.Clear();
            txtEmail.Clear();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clearData();
        }

        private void txtAddress_Click(object sender, EventArgs e)
        {
            txtAddress.Text = string.Empty;
        }

        private void txtStreet_Click(object sender, EventArgs e)
        {
            txtStreet.Text = string.Empty;
        }

        private void txtCity_Click(object sender, EventArgs e)
        {
            txtCity.Text = string.Empty;
        }

        private void UpdateUserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }
    }
}
