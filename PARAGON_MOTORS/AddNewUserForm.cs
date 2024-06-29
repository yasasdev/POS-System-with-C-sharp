using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class AddNewUserForm : Form
    {
        private SqlConnection conn = null;
        public AddNewUserForm()
        {
            InitializeComponent();
            conn = DatabaseConnectivity.getConnection();

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateDateTime();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            DateTime now = DateTime.Now;
            string formattedDateTime = now.ToString("dddd, yyyy-MM-dd HH:mm:ss");
            labelDateTime.Text = formattedDateTime;
        }

        private void lblCLOSE_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            Hide();
        }

        private void AddNewUserForm_Load_1(object sender, EventArgs e)
        {
        }

        private bool IsAllFilled()
        {
            if (chkMr.Checked == false && chkMrs.Checked == false && chkMiss.Checked == false)
            {
                errorProvider1.SetError(chkMr, "Title is required!");
                return false;
            }
            else
            {
                errorProvider1.SetError(chkMr, string.Empty);
            }

            if (string.IsNullOrEmpty(txtFirstName.Text.Trim()))
            {
                errorProvider2.SetError(txtFirstName, "Name is required!");
                return false;
            }
            else
            {
                errorProvider2.SetError(txtFirstName, string.Empty);
            }

            if (string.IsNullOrEmpty(txtNIC.Text.Trim()))
            {
                errorProvider3.SetError(txtNIC, "NIC is required!");
                return false;
            }
            else
            {
                errorProvider3.SetError(txtNIC, string.Empty);
            }

            if (string.IsNullOrEmpty(txtMobileNumber.Text.Trim()))
            {
                errorProvider4.SetError(txtMobileNumber, "Mobile number is required!");
                return false;
            }
            else
            {
                errorProvider4.SetError(txtMobileNumber, string.Empty);
            }
            return true;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string title = null, gender = null;
            if (chkMr.Checked == true)
            {
                title = "Mr.";
            }
            else if (chkMrs.Checked == true)
            {
                title = "Mrs.";
            }
            else if (chkMiss.Checked == true)
            {
                title = "Miss.";
            }

            if (rdbMale.Checked == true)
            {
                gender = "Male";
            }
            else if (rdbFemale.Checked == true)
            {
                gender = "Female";
            }

            if (IsAllFilled() == true)
            {
                try
                {
                    String firstName = txtFirstName.Text;
                    String lastName = txtLastName.Text;
                    String NIC = txtNIC.Text;
                    if(NIC.Length > 12)
                    {
                        MessageBox.Show("Please check your NIC number again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    String address = txtAddress.Text;

                    if (address == "Address")
                    {
                        address = string.Empty;
                    }

                    String street = txtStreet.Text;

                    if (street == "Street")
                    {
                        street = string.Empty;
                    }

                    string city = txtCity.Text;

                    if (city == "City")
                    {
                        city = string.Empty;
                    }

                    SqlCommand cmd = new SqlCommand("INSERT INTO employee VALUES (@val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8, @val9, @val10, @val11, @val12, @val13)", conn);
                    cmd.Parameters.AddWithValue("@val1", title);
                    cmd.Parameters.AddWithValue("@val2", firstName);
                    cmd.Parameters.AddWithValue("@val3", lastName);
                    cmd.Parameters.AddWithValue("@val4", NIC);
                    cmd.Parameters.AddWithValue("@val5", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@val6", gender);
                    cmd.Parameters.AddWithValue("@val7", address);
                    cmd.Parameters.AddWithValue("@val8", street);
                    cmd.Parameters.AddWithValue("@val9", city);
                    cmd.Parameters.AddWithValue("@val10", txtMobileNumber.Text);
                    cmd.Parameters.AddWithValue("@val11", txtOfficeNumber.Text);
                    cmd.Parameters.AddWithValue("@val12", txtHomeNumber.Text);
                    cmd.Parameters.AddWithValue("@val13", txtEmailAddress.Text);

                    /*string email = txtEmailAddress.Text.Trim();

                    if (IsValidEmail(email))
                    {
                        cmd.Parameters.AddWithValue("@val13", email);
                    }
                    else
                    {
                        MessageBox.Show("Invalid email format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }*/

                    int result = cmd.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("User Registered successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            Regex regex = new Regex(emailPattern);
            return regex.IsMatch(email);
        }

        private void AddNewUserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
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

        public void clearData()
        {
            chkMr.Checked = false;
            chkMiss.Checked = false;
            chkMrs.Checked = false;
            txtFirstName.Clear();
            txtLastName.Clear();
            txtNIC.Clear();
            rdbMale.Checked = false;
            rdbFemale.Checked = false;
            txtAddress.Clear();
            txtStreet.Clear();
            txtCity.Clear();
            txtMobileNumber.Clear();
            txtOfficeNumber.Clear();
            txtHomeNumber.Clear();
            txtEmailAddress.Clear();
        }

        private void btnReset_MouseEnter(object sender, EventArgs e)
        {
            btnReset.ForeColor = Color.Red;
        }

        private void btnReset_MouseLeave(object sender, EventArgs e)
        {
            btnReset.ForeColor = Color.Black;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clearData();
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            DeleteUserForm deleteUserForm = new DeleteUserForm();
            deleteUserForm.Show();
            Hide();
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            UpdateUserForm updateUserForm = new UpdateUserForm();
            updateUserForm.Show();
            Hide();
        }

        private void pictureboxHOME_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            Hide();
        }

        private void btnDisplayUser_Click(object sender, EventArgs e)
        {
            DisplayUserForm display = new DisplayUserForm();
            display.Show();
            Hide();
        }

        private void pictureboxHOTLINE_Click(object sender, EventArgs e)
        {
            ContactUS contactUS = new ContactUS();
            contactUS.Show();
            Hide();
        }

        private void labelDateTime_Click(object sender, EventArgs e)
        {

        }
    }
}
