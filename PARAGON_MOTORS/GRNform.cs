using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class GRNform : Form
    {
        private SqlConnection conn = null;
        public GRNform()
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
                using (SqlDataReader reader = command.ExecuteReader())
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
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GRNform_Load(object sender, EventArgs e)
        {
            FillListView();
            try
            {
                string query = "SELECT Category_Name FROM category";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string category = reader["Category_Name"].ToString();
                            cmbCategory.Items.Add(category);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshListView()
        {
            listView1.Items.Clear(); // Clear existing items
            FillListView(); // Refill ListView
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

        private void pictureboxHOME_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
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

        private void lblClose_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddQuantityForm newgrnew = new AddQuantityForm();
            newgrnew.Show();
            Hide();
        }

        string filePath = @"D:\supplierCode.txt";
        private int supplierCode = 2;
        private int LoadLastReceiptNumber()
        {
            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Read the number from the text file
                    supplierCode = Convert.ToInt32(File.ReadAllText(filePath));
                }
                else
                {
                    // File doesn't exist, create it and initialize with default receipt number
                    File.WriteAllText(filePath, supplierCode.ToString());
                    //MessageBox.Show("The file has been created with default receipt number.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return supplierCode;
        }

        private void SaveReceiptNumber()
        {
            try
            {
                // Write the incremented receipt number to the text file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(++supplierCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            
        }

        public void clearData()
        {
            txtBarcode.Clear();
            txtModel.Clear();
            txtItemName.Clear();
            txtQuantity.Clear();
            txtCost.Clear();
            txtHighMargin.Clear();
            txtLowMargin.Clear();
            txtSupplier.Clear();
            cmbCategory.Text = "Select Category";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clearData();
            listView1.Items.Clear();
            FillListView();
        }

        private void GRNform_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddCategoryForm add = new AddCategoryForm();
            add.Show();
            Hide();
        }

        public int supplier()
        {
            int supplierCode = 2;
            try
            {
                SqlCommand cmd1 = new SqlCommand("SELECT Supplier_Code FROM invoice_details WHERE Supplier = @val1", conn);
                cmd1.Parameters.AddWithValue("@val1", txtSupplier.Text);
                SqlDataReader reader = cmd1.ExecuteReader();

                if (reader.Read())
                {
                    supplierCode = Convert.ToInt32(reader["Supplier_Code"]);
                }
                else
                {
                    supplierCode = LoadLastReceiptNumber();
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return supplierCode;
        }


        private void btnADD_Click_1(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO invoice_details VALUES (@val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8, @val9, @val10, @val11, @val12)", conn);
                cmd.Parameters.AddWithValue("@val1", txtBarcode.Text);
                cmd.Parameters.AddWithValue("@val2", txtModel.Text);
                cmd.Parameters.AddWithValue("@val3", txtItemName.Text);
                cmd.Parameters.AddWithValue("@val4", Convert.ToInt32(txtQuantity.Text));
                cmd.Parameters.AddWithValue("@val5", Convert.ToInt32(txtCost.Text));
                cmd.Parameters.AddWithValue("@val6", Convert.ToInt32(txtHighMargin.Text));
                cmd.Parameters.AddWithValue("@val7", Convert.ToInt32(txtLowMargin.Text));
                cmd.Parameters.AddWithValue("@val8", txtSupplier.Text);
                cmd.Parameters.AddWithValue("@val9", supplier());
                cmd.Parameters.AddWithValue("@val10", cmbCategory.Text);
                cmd.Parameters.AddWithValue("@val11", currentDate);
                cmd.Parameters.AddWithValue("@val12", "ACTIVE");

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("GRN Added successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearData();
                    SaveReceiptNumber();
                    RefreshListView();
                }
                else
                {
                    MessageBox.Show("Something went wrong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
            Hide();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            ContactUS contact = new ContactUS();
            contact.Show();
            Hide();
        }

        private void lblClose_Click_1(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            Hide();
        }
    }
}