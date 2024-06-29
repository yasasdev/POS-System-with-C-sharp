using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class AddQuantityForm : Form
    {
        private SqlConnection conn;
        public AddQuantityForm()
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

        private void lblClose_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            Hide();
        }

        private void btnAddGRN_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtModel.Text.Trim() != "")
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM invoice_details WHERE Model = @val1", conn);
                    cmd.Parameters.AddWithValue("@val1", txtModel.Text);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string quantity = reader["Quantity"].ToString();
                        int totQuantity = Convert.ToInt32(txtQuantity.Text) + Convert.ToInt32(quantity);

                        reader.Close();
                        SqlCommand cmd1 = new SqlCommand("UPDATE invoice_details SET Quantity = @val2 WHERE Model = @val1", conn);
                        cmd1.Parameters.AddWithValue("@val2", totQuantity);
                        cmd1.Parameters.AddWithValue("@val1", txtModel.Text);
                        int result = cmd1.ExecuteNonQuery();

                        if (result == 1)
                        {
                            MessageBox.Show("Quantity updated successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearData();
                        }
                        else
                        {
                            MessageBox.Show("Something went wrong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ClearData();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Sorry, model does not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ClearData();
                    }

                }
                else
                {
                    MessageBox.Show("Please enter Model!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ClearData();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                ClearData();
            }
        }

        private void NewGRNForm_Load(object sender, EventArgs e)
        {
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        public void ClearData()
        {
            txtModel.Clear();
            txtQuantity.Clear();
        }

        private void NewGRNForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ContactUS contactUS = new ContactUS();
            contactUS.Show();
            Hide();
        }
    }
}
