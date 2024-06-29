using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class StockForm : Form
    {
        private SqlConnection conn;
        public StockForm()
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
                string sqlQuery = "SELECT * FROM invoice_details";
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

        private void StockForm_Load(object sender, EventArgs e)
        {
            FillListView();
            UpdateRowCount();
        }

        private void StockForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }

        private void UpdateRowCount()
        {
            int rowCount = listView1.Items.Count;
            label8.Text = $"GRN Quantity: {rowCount.ToString()}";
        }

        private void RefreshListView()
        {
            if (chkSearch.Checked)
            {
                try
                {
                    listView1.Items.Clear(); // Clear existing items

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM invoice_details WHERE Date >= @val1 AND Date <= @val2", conn))
                    {
                        cmd.Parameters.AddWithValue("@val1", dateTimePickerFROM.Value.Date);
                        cmd.Parameters.AddWithValue("@val2", dateTimePickerTO.Value.Date.AddDays(1).AddSeconds(-1));

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
            else
            {
                listView1.Items.Clear(); // Clear existing items
            }
        }

        private void dateTimePickerFROM_ValueChanged(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void dateTimePickerTO_ValueChanged(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

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

        private void btnReset_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            FillListView();
        }

        private void btnAddQuantity_Click(object sender, EventArgs e)
        {
            AdministratorLOGIN loginForm = new AdministratorLOGIN();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                AddQuantityForm ad = new AddQuantityForm();
                ad.Show();
                Hide();
            }
        }
    }
}
