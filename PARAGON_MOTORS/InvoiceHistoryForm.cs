using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PARAGON_MOTORS
{
    public partial class InvoiceHistoryForm : Form
    {
        private SqlConnection conn = null;
        public InvoiceHistoryForm()
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
            listView1.Columns.Add("Selling_Price", 80);
            listView1.Columns.Add("Supplier", 120);
            listView1.Columns.Add("Category ", 100);
            listView1.Columns.Add("Receipt_Number", 80);
            listView1.Columns.Add("Date ", 80);
        }

        private void FillListView()
        {
            try
            {
                string sqlQuery = "SELECT * FROM salesreport_perm";
                SqlCommand command = new SqlCommand(sqlQuery, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["Barcode"].ToString());
                    item.SubItems.Add(reader["Model"].ToString());
                    item.SubItems.Add(reader["Item_Name"].ToString());
                    item.SubItems.Add(reader["Quantity"].ToString());
                    item.SubItems.Add(reader["Selling_Price"].ToString());
                    item.SubItems.Add(reader["Supplier"].ToString());
                    item.SubItems.Add(reader["Category"].ToString());
                    item.SubItems.Add(reader["Receipt_Number"].ToString());
                    item.SubItems.Add(reader["Date"].ToString());

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

        private void chkSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSearch.Checked == true) {                
                dateTimePickerFROM.Enabled = true;
                dateTimePickerTO.Enabled = true;
                RefreshListView();
            }
            else
            {
                dateTimePickerFROM.Enabled = false;
                dateTimePickerTO.Enabled = false;
            }
        }

        private void RefreshListView()
        {
            if (chkSearch.Checked)
            {
                try
                {
                    listView1.Items.Clear();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM salesreport_perm WHERE Date >= @val1 AND Date <= @val2", conn))
                    {
                        cmd.Parameters.AddWithValue("@val1", dateTimePickerFROM.Value.Date);
                        cmd.Parameters.AddWithValue("@val2", dateTimePickerTO.Value.Date.AddDays(1).AddSeconds(-1));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Populate ListView with data from reader
                                ListViewItem item = new ListViewItem(reader["Barcode"].ToString());
                                item.SubItems.Add(reader["Model"].ToString());
                                item.SubItems.Add(reader["Item_Name"].ToString());
                                item.SubItems.Add(reader["Quantity"].ToString());
                                item.SubItems.Add(reader["Selling_Price"].ToString());
                                item.SubItems.Add(reader["Supplier"].ToString());
                                item.SubItems.Add(reader["Category"].ToString());
                                item.SubItems.Add(reader["Receipt_Number"].ToString());
                                item.SubItems.Add(reader["Date"].ToString());

                                listView1.Items.Add(item);
                            }
                        }
                    }
                    CalculateTotal();
                    UpdateRowCount();
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

        private void CalculateTotal()
        {
            int total = 0;

            // Loop through each item in the ListView
            foreach (ListViewItem item in listView1.Items)
            {
                // Get the value of the "Selling_Price" column
                int sellingPrice;
                if (int.TryParse(item.SubItems[4].Text, out sellingPrice))
                {
                    // Add the value to the total
                    total += sellingPrice;
                }
            }

            // Set the total value to the label
            lblTotal.Text = "Total: " + total.ToString();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtReceiptNumber.Text.Trim();

            listView1.Items.Clear();

            try
            {
                string sqlQuery = $"SELECT * FROM salesreport_perm WHERE Receipt_Number LIKE '%{searchText}%'";
                SqlCommand command = new SqlCommand(sqlQuery, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["Barcode"].ToString());
                    item.SubItems.Add(reader["Model"].ToString());
                    item.SubItems.Add(reader["Item_Name"].ToString());
                    item.SubItems.Add(reader["Quantity"].ToString());
                    item.SubItems.Add(reader["Selling_Price"].ToString());
                    item.SubItems.Add(reader["Supplier"].ToString());
                    item.SubItems.Add(reader["Category"].ToString());
                    item.SubItems.Add(reader["Receipt_Number"].ToString());
                    item.SubItems.Add(reader["Date"].ToString());

                    listView1.Items.Add(item);
                }

                reader.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InvoiceHistoryForm_Load(object sender, EventArgs e)
        {
            FillListView();
            UpdateRowCount();
            CalculateTotal();
        }

        private void UpdateRowCount()
        {
            int rowCount = listView1.Items.Count;
            label10.Text = $"Number of Items: {rowCount.ToString()}";
        }

        private void InvoiceHistoryForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            FillListView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnExport_Click(object sender, EventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to Excel: " + ex.Message);
            }
        }
    }
}