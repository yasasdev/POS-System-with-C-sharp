using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class InvoiceReturn : Form
    {
        private SqlConnection conn = null;
        public InvoiceReturn()
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

        private void InvoiceReturn_Load(object sender, EventArgs e)
        {
            FillListView();
        }

        public void RefreshListView()
        {
            listView1.Items.Clear();
            FillListView();
        }

        private void InvoiceReturn_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }

        private void lblCLOSE_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            Hide();
        }

        private void pictureboxHOME_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            Hide();
        }

        private void btnReturnInvoice_Click(object sender, EventArgs e)
        {           

            string receipt = txtReceiptNumber.Text;

            // Check if an item is selected in the ListView
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select the ITEM", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
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
                                //MessageBox.Show("!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Something went wrong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            MessageBox.Show("Sorry, model does not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please enter Model!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    SqlCommand command3 = new SqlCommand($"DELETE FROM salesreport_perm where Receipt_Number = {receipt}", conn);
                    int result3 = command3.ExecuteNonQuery();
                    if (result3 > 0)
                    {
                        MessageBox.Show("Returned Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        SqlCommand command2 = new SqlCommand($"DELETE FROM salesreport_temp where Receipt_Number = {receipt}", conn);
                        int result2 = command2.ExecuteNonQuery();
                        if (result2 > 0)
                        {
                            SqlCommand command1 = new SqlCommand($"DELETE FROM discount_table where Receipt_Number = {receipt}", conn);
                            int result1 = command1.ExecuteNonQuery();
                            if (result1 > 0)
                            {

                            }
                        }
                    }

                    PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                    printPreviewDialog.Document = printDocument1;
                    printPreviewDialog.ShowDialog();

                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Check if an item is selected in the ListView
            if (listView1.SelectedItems.Count > 0)
            {
                // Get the selected item for printing
                ListViewItem selectedItem = listView1.SelectedItems[0];

                // Create fonts
                Font boldFont = new Font("Arial", 9, FontStyle.Bold);
                Font normalFont = new Font("Arial", 8);
                Font centerFont = new Font("Arial", 8);

                // Define positions for drawing
                int x = 7;
                int y = 7;
                int lineOffset = 20;

                // Create a brush
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    // Measure the string size
                    SizeF size = e.Graphics.MeasureString("SAKURA MOBILE", boldFont);
                    float xCentered = (e.PageBounds.Width - size.Width) / 2; // Calculate the starting position to center the text
                    e.Graphics.DrawString("SAKURA MOBILE", boldFont, brush, xCentered, y);
                    y += lineOffset + 3; // You may adjust this offset as needed


                    // Draw address
                    size = e.Graphics.MeasureString("No 92,Anuradhapura Road,Thambuththegama", normalFont);
                    float xCentered1 = (e.PageBounds.Width - size.Width) / 3; // Calculate the starting position to center the text
                    e.Graphics.DrawString("No 92,Anuradhapura Road,Thambuththegama", normalFont, brush, xCentered1, y);
                    y += lineOffset + 3;

                    // Center the following lines
                    string telString = "Tel: 077 9999 261 | 077 5761 663";
                    size = e.Graphics.MeasureString(telString, centerFont);
                    float xCentered2 = (e.PageBounds.Width - size.Width) / 3; // Calculate the starting position to center the text
                    e.Graphics.DrawString(telString, centerFont, brush, xCentered2, y);
                    y += lineOffset + 3;

                    string adminString = "Owner: SAKURA MOBILE";
                    size = e.Graphics.MeasureString(adminString, centerFont);
                    float xLeftAligned4 = x; // Align to the left
                    e.Graphics.DrawString(adminString, centerFont, brush, xLeftAligned4, y);
                    y += lineOffset;

                    // Center the following lines
                    string line2 = "------------------------------------------------";
                    size = e.Graphics.MeasureString(line2, centerFont);
                    float xCentered5 = x + (e.PageBounds.Width - size.Width) / 3; // Calculate centered x-coordinate
                    e.Graphics.DrawString(line2, centerFont, brush, xCentered5, y - 30); // Adjusted the y-coordinate to bring line2 closer to the upper line
                    y += lineOffset - 20; // Decreased line offset

                    // Calculate the maximum length of each label
                    int maxItemNameLength = "ITEM NAME".Length;
                    int maxModelLength = "MODEL".Length;
                    int maxQuantityLength = "QUANTITY".Length;
                    int maxAmountLength = "AMOUNT".Length;

                    // Adjust the padding space based on the maximum length of each label
                    int itemNamePadding = maxItemNameLength + 30;
                    int modelPadding = maxModelLength + 45;
                    int quantityPadding = maxQuantityLength + 42;
                    int amountPadding = maxAmountLength + 40;

                    // Subtotal
                    string sub = "ITEM NAME".PadRight(itemNamePadding) + selectedItem.SubItems[2].Text;
                    SizeF sizeUserInfo2 = e.Graphics.MeasureString(sub, centerFont);
                    float xLeftAlignedUserInfo2 = x; // Align to the left
                    e.Graphics.DrawString(sub, normalFont, brush, xLeftAlignedUserInfo2, y);
                    y += lineOffset * 2; // Add extra space

                    // Discount
                    string discount = "Receipt No".PadRight(modelPadding) + selectedItem.SubItems[7].Text;
                    SizeF sizeUserInfo22 = e.Graphics.MeasureString(discount, centerFont);
                    float xLeftAlignedUserInfo22 = x; // Align to the left
                    e.Graphics.DrawString(discount, normalFont, brush, xLeftAlignedUserInfo22, y - 17);
                    y += lineOffset - 17; // Add extra space

                    // Net total
                    string netTotal = "QUANTITY".PadRight(quantityPadding) + txtQuantity.Text;
                    SizeF sizeUserInfo24 = e.Graphics.MeasureString(netTotal, centerFont);
                    float xLeftAlignedUserInfo24 = x; // Align to the left
                    e.Graphics.DrawString(netTotal, normalFont, brush, xLeftAlignedUserInfo24, y);
                    y += lineOffset * 2; // Add extra space

                    // Paid amount
                    string paidAmount = "AMOUNT".PadRight(amountPadding) + txtAmount.Text;
                    SizeF sizeUserInfo23 = e.Graphics.MeasureString(paidAmount, centerFont);
                    float xLeftAlignedUserInfo23 = x; // Align to the left
                    e.Graphics.DrawString(paidAmount, normalFont, brush, xLeftAlignedUserInfo23, y - 16);
                    y += lineOffset - 16; // Add extra space


                    // Center the following lines
                    string line4 = "------------------------------------------------";
                    size = e.Graphics.MeasureString(line4, centerFont);
                    float xCentered7 = x + (e.PageBounds.Width - size.Width) / 3; // Calculate centered x-coordinate
                    e.Graphics.DrawString(line4, centerFont, brush, xCentered7, y - 15); // Adjusted the y-coordinate to bring line2 closer to the upper line
                    y += lineOffset - 15; // Decreased line offset

                    size = e.Graphics.MeasureString("THANK YOU, COME AGAIN!", normalFont);
                    float xCentered8 = x + (e.PageBounds.Width - size.Width) / 3; // Calculate centered x-coordinate
                    e.Graphics.DrawString("THANK YOU, COME AGAIN!", normalFont, brush, xCentered8, y);
                    y += lineOffset;

                    // Measure the string size
                    SizeF size10 = e.Graphics.MeasureString("SOFTWARE BY CODEZCOPE | 070-1941387", normalFont);
                    float xOffset1 = 8; // You can adjust this value as needed
                    float xCentered11 = (e.PageBounds.Width - size10.Width) / 4 - xOffset1;
                    e.Graphics.DrawString("SOFTWARE BY CODEZCOPE | 070-1941387", normalFont, brush, xCentered11, y);
                    y += lineOffset;

                }
            }
            else
            {

            }
        }
    }
}
