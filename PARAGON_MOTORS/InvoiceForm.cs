using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace PARAGON_MOTORS
{
    public partial class InvoiceForm : Form
    {

        private SqlConnection conn = null;

        //Variables declaration
        int updatedQuantity = 0;
        int userInput = 0;
        double tot = 0.00;
        int rate = 0;
        int finalDiscount = 0;
        String amountTobeDeduct = null;
        String userName = null;
        double totalRate = 0.00;
        public InvoiceForm()
        {
            InitializeComponent();
            UpdateRowCount();
            conn = DatabaseConnectivity.getConnection();

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateDateTime();
            InitializeListView();
        }



        private void InvoiceForm1_Load(object sender, EventArgs e)
        {
            rdbItemName.Checked = true;
            cmbDisRate.SelectedIndex = 0;
            txtTotalAmount.Text = "Total : Rs 0/=";
            txtDiscount.Text = "0";
            UpdateRowCount();

            try
            {
                FillListView();
                try
                {
                    string query = "SELECT First_Name FROM employee";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string firstName = reader["First_Name"].ToString();
                                cmbUsers.Items.Add(firstName);
                            }
                        }
                    }
                    // Select the first item if it exists
                    if (cmbUsers.Items.Count > 0)
                    {
                        cmbUsers.SelectedIndex = 0; // Select the first item
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            listView2.View = View.Details;
            listView2.GridLines = true;
            listView2.FullRowSelect = true;

            listView2.Columns.Add("Barcode", 100);
            listView2.Columns.Add("Model", 80);
            listView2.Columns.Add("Item_Name", 150);
            listView2.Columns.Add("Quantity", 80);
            listView2.Columns.Add("Selling_Price", 80);
            listView2.Columns.Add("Supplier", 120);
            listView2.Columns.Add("Category ", 100);
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
                    listView1.Items.Add(item);
                }
                reader.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
            Hide();
        }

        private void InvoiceForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchColumn = "";

            if (rdbModel.Checked)
            {
                searchColumn = "Model";
            }
            else if (rdbItemName.Checked)
            {
                searchColumn = "Item_Name";
            }
            string searchText = txtSearch.Text.Trim();

            listView1.Items.Clear();

            try
            {
                string sqlQuery = $"SELECT * FROM invoice_details WHERE {searchColumn} LIKE '%{searchText}%'";
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
                    listView1.Items.Add(item);
                }

                reader.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtRate.Text == "0.00")
                    {
                        MessageBox.Show("Please enter the amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string inputText = ShowInputDialog("Enter Quantity:");

                        if (!string.IsNullOrWhiteSpace(inputText))
                        {
                            userInput = Convert.ToInt32(inputText);

                            ListViewItem selectedItem = listView1.SelectedItems[0];

                            int availableQuantity = Convert.ToInt32(selectedItem.SubItems[3].Text);

                            if (userInput <= availableQuantity) // Check if entered quantity is less than or equal to available quantity
                            {
                                MoveSelectedItem(listView1, listView2, userInput);

                                double totalRate = CalculateTotalRate();
                                tot = totalRate * userInput;
                                txtTotalAmount.Text = "Total : Rs " + tot.ToString() + "/=";

                                updatedQuantity = availableQuantity - userInput;

                                try
                                {
                                    string selectedSize = selectedItem.SubItems[1].Text; // assuming Size is at index 1
                                    SqlCommand cmd = new SqlCommand($"UPDATE invoice_details SET Quantity = @val1 WHERE Model = '{selectedSize}'", conn);
                                    cmd.Parameters.AddWithValue("@val1", updatedQuantity);
                                    int result = cmd.ExecuteNonQuery();

                                    if (result == 1)
                                    {
                                    }
                                    else
                                    {
                                        MessageBox.Show("Something went wrong!");
                                    }
                                }
                                catch (Exception ee)
                                {
                                    MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Entered quantity exceeds the available quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private string ShowInputDialog(string prompt)
        {
            Form promptForm = new Form();
            promptForm.Width = 300;
            promptForm.Height = 150;
            promptForm.Text = prompt;
            promptForm.StartPosition = FormStartPosition.CenterScreen;

            Label lblPrompt = new Label() { Left = 50, Top = 20, Text = prompt };
            System.Windows.Forms.TextBox txtInput = new System.Windows.Forms.TextBox() { Left = 50, Top = 50, Width = 200 };
            System.Windows.Forms.Button btnOK = new System.Windows.Forms.Button() { Text = "OK", Left = 50, Width = 100, Top = 80 };
            System.Windows.Forms.Button btnCancel = new System.Windows.Forms.Button() { Text = "Cancel", Left = 160, Width = 100, Top = 80 };

            btnOK.Click += (sender, e) => { promptForm.Close(); };
            btnCancel.Click += (sender, e) => { txtInput.Text = ""; promptForm.Close(); };

            txtInput.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnOK.PerformClick();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    promptForm.Close();
                }
            };

            txtInput.KeyPress += (sender, e) =>
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                    MessageBox.Show("Only numbers can be entered", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            promptForm.Controls.Add(lblPrompt);
            promptForm.Controls.Add(txtInput);
            promptForm.Controls.Add(btnOK);
            promptForm.Controls.Add(btnCancel);

            promptForm.ShowDialog();

            return txtInput.Text;
        }

        private void MoveSelectedItem(System.Windows.Forms.ListView sourceListView, System.Windows.Forms.ListView destinationListView, double quantity)
        {
            if (sourceListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = (ListViewItem)sourceListView.SelectedItems[0].Clone();

                // Set the Quantity and Rate values in listView2
                selectedItem.SubItems[2].Text = txtItem.Text; // Set Description
                selectedItem.SubItems[3].Text = quantity.ToString(); // Assuming Quantity column is at index 4
                selectedItem.SubItems[4].Text = txtRate.Text;

                // Edit specific indices for values to be inserted in listView2
                if (sourceListView == listView1)
                {
                    // Set value of index 5 of listView1 to index 4 of listView2
                    selectedItem.SubItems[4].Text = txtRate.Text;
                    // Set value of index 7 of listView1 to index 5 of listView2
                    selectedItem.SubItems[5].Text = sourceListView.SelectedItems[0].SubItems[7].Text;
                    // Set value of index 9 of listView1 to index 6 of listView2
                    selectedItem.SubItems[6].Text = sourceListView.SelectedItems[0].SubItems[9].Text;
                }

                destinationListView.Items.Add(selectedItem);
                UpdateRowCount();

                // Retrieve the primary key (Size) of the selected item
                string primaryKey = selectedItem.SubItems[1].Text; // Assuming Size is at index 1

                // Update the quantity value in listView1 based on the primary key
                UpdateQuantityInListView1(primaryKey, quantity);
            }
        }


        private void UpdateQuantityInListView1(string primaryKey, double quantity)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[1].Text == primaryKey) // Assuming Size is at index 1
                {
                    int currentQuantity = int.Parse(item.SubItems[3].Text); // Assuming Quantity is at index 4
                    item.SubItems[3].Text = (currentQuantity - quantity).ToString();

                    // Update the database with the new quantity value
                    UpdateQuantityInDatabase(primaryKey, currentQuantity - quantity);
                    break;
                }
            }
        }

        private void UpdateQuantityInDatabase(string primaryKey, double newQuantity)
        {
            try
            {
                SqlCommand cmd = new SqlCommand($"UPDATE invoice_details SET Quantity = @val1 WHERE Model = '{primaryKey}'", conn);
                cmd.Parameters.AddWithValue("@val1", newQuantity);
                int result = cmd.ExecuteNonQuery();

                if (result != 1)
                {
                    MessageBox.Show("Something went wrong!");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private double CalculateTotalRate()
        {
            double totalRate = 0;
            foreach (ListViewItem item in listView2.Items)
            {
                // Assuming rate is stored in the 5 subitem (index 4)
                double rate;
                if (double.TryParse(item.SubItems[4].Text, out rate))
                {
                    totalRate += rate;
                }
            }
            return totalRate;
        }

        private ListViewItem FindItemBySize(string size)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[1].Text == size)
                {
                    return item;
                }
            }
            return null;
        }

        private void RemoveSelectedItem(System.Windows.Forms.ListView listView)
        {
            if (listView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView.SelectedItems[0];

                // Deduct the rate of the removed item from totalRate
                double rateToRemove = Convert.ToDouble(selectedItem.SubItems[4].Text);
                totalRate = CalculateTotalRate();
                tot -= rateToRemove;
                txtTotalAmount.Text = "Total : Rs " + tot.ToString() + "/=";

                // Update the totalRate variable
                txtTotalAmount.Text = "Total : Rs " + tot.ToString() + "/=";
                txtSubTotal.Text = tot.ToString();

                // Remove the selected item from the listView
                listView.Items.Remove(selectedItem);
                UpdateRowCount();

                // Update listView1 immediately
                UpdateListView1(selectedItem);
            }
        }

        private void UpdateListView1(ListViewItem removedItem)
        {
            ListViewItem matchingItem = FindItemBySize(removedItem.SubItems[3].Text); // Assuming Size is in the fourth column (index 3)

            if (matchingItem != null)
            {
                int originalQuantity = int.Parse(matchingItem.SubItems[3].Text);
                matchingItem.SubItems[3].Text = (originalQuantity + userInput).ToString();

                // Update the database with the new quantity value
                UpdateQuantityInDatabase(matchingItem.SubItems[1].Text, originalQuantity + userInput);

                // Clear and reload entire data into listView1
                listView1.Items.Clear();
                FillListView();
            }
        }

        private DateTime lastEnterKeyPress = DateTime.MinValue;
        private const int doubleEnterThreshold = 500; // Adjust this threshold as needed

        private void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if (rdbModel.Checked == true)
                {
                    PassDataToTextBoxes(listView1.SelectedItems[0]);
                }
                else if (rdbItemName.Checked == true)
                {
                    PassDataToTextBoxes(listView1.SelectedItems[0]);
                }
            }
        }

        private void PassDataToTextBoxes(ListViewItem selectedItem)
        {
            if (rdbModel.Checked == true)
            {
                txtItem.Text = selectedItem.SubItems[2].Text;
            }
            else if (rdbItemName.Checked == true)
            {
                txtItem.Text = selectedItem.SubItems[2].Text;
            }
            txtItem.Text = selectedItem.SubItems[2].Text;

            txtRate.Text = selectedItem.SubItems[5].Text;

            txtAvailableQty.Text = selectedItem.SubItems[3].Text;
        }

        private void listView2_KeyDown_1(object sender, KeyEventArgs e)
        {
            UpdateRowCount();
            if (e.KeyCode == Keys.Delete)
            {
                if (listView2.SelectedItems.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to remove the item?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        ListViewItem selectedItem = listView2.SelectedItems[0];
                        userInput = int.Parse(selectedItem.SubItems[3].Text); // Assuming Quantity is in the fifth column (index 4)

                        RemoveSelectedItem(listView2);


                        // Add userInput back to listView1
                        ListViewItem originalItem = FindItemBySize(selectedItem.SubItems[1].Text); // Assuming Size is in the fourth column (index 3)
                        if (originalItem != null)
                        {
                            int originalQuantity = int.Parse(originalItem.SubItems[3].Text);
                            originalItem.SubItems[3].Text = (originalQuantity + userInput).ToString();
                            try
                            {
                                string selectedSize = originalItem.SubItems[1].Text;
                                SqlCommand cmd = new SqlCommand($"UPDATE invoice_details SET Quantity = @val1 WHERE Model = '{selectedSize}'", conn);
                                cmd.Parameters.AddWithValue("@val1", originalQuantity + userInput);
                                int result1 = cmd.ExecuteNonQuery();
                                if (result1 == 1)
                                {
                                }
                                else
                                {
                                    MessageBox.Show("Something went wrong!");
                                }
                            }
                            catch (Exception ee)
                            {
                                MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private void txtItem_KeyDown_1(object sender, KeyEventArgs e)
        {
            string item = txtItem.Text;
            if (e.KeyCode == Keys.Enter)
            {
                if (txtRate.Text == "0.00")
                {
                    MessageBox.Show("Please enter the amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmbDisRate_TextChanged_1(object sender, EventArgs e)
        {
            // Get the selected value from the ComboBox
            string selectedValue = cmbDisRate.SelectedItem.ToString();

            // Remove the "%" mark from the selected value
            String modifiedValue = selectedValue.Replace("%", "");
            int percentage = Convert.ToInt32(modifiedValue);

            finalDiscount = Convert.ToInt32((tot * percentage) / 100);

            // Set the modified value to the TextBox or use it as needed
            txtDiscount.Text = finalDiscount.ToString();
        }

        private void txtTotalAmount_TextChanged_1(object sender, EventArgs e)
        {
            txtSubTotal.Text = tot.ToString();
            txtNetTotal.Text = tot.ToString();
        }

        int netTotal = 0;
        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            netTotal = Convert.ToInt32(tot - finalDiscount);
            txtNetTotal.Text = netTotal.ToString();
        }

        private void txtPaidAmount_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtPaidAmount.Text.Trim() == "")
                    {
                        MessageBox.Show("Please enter Paid Amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        double remainder = Convert.ToDouble(txtPaidAmount.Text);
                        double amountTobeDeduct1 = Convert.ToDouble(amountTobeDeduct);
                        double finalRemainder = remainder - amountTobeDeduct1;
                        txtCustomerBalance.Text = finalRemainder.ToString();
                    }
                }

                if (e.KeyCode == Keys.Enter)
                {
                    if (txtPaidAmount.Text.Trim() == "")
                    {
                        MessageBox.Show("Please enter Paid Amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        double netTot = Convert.ToDouble(txtNetTotal.Text.Trim());
                        double userAmount = Convert.ToDouble(txtPaidAmount.Text.Trim());
                        double remainder = userAmount - netTot;

                        txtCustomerBalance.Text = remainder.ToString();
                    }
                }

                // Check for Enter key press
                if (e.KeyCode == Keys.Enter)
                {
                    // Check if the Enter key was pressed twice in quick succession
                    if (e.Modifiers == Keys.None && (DateTime.Now - lastEnterKeyPress).TotalMilliseconds < doubleEnterThreshold)
                    {
                        // Perform the desired action for a double Enter key press
                        if (cmbUsers.Text == "Select a User")
                        {
                            MessageBox.Show("Pleaase select a user before billing.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if (cmbUsers.Text.Trim() == "")
                        {
                            MessageBox.Show("Pleaase select a user before billing.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        // Reset the timer
                        lastEnterKeyPress = DateTime.MinValue;
                    }
                    else
                    {
                        // Update the timestamp for the last Enter key press
                        lastEnterKeyPress = DateTime.Now;
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }


        double rateTotal = 0;
        //double final = 0.00;
        private void txtRate_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string inputText = ShowInputDialog("Enter Quantity:");

                    if (!string.IsNullOrWhiteSpace(inputText))
                    {
                        int userInput = Convert.ToInt32(inputText);
                        MoveSelectedItem(listView1, listView2, userInput);

                        rateTotal = Convert.ToDouble(txtRate.Text);
                        double final = rateTotal * userInput;

                        if (tot != 0)
                        {
                            tot += final;
                            txtTotalAmount.Text = "Total : Rs " + tot.ToString() + "/=";
                        }
                        else
                        {
                            tot += final;
                            txtTotalAmount.Text = "Total : Rs " + tot.ToString() + "/=";
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        public int difference()
        {
            int difference = 0;

            difference = Convert.ToInt32(txtSubTotal.Text) - Convert.ToInt32(txtNetTotal.Text);

            return difference;
        }

        private void txtNetTotal_TextChanged(object sender, EventArgs e)
        {
            txtLastBill.Text = "Final Bill : Rs " + txtNetTotal.Text + "/=";
        }

        private void UpdateRowCount()
        {
            int rowCount = listView2.Items.Count;
            lblNumberItem.Text = $"Number of Items: {rowCount.ToString()}";
        }

        string receiptNumber1 = "";

        public string getReceiptNumber()
        {
            string receiptNumber = "";
            try
            {
                // Select the last invoid value by ordering in descending order and getting the top 1 record
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 invoid FROM invo_id ORDER BY invoid DESC", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    receiptNumber = reader["invoid"].ToString();

                    // Increment the receipt number
                    int incrementedReceiptNumber = int.Parse(receiptNumber) + 1;

                    // Store the incremented receipt number with leading zeros
                    receiptNumber1 = incrementedReceiptNumber.ToString("D6");
                }
                else
                {
                    MessageBox.Show("Sorry, receipt number does not exist!", "Error", MessageBoxButtons.OK);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return receiptNumber;
        }


        public void setReceiptNumber()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO invo_id VALUES (@val1)", conn);
                cmd.Parameters.AddWithValue("@val1", receiptNumber1);
                int result = cmd.ExecuteNonQuery();

                if (result == 1)
                {
                    //MessageBox.Show("Data sent successfully!");
                }
                else
                {
                    //MessageBox.Show("Something went wrong!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Create fonts
            Font boldFont = new Font("Times New Roman", 15, FontStyle.Bold);
            Font normalFont = new Font("Times New Roman", 10);
            Font centerFont = new Font("Times New Roman", 10);

            // Define positions for drawing
            int x = 7;
            int y = 7;
            int lineOffset = 20;

            // Create a brush
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                String user = cmbUsers.Text;
                // Measure the string size
                SizeF size = e.Graphics.MeasureString("SAKURA MOBILE", boldFont);
                float xCentered = (e.PageBounds.Width - size.Width) / 2; // Calculate the starting position to center the text
                e.Graphics.DrawString("SAKURA MOBILE", boldFont, brush, xCentered, y);
                y += lineOffset + 3; // You may adjust this offset as needed


                // Draw address

                string addressLine1 = "No 92,Anuradhapura Road,";
                string addressLine2 = "Thambuththegama";


                size = e.Graphics.MeasureString(addressLine1, normalFont);
                float xCentered1 = (e.PageBounds.Width - size.Width) / 2; // Calculate the starting position to center the text
                e.Graphics.DrawString(addressLine1, normalFont, brush, xCentered1, y);
                y += lineOffset + 3;

                size = e.Graphics.MeasureString(addressLine2, normalFont);
                float xCentered11 = (e.PageBounds.Width - size.Width) / 2; // Calculate the starting position to center the text
                e.Graphics.DrawString(addressLine2, normalFont, brush, xCentered11, y);
                y += lineOffset + 3;

                // Center the following lines
                string telString = "Tel: 077 9999 261 | 077 5761 663";
                size = e.Graphics.MeasureString(telString, centerFont);
                float xCentered2 = (e.PageBounds.Width - size.Width) / 2; // Calculate the starting position to center the text
                e.Graphics.DrawString(telString, centerFont, brush, xCentered2, y);
                y += lineOffset + 3;

                // Center the following lines
                string line = "------------------------------------------------";
                size = e.Graphics.MeasureString(line, centerFont);
                float xCentered3 = x + (e.PageBounds.Width - size.Width) / 3; // Calculate centered x-coordinate
                e.Graphics.DrawString(line, centerFont, brush, xCentered3, y);
                y += lineOffset;

                string adminString = "Owner: Sakura Mobile | Receipt No: " + getReceiptNumber();
                size = e.Graphics.MeasureString(adminString, centerFont);
                float xLeftAligned4 = x; // Align to the left
                e.Graphics.DrawString(adminString, centerFont, brush, xLeftAligned4, y);
                y += lineOffset;

                string userInfoString = $"User: " + user;
                SizeF sizeUserInfo = e.Graphics.MeasureString(userInfoString, centerFont);
                float xLeftAlignedUserInfo = x; // Align to the left
                e.Graphics.DrawString(userInfoString, centerFont, brush, xLeftAlignedUserInfo, y);
                y += lineOffset * 2; // Add extra space

                DateTime currentTime99 = DateTime.Now;
                string userInfoString99 = currentTime99.ToString("hh:mm | dd/MM/yyyy");
                SizeF sizeUserInfo99 = e.Graphics.MeasureString(userInfoString99, centerFont);
                float xLeftAlignedUserInfo99 = x; // Align to the left
                e.Graphics.DrawString(userInfoString99, centerFont, brush, xLeftAlignedUserInfo99, y - 17);
                y += lineOffset * 2; // Add extra space

                // Center the following lines
                string line2 = "------------------------------------------------";
                size = e.Graphics.MeasureString(line2, centerFont);
                float xCentered5 = x + (e.PageBounds.Width - size.Width) / 3; // Calculate centered x-coordinate
                e.Graphics.DrawString(line2, centerFont, brush, xCentered5, y - 30); // Adjusted the y-coordinate to bring line2 closer to the upper line
                y += lineOffset - 20; // Decreased line offset

                // Item list
                string itemsHeader = "Item".PadRight(20) + "Quantity".PadRight(25) + "Price";
                SizeF sizeUserInfo1 = e.Graphics.MeasureString(itemsHeader, centerFont);
                float xLeftAlignedUserInfo1 = x; // Align to the left
                e.Graphics.DrawString(itemsHeader, centerFont, brush, xLeftAlignedUserInfo1, y);
                y += lineOffset * 2; // Add extra space

                // Create an array to store items, quantities, and rates from the 3rd, 4th, and 5th index of listView2
                object[,] itemsArray = new object[listView2.Items.Count, 3];

                // Populate the array with items, quantities, and rates from the 3rd, 4th, and 5th index of listView2
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    itemsArray[i, 0] = listView2.Items[i].SubItems[2].Text; // Description
                    itemsArray[i, 1] = listView2.Items[i].SubItems[3].Text; // Quantity
                    itemsArray[i, 2] = listView2.Items[i].SubItems[4].Text; // Rate
                }

                // Define padding lengths for each component
                int itemNamePadding = 22; // Adjust according to the maximum length of your item name
                int quantityPadding = 15;
                int pricePadding = 12;

                // Calculate the starting positions for each component
                float itemNameStartX = xLeftAlignedUserInfo1; // Start at the same position as the "Item" heading

                // Define a monospaced font for consistent width
                Font monospacedFont = new Font("Courier New", 8); // Change size and font as needed

                // Define the gap between items
                int itemVerticalSpacing = 5; // Adjust as needed

                for (int i = 0; i < itemsArray.GetLength(0); i++)
                {
                    // Pad the strings to ensure fixed positions
                    string itemName = itemsArray[i, 0].ToString().PadRight(itemNamePadding);
                    string quantity = itemsArray[i, 1].ToString().PadLeft(quantityPadding);
                    string price = itemsArray[i, 2].ToString().PadLeft(pricePadding);

                    // Draw each component separately
                    e.Graphics.DrawString(itemName, monospacedFont, Brushes.Black, itemNameStartX, y);

                    // Concatenate quantity and price into a single string
                    string quantityAndPrice = $"{quantity} {price}";

                    // Draw the concatenated string
                    e.Graphics.DrawString(quantityAndPrice, monospacedFont, Brushes.Black, itemNameStartX + itemNamePadding, y + monospacedFont.Height);

                    // Increase y position for the next item
                    y += monospacedFont.Height * 2 + itemVerticalSpacing; // Add extra space
                }

                // Center the following lines
                string line3 = "------------------------------------------------";
                size = e.Graphics.MeasureString(line3, centerFont);
                float xCentered6 = x + (e.PageBounds.Width - size.Width) / 3; // Calculate centered x-coordinate
                e.Graphics.DrawString(line3, centerFont, brush, xCentered6, y - 9); // Adjusted the y-coordinate to bring line2 closer to the upper line
                y += lineOffset - 9; // Decreased line offset

                // Subtotal
                int subTotalPadding = 24;
                string sub = "SUB TOTAL" + txtSubTotal.Text.ToString().PadLeft(subTotalPadding);
                SizeF sizeUserInfo2 = e.Graphics.MeasureString(sub, centerFont);
                float xLeftAlignedUserInfo2 = x; // Align to the left
                e.Graphics.DrawString(sub, monospacedFont, brush, xLeftAlignedUserInfo2, y);
                y += lineOffset * 2; // Add extra space

                // Discount
                int discountPadding = 25;
                string discount = "DISCOUNT" + txtDiscount.Text.ToString().PadLeft(discountPadding);
                SizeF sizeUserInfo22 = e.Graphics.MeasureString(discount, centerFont);
                float xLeftAlignedUserInfo22 = x; // Align to the left
                e.Graphics.DrawString(discount, monospacedFont, brush, xLeftAlignedUserInfo22, y - 17);
                y += lineOffset - 17; // Add extra space

                // Net total
                int netTotalPadding = 24;
                string netTotal = "NET TOTAL" + txtNetTotal.Text.ToString().PadLeft(netTotalPadding);
                SizeF sizeUserInfo24 = e.Graphics.MeasureString(netTotal, centerFont);
                float xLeftAlignedUserInfo24 = x; // Align to the left
                e.Graphics.DrawString(netTotal, monospacedFont, brush, xLeftAlignedUserInfo24, y);
                y += lineOffset * 2; // Add extra space

                // Paid amount
                int paidAmountPadding = 22;
                string paidAmount = "PAID AMOUNT" + txtPaidAmount.Text.ToString().PadLeft(paidAmountPadding);
                SizeF sizeUserInfo23 = e.Graphics.MeasureString(paidAmount, centerFont);
                float xLeftAlignedUserInfo23 = x; // Align to the left
                e.Graphics.DrawString(paidAmount, monospacedFont, brush, xLeftAlignedUserInfo23, y - 16);
                y += lineOffset - 16; // Add extra space

                // Balance
                int balancePadding = 26;
                string balance = "BALANCE" + txtCustomerBalance.Text.ToString().PadLeft(balancePadding);
                SizeF sizeUserInfo25 = e.Graphics.MeasureString(balance, centerFont);
                float xLeftAlignedUserInfo25 = x; // Align to the left
                e.Graphics.DrawString(balance, monospacedFont, brush, xLeftAlignedUserInfo25, y);
                y += lineOffset * 2; // Add extra space

                // Center the following lines
                string line4 = "------------------------------------------------";
                size = e.Graphics.MeasureString(line4, centerFont);
                float xCentered7 = x + (e.PageBounds.Width - size.Width) / 3; // Calculate centered x-coordinate
                e.Graphics.DrawString(line4, centerFont, brush, xCentered7, y - 15); // Adjusted the y-coordinate to bring line2 closer to the upper line
                y += lineOffset - 15; // Decreased line offset

                size = e.Graphics.MeasureString("THANK YOU, COME AGAIN!", normalFont);
                float xCentered8 = x + (e.PageBounds.Width - size.Width) / 2; // Calculate centered x-coordinate
                e.Graphics.DrawString("THANK YOU, COME AGAIN!", monospacedFont, brush, xCentered8, y);
                y += lineOffset;

                SizeF size9 = e.Graphics.MeasureString(" Return is possible within 3 days", monospacedFont);
                float xOffset = 12; // Adjust this value as needed to move the line to the left
                e.Graphics.DrawString(" Return is possible within 3 days", monospacedFont, brush, xOffset, y);
                y += lineOffset + 3;

                // Measure the string size
                SizeF size10 = e.Graphics.MeasureString("SOFTWARE BY CODEZCOPE | 070-1941387", monospacedFont);
                float xOffset1 = 8; // You can adjust this value as needed
                float xCentered12 = (e.PageBounds.Width - size10.Width) / 4 - xOffset1;
                e.Graphics.DrawString("  SOFTWARE BY CODEZCOPE | 070-1941387", monospacedFont, brush, xCentered12, y);
                y += lineOffset + 15; // Add space between lines

                // Center the following lines
                string line5 = "------------------------------------------------";
                SizeF size5 = e.Graphics.MeasureString(line5, centerFont);
                float xCentered44 = x + (e.PageBounds.Width - size5.Width) / 3; // Calculate centered x-coordinate
                e.Graphics.DrawString(line5, centerFont, brush, xCentered44, y - 15); // Adjusted the y-coordinate to bring line2 closer to the upper line
                y += lineOffset - 15; // Decreased line offset

            }
        }

        private void cmbUsers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Modifiers == Keys.None && (DateTime.Now - lastEnterKeyPress).TotalMilliseconds < doubleEnterThreshold)
                {
                    PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                    printPreviewDialog.Document = printDocument1;

                    if (txtPaidAmount.Text.Trim() == "")
                    {
                        MessageBox.Show("Please enter paid amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        printPreviewDialog.ShowDialog();
                        insertData();
                        clearData();
                    }
                    // Reset the timer
                    lastEnterKeyPress = DateTime.MinValue;
                }
                else
                {
                    // Update the timestamp for the last Enter key press
                    lastEnterKeyPress = DateTime.Now;
                }

            }
        }

        private void clearData()
        {
            txtItem.Clear();
            txtRate.Clear();
            txtAvailableQty.Clear();
            cmbDisRate.SelectedIndex = 0;
            txtSubTotal.Clear();
            txtDiscount.Clear();
            txtNetTotal.Clear();
            txtPaidAmount.Clear();
            rdbModel.Checked = true;
            cmbUsers.SelectedItem = "Select a User";
            listView2.Items.Clear();
            txtSearch.Focus();
            tot = 0;
            txtTotalAmount.Text = "Total : Rs 0/=";
            txtCustomerBalance.Clear();
            txtFinalDiscount.Clear();
            txtDiscount.Text = "0";
        }

        private void insertData()
        {
            string paid = txtPaidAmount.Text;
            DateTime currentDate = DateTime.Now;
            // Check if there is an item at index 1 in listView2
            if (listView2.Items.Count > 0)
            {
                try
                {
                    foreach (ListViewItem item in listView2.Items)
                    {
                        string Barcode = "NULL";
                        string Model = item.SubItems[1].Text;
                        string ItemName = item.SubItems[2].Text;
                        int Quantity = Convert.ToInt32(item.SubItems[3].Text);
                        int sellingPrice = Convert.ToInt32(item.SubItems[4].Text);
                        string Supplier = item.SubItems[7].Text;
                        string Category = item.SubItems[9].Text;

                        SqlCommand cmd = new SqlCommand("INSERT INTO salesreport_temp VALUES (@val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8, @val9)", conn);
                        cmd.Parameters.AddWithValue("@val1", Barcode);
                        cmd.Parameters.AddWithValue("@val2", Model);
                        cmd.Parameters.AddWithValue("@val3", ItemName);
                        cmd.Parameters.AddWithValue("@val4", Quantity);
                        cmd.Parameters.AddWithValue("@val5", sellingPrice);
                        cmd.Parameters.AddWithValue("@val6", Supplier);
                        cmd.Parameters.AddWithValue("@val7", Category);
                        cmd.Parameters.AddWithValue("@val8", getReceiptNumber());
                        cmd.Parameters.AddWithValue("@val9", currentDate);

                        int result = cmd.ExecuteNonQuery();

                        if (result == 1)
                        {
                            //MessageBox.Show("Data sent successfully!");
                        }
                        else
                        {
                            //MessageBox.Show("Something went wrong while inserting data!");
                        }

                        SqlCommand cmd5 = new SqlCommand("INSERT INTO salesreport_perm VALUES (@val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8, @val9)", conn);
                        cmd5.Parameters.AddWithValue("@val1", Barcode);
                        cmd5.Parameters.AddWithValue("@val2", Model);
                        cmd5.Parameters.AddWithValue("@val3", ItemName);
                        cmd5.Parameters.AddWithValue("@val4", Quantity);
                        cmd5.Parameters.AddWithValue("@val5", sellingPrice);
                        cmd5.Parameters.AddWithValue("@val6", Supplier);
                        cmd5.Parameters.AddWithValue("@val7", Category);
                        cmd5.Parameters.AddWithValue("@val8", getReceiptNumber());
                        cmd5.Parameters.AddWithValue("@val9", currentDate);

                        int result5 = cmd5.ExecuteNonQuery();

                        if (result5 == 1)
                        {
                            //MessageBox.Show("Data sent successfully!");
                        }
                        else
                        {
                            //MessageBox.Show("Something went wrong while inserting data!");
                        }
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            SqlCommand cmd1 = new SqlCommand("INSERT INTO discount_table VALUES (@val1, @val2)", conn);
            cmd1.Parameters.AddWithValue("@val1", Convert.ToInt32(txtNetTotal.Text));
            cmd1.Parameters.AddWithValue("@val2", getReceiptNumber());
            int result1 = cmd1.ExecuteNonQuery();

            if (result1 == 1)
            {
                //MessageBox.Show("Discount sent successfully!");
                setReceiptNumber();
            }
            else
            {
                MessageBox.Show("Something went wrong while inserting data!");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
            Hide();
        }

        private void btnReprint_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                printPreviewDialog.Document = printDocument2;
                printPreviewDialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select an item to reprint.", "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RefreshListView1()
        {
            if (chkSearch.Checked)
            {
                try
                {
                    listView2.Items.Clear(); // Clear existing items



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
                                listView2.Items.Add(item);
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

        private void chkSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSearch.Checked == true)
            {
                dateTimePickerFROM.Enabled = true;
                dateTimePickerTO.Enabled = true;
                RefreshListView1();
            }
            else
            {
                dateTimePickerFROM.Enabled = false;
                dateTimePickerTO.Enabled = false;
            }
        }

        private void dateTimePickerFROM_ValueChanged(object sender, EventArgs e)
        {
            RefreshListView1();
        }

        private void dateTimePickerTO_ValueChanged(object sender, EventArgs e)
        {
            RefreshListView1();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }

        private void printDocument2_PrintPage_1(object sender, PrintPageEventArgs e)
        {
            // Check if an item is selected in the ListView
            if (listView2.SelectedItems.Count > 0)
            {
                // Get the selected item for printing
                ListViewItem selectedItem = listView2.SelectedItems[0];

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
                    String user = cmbUsers.Text;
                    // Measure the string size
                    SizeF size = e.Graphics.MeasureString("SAKURA MOBILE", boldFont);
                    float xCentered = (e.PageBounds.Width - size.Width) / 3; // Calculate the starting position to center the text
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

                    string adminString = "Owner: Sakura Mobile";
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

                    // Item list
                    string itemsHeader = "Item".PadRight(28) + "Quantity".PadRight(28) + "Price";
                    SizeF sizeUserInfo1 = e.Graphics.MeasureString(itemsHeader, centerFont);
                    float xLeftAlignedUserInfo1 = x; // Align to the left
                    e.Graphics.DrawString(itemsHeader, centerFont, brush, xLeftAlignedUserInfo1, y);
                    y += lineOffset * 2; // Add extra space

                    // Define padding lengths for each component
                    int itemNamePadding = 22; // Adjust according to the maximum length of your item name
                    int quantityPadding = 2;
                    int pricePadding = 5;

                    // Calculate the starting positions for each component
                    float itemNameStartX = xLeftAlignedUserInfo1; // Start at the same position as the "Item" heading
                    float quantityStartX = itemNameStartX + itemNamePadding; // Align quantity after the item name
                    float priceStartX = quantityStartX + quantityPadding; // Align price after the quantity

                    // Define a monospaced font for consistent width
                    Font monospacedFont = new Font("Courier New", 8); // Change size and font as needed

                    // Pad the strings to ensure fixed positions
                    string itemName = selectedItem.SubItems[2].Text.PadRight(itemNamePadding);
                    string quantity = selectedItem.SubItems[3].Text.PadLeft(quantityPadding);
                    string price = selectedItem.SubItems[4].Text.PadLeft(pricePadding);

                    string itemText = $"{itemName} x {quantity} {price}";

                    // Measure the string using the monospaced font
                    SizeF textSize = e.Graphics.MeasureString(itemText, monospacedFont);

                    // Draw the string with adjusted starting positions for each component
                    e.Graphics.DrawString(itemText, monospacedFont, Brushes.Black, itemNameStartX, y - 15);

                    y += Convert.ToInt16(textSize.Height + lineOffset - 15); // Add extra space

                    // Center the following lines
                    string line3 = "------------------------------------------------";
                    size = e.Graphics.MeasureString(line3, centerFont);
                    float xCentered6 = x + (e.PageBounds.Width - size.Width) / 3; // Calculate centered x-coordinate
                    e.Graphics.DrawString(line3, centerFont, brush, xCentered6, y - 15); // Adjusted the y-coordinate to bring line2 closer to the upper line
                    y += lineOffset - 15; // Decreased line offset

                    // Subtotal
                    int subTotalPadding = 24;
                    string sub = "SUB TOTAL" + selectedItem.SubItems[4].Text.PadLeft(subTotalPadding);
                    SizeF sizeUserInfo2 = e.Graphics.MeasureString(sub, centerFont);
                    float xLeftAlignedUserInfo2 = x; // Align to the left
                    e.Graphics.DrawString(sub, monospacedFont, brush, xLeftAlignedUserInfo2, y);
                    y += lineOffset * 2; // Add extra space

                    // Discount
                    int discountPadding = 25;
                    string discount = "DISCOUNT" + selectedItem.SubItems[5].Text.PadLeft(discountPadding);
                    SizeF sizeUserInfo22 = e.Graphics.MeasureString(discount, centerFont);
                    float xLeftAlignedUserInfo22 = x; // Align to the left
                    e.Graphics.DrawString(discount, monospacedFont, brush, xLeftAlignedUserInfo22, y - 17);
                    y += lineOffset - 17; // Add extra space

                    // Net total
                    int netTotalPadding = 24;
                    string netTotal = "NET TOTAL" + "0".PadLeft(netTotalPadding);
                    SizeF sizeUserInfo24 = e.Graphics.MeasureString(netTotal, centerFont);
                    float xLeftAlignedUserInfo24 = x; // Align to the left
                    e.Graphics.DrawString(netTotal, monospacedFont, brush, xLeftAlignedUserInfo24, y);
                    y += lineOffset * 2; // Add extra space

                    // Paid amount
                    int paidAmountPadding = 22;
                    string paidAmount = "PAID AMOUNT" + "0".PadLeft(paidAmountPadding);
                    SizeF sizeUserInfo23 = e.Graphics.MeasureString(paidAmount, centerFont);
                    float xLeftAlignedUserInfo23 = x; // Align to the left
                    e.Graphics.DrawString(paidAmount, monospacedFont, brush, xLeftAlignedUserInfo23, y - 16);
                    y += lineOffset - 16; // Add extra space

                    // Balance
                    int balancePadding = 26;
                    string balance = "BALANCE" + "0".PadLeft(balancePadding);
                    SizeF sizeUserInfo25 = e.Graphics.MeasureString(balance, centerFont);
                    float xLeftAlignedUserInfo25 = x; // Align to the left
                    e.Graphics.DrawString(balance, monospacedFont, brush, xLeftAlignedUserInfo25, y);
                    y += lineOffset * 2; // Add extra space

                    // Center the following lines
                    string line4 = "------------------------------------------------";
                    size = e.Graphics.MeasureString(line4, centerFont);
                    float xCentered7 = x + (e.PageBounds.Width - size.Width) / 3; // Calculate centered x-coordinate
                    e.Graphics.DrawString(line4, centerFont, brush, xCentered7, y - 15); // Adjusted the y-coordinate to bring line2 closer to the upper line
                    y += lineOffset - 15; // Decreased line offset

                    size = e.Graphics.MeasureString("THANK YOU, COME AGAIN!", normalFont);
                    float xCentered8 = x + (e.PageBounds.Width - size.Width) / 3; // Calculate centered x-coordinate
                    e.Graphics.DrawString("THANK YOU, COME AGAIN!", monospacedFont, brush, xCentered8, y);
                    y += lineOffset;

                    SizeF size9 = e.Graphics.MeasureString(" Return is possible within 7 days", monospacedFont);
                    float xOffset = 12; // Adjust this value as needed to move the line to the left
                    e.Graphics.DrawString(" Return is possible within 7 days", monospacedFont, brush, xOffset, y);
                    y += lineOffset + 3;

                    // Measure the string size
                    SizeF size10 = e.Graphics.MeasureString("SOFTWARE BY CODEZCOPE | 070-1941387", monospacedFont);
                    float xOffset1 = 8; // You can adjust this value as needed
                    float xCentered11 = (e.PageBounds.Width - size10.Width) / 4 - xOffset1;
                    e.Graphics.DrawString("SOFTWARE BY CODEZCOPE | 070-1941387", monospacedFont, brush, xCentered11, y);
                    y += lineOffset;

                }
            }
            else
            {

            }
        }

        private void txtFinalDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int finalDis = Convert.ToInt32(txtFinalDiscount.Text);


                int sub = Convert.ToInt32(txtSubTotal.Text);

                txtDiscount.Text = (sub - finalDis).ToString();
                txtNetTotal.Text = finalDis.ToString();
            }
        }
    }
}