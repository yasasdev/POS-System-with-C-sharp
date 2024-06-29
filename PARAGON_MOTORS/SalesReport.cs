using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PARAGON_MOTORS
{
    public partial class SalesReport : Form
    {
        private SqlConnection conn = null;
        public SalesReport()
        {
            InitializeComponent();
            conn = DatabaseConnectivity.getConnection();

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
            listView1.Columns.Add("Date", 80);
        }

        private void RefreshListView()
        {
            listView1.Items.Clear(); // Clear existing items
            FillListView(); // Refill ListView
        }

        private void ClearListView()
        {
            // Clear all items in listView1
            listView1.Items.Clear();
        }

        int discount = 0;
        private void SalesReport_Load(object sender, EventArgs e)
        {
            FillListView();
            discountUpdate();
        }

        public void discountUpdate()
        {
            string sqlQuery1 = "SELECT discount FROM discount_table";
            SqlCommand command1 = new SqlCommand(sqlQuery1, conn);
            SqlDataReader reader1 = command1.ExecuteReader();
            int totalDiscount = 0; // Initialize total discount variable
            while (reader1.Read())
            {
                if (!reader1.IsDBNull(reader1.GetOrdinal("discount")))
                {
                    // Retrieve the discount value from the reader
                    int discountValue = reader1.GetInt32(reader1.GetOrdinal("discount"));
                    totalDiscount += discountValue; // Accumulate discount values
                }
            }
            reader1.Close();
            lblTotalAmount.Text = "Total sales of the day: Rs " + totalDiscount.ToString() + "/=";
        }

        private void FillListView()
        {
            try
            {
                string sqlQuery = "SELECT * FROM salesreport_temp";
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

        private void SalesReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
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
                MessageBox.Show("Data exported successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to Excel: " + ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
            Hide();
        }

        private void btnExport_Click_1(object sender, EventArgs e)
        {
            ExportToExcel();
        }
    }
}