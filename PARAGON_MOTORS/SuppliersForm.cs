using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class SuppliersForm : Form
    {
        private SqlConnection conn = null;
        string status = "ACTIVE";
        string status1 = "DEACTIVATED";
        private object dataSource;

        public SuppliersForm()
        {
            InitializeComponent();
            conn = DatabaseConnectivity.getConnection();

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateDateTime();

            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            listView1.Columns.Add("Company_Name", 100);
            listView1.Columns.Add("First_Name", 80);
            listView1.Columns.Add("Last_Name", 150);
            listView1.Columns.Add("Address", 80);
            listView1.Columns.Add("Mobile", 80);
            listView1.Columns.Add("Land_Number", 80);
            listView1.Columns.Add("Email", 80);
            listView1.Columns.Add("Status", 80);
        }

        private void FillListView()
        {
            try
            {
                string sqlQuery = "SELECT * FROM suppliers";
                SqlCommand command = new SqlCommand(sqlQuery, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["Company_Name"].ToString());
                    item.SubItems.Add(reader["First_Name"].ToString());
                    item.SubItems.Add(reader["Last_Name"].ToString());
                    item.SubItems.Add(reader["Address"].ToString());
                    item.SubItems.Add(reader["Mobile"].ToString());
                    item.SubItems.Add(reader["Land_Number"].ToString());
                    item.SubItems.Add(reader["Email"].ToString());
                    item.SubItems.Add(reader["Status"].ToString());
                    listView1.Items.Add(item);
                }
                reader.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void label3_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
            Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            AdministratorLOGIN admin = new AdministratorLOGIN();
            admin.Show();
            Hide();
        }

        private void btnSaveSupplier_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO suppliers VALUES (@val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8)", conn);
                cmd.Parameters.AddWithValue("@val1", txtCompanyName.Text);
                cmd.Parameters.AddWithValue("@val2", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@val3", txtLastName.Text);
                cmd.Parameters.AddWithValue("@val4", txtAddress.Text);
                cmd.Parameters.AddWithValue("@val5", txtMobile.Text);
                cmd.Parameters.AddWithValue("@val6", txtHome.Text);
                cmd.Parameters.AddWithValue("@val7", txtEmailAddress.Text);
                cmd.Parameters.AddWithValue("@val8", status);
                int result = cmd.ExecuteNonQuery();

                if (result == 1)
                {
                    MessageBox.Show("Supplier added successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshListView();
                }
                else
                {
                    MessageBox.Show("Something went wrong!");
                }
                reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void reset()
        {
            txtCompanyName.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtAddress.Clear();
            txtMobile.Clear();
            txtHome.Clear();
            txtEmailAddress.Clear();            
        }

        private void SuppliersForm_Load(object sender, EventArgs e)
        {
            FillListView();
        }

        private void RefreshListView()
        {
            listView1.Items.Clear(); // Clear existing items
            FillListView(); // Refill ListView
        }

        private void SuppliersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE suppliers SET First_Name = @val2, Last_Name = @val3, Address = @val4, Mobile = @val5, Land_Number = @val6, Email = @val7, Status = @val8 WHERE Company_Name = @val1", conn);
                cmd.Parameters.AddWithValue("@val2", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@val3", txtLastName.Text);
                cmd.Parameters.AddWithValue("@val4", txtAddress.Text);
                cmd.Parameters.AddWithValue("@val5", txtMobile.Text);
                cmd.Parameters.AddWithValue("@val6", txtHome.Text);
                cmd.Parameters.AddWithValue("@val7", txtEmailAddress.Text);
                cmd.Parameters.AddWithValue("@val8", status);
                cmd.Parameters.AddWithValue("@val1", txtCompanyName.Text);
                int result = cmd.ExecuteNonQuery();

                if (result == 1)
                {
                    MessageBox.Show("Supplier updated successfully!");
                }
                else
                {
                    MessageBox.Show("Something went wrong!");
                }
                reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE suppliers SET Status = @val2 where Company_Name = @val1", conn);
                cmd.Parameters.AddWithValue("@val2", status);
                cmd.Parameters.AddWithValue("@val1", txtCompanyName.Text);
                int result = cmd.ExecuteNonQuery();

                if (result == 1)
                {
                    MessageBox.Show("Supplier Activated successfully!");
                }
                else
                {
                    MessageBox.Show("Something went wrong!");
                }
                reset();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE suppliers SET Status = @val2 where Company_Name = @val1", conn);
                cmd.Parameters.AddWithValue("@val2", status1);
                cmd.Parameters.AddWithValue("@val1", txtCompanyName.Text);
                int result = cmd.ExecuteNonQuery();

                if (result == 1)
                {
                    MessageBox.Show("Supplier Deactivated successfully!");
                }
                else
                {
                    MessageBox.Show("Something went wrong!");
                }
                reset();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            reset();
        }
    }
}
