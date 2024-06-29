using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PARAGON_MOTORS
{
    public partial class GRNHistoryForm : Form
    {
        private SqlConnection conn = null;
        public GRNHistoryForm()
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void UpdateRowCount()
        {
            int rowCount = listView1.Items.Count;
            lblNoItem.Text = $"Number of Items: {rowCount.ToString()}";
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

        private void GRNHistoryForm_Load(object sender, EventArgs e)
        {
            FillListView();
            UpdateRowCount();
        }

        private void GRNHistoryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }

        private void dateTimePickerFROM_ValueChanged(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void dateTimePickerTO_ValueChanged(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            FillListView();
        }

        private void btnEXPORT_Click(object sender, EventArgs e)
        {
            //ExportToExcel();
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];

                string primaryKey = selectedItem.SubItems[1].Text;
                try
                {
                    SqlCommand cmd = new SqlCommand($"UPDATE invoice_details SET Status = @val2 WHERE Model = '{primaryKey}'", conn);
                    cmd.Parameters.AddWithValue("@val2", "ACTIVE");
                    int result = cmd.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("Activated successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Please select an item to Activate", "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];

                string primaryKey = selectedItem.SubItems[1].Text;
                try
                {
                    SqlCommand cmd = new SqlCommand($"UPDATE invoice_details SET Status = @val2 WHERE Model = '{primaryKey}'", conn);
                    cmd.Parameters.AddWithValue("@val2", "DEACTIVATED");
                    int result = cmd.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("Deactivated successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Please select an item to Deactivate", "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            FillListView();
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
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

        private void dateTimePickerFROM_ValueChanged_1(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void dateTimePickerTO_ValueChanged_1(object sender, EventArgs e)
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

        private void btnExport_Click_1(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void ExportToExcel()
        {
            try
            {
                // Create Excel Application
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = true;

                // Create a new Workbook
                Excel.Workbook workbook = excelApp.Workbooks.Add(System.Type.Missing);

                // Create a new Worksheet
                Excel.Worksheet worksheet = workbook.ActiveSheet;

                // Set column headers
                for (int i = 0; i < listView1.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = listView1.Columns[i].Text;
                }

                // Export ListView data to Excel
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    ListViewItem item = listView1.Items[i];
                    for (int j = 0; j < item.SubItems.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = item.SubItems[j].Text;
                    }
                }

                // Save the Workbook
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                workbook.SaveAs(desktopPath + "\\ListViewData.xlsx");

                // Clean up
                workbook.Close();
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                // Display message after data export
                MessageBox.Show("Data exported");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to Excel: " + ex.Message);
            }
        }

        private void label3_Click_1(object sender, EventArgs e)
        {
            LoginForm lg = new LoginForm();
            lg.Show();
            Hide();
        }
    }
}
