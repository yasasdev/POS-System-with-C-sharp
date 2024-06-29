using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class GRNRetrunForm : Form
    {
        private SqlConnection conn = null;
        public GRNRetrunForm()
        {
            InitializeComponent();
            conn = DatabaseConnectivity.getConnection();

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateDateTime();
            InitializeListView();
        }

        private void InitializeListView()
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            listView1.Columns.Add("Barcode", 100);
            listView1.Columns.Add("Model", 80);
            listView1.Columns.Add("Item_Name", 150);
            listView1.Columns.Add("Quantity", 80);
            listView1.Columns.Add("Cost", 80);
            listView1.Columns.Add("High_Margin", 80);
            listView1.Columns.Add("Low_Margin", 80);
            listView1.Columns.Add("Supplier", 120);
            listView1.Columns.Add("Supplier_Code", 80);
            listView1.Columns.Add("Category ", 100);
            listView1.Columns.Add("Date", 80);
            listView1.Columns.Add("Status", 80);
        }

        private void FillListView()
        {
            try
            {
                string sqlQuery = "SELECT * FROM invoice_details WHERE Status = 'DEACTIVATED'";
                SqlCommand command = new SqlCommand(sqlQuery, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["Barcode"].ToString());
                    item.SubItems.Add(reader["Model"].ToString());
                    item.SubItems.Add(reader["Item_Name"].ToString());
                    item.SubItems.Add(reader["Quantity"].ToString());
                    item.SubItems.Add(reader["Cost"].ToString());
                    item.SubItems.Add(reader["High_Margin"].ToString());
                    item.SubItems.Add(reader["Low_Margin"].ToString());
                    item.SubItems.Add(reader["Supplier"].ToString());
                    item.SubItems.Add(reader["Supplier_Code"].ToString());
                    item.SubItems.Add(reader["Category"].ToString());
                    item.SubItems.Add(reader["Date"].ToString());
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

        private void RefreshListView()
        {
            try
            {
                listView1.Items.Clear(); // Clear existing items

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM invoice_details WHERE Status = 'DEACTIVATED'", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListViewItem item = new ListViewItem(reader["Barcode"].ToString());
                            item.SubItems.Add(reader["Model"].ToString());
                            item.SubItems.Add(reader["Item_Name"].ToString());
                            item.SubItems.Add(reader["Quantity"].ToString());
                            item.SubItems.Add(reader["Cost"].ToString());
                            item.SubItems.Add(reader["High_Margin"].ToString());
                            item.SubItems.Add(reader["Low_Margin"].ToString());
                            item.SubItems.Add(reader["Supplier"].ToString());
                            item.SubItems.Add(reader["Supplier_Code"].ToString());
                            item.SubItems.Add(reader["Category"].ToString());
                            item.SubItems.Add(reader["Date"].ToString());
                            item.SubItems.Add(reader["Status"].ToString());

                            listView1.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void btnResetAll_Click(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void GRNRetrunForm_Load(object sender, EventArgs e)
        {
            FillListView();
            UpdateRowCount();
        }

        private void UpdateRowCount()
        {
            int rowCount = listView1.Items.Count;
            lblNoItem.Text = $"Number of Items: {rowCount.ToString()}";
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];

                string primaryKey = selectedItem.SubItems[1].Text;
                try
                {
                    SqlCommand cmd = new SqlCommand($"DELETE FROM invoice_details WHERE Model = '{primaryKey}'", conn);
                    int result = cmd.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("GRN Deleted successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        listView1.Items.Clear();
                        FillListView();
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
            else
            {
                MessageBox.Show("Please select an item to Delete", "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtGRNNumber_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtGRNNumber.Text.Trim();

            listView1.Items.Clear();

            try
            {
                string sqlQuery = $"SELECT * FROM invoice_details WHERE Model LIKE '%{searchText}%'";
                SqlCommand command = new SqlCommand(sqlQuery, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["Barcode"].ToString());
                    item.SubItems.Add(reader["Model"].ToString());
                    item.SubItems.Add(reader["Item_Name"].ToString());
                    item.SubItems.Add(reader["Quantity"].ToString());
                    item.SubItems.Add(reader["Cost"].ToString());
                    item.SubItems.Add(reader["High_Margin"].ToString());
                    item.SubItems.Add(reader["Low_Margin"].ToString());
                    item.SubItems.Add(reader["Supplier"].ToString());
                    item.SubItems.Add(reader["Supplier_Code"].ToString());
                    item.SubItems.Add(reader["Category"].ToString());
                    item.SubItems.Add(reader["Date"].ToString());
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

        private void btnResetAll_Click_1(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
            Hide();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            ContactUS us = new ContactUS();
            us.Show();
            Hide();
        }
    }
}
