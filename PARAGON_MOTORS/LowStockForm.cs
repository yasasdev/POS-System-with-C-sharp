using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class LowStockForm : Form
    {
        private SqlConnection conn = null;
        public LowStockForm()
        {
            InitializeComponent();
            conn = DatabaseConnectivity.getConnection();
            InitializeListView();
        }

        private void btnAddQuantity_Click(object sender, EventArgs e)
        {
            AdministratorLOGIN loginForm = new AdministratorLOGIN();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                AddQuantityForm adq = new AddQuantityForm();
                adq.Show();
                Hide();
            }
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
                string sqlQuery = "SELECT * FROM invoice_details WHERE Quantity <= 5";
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

        private void LowStockForm_Load(object sender, EventArgs e)
        {
            FillListView();
        }

        private void LowStockForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }

        private void pictureboxHOME_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
            Hide();
        }

        private void lblCLOSE_Click(object sender, EventArgs e)
        {
            LoginForm lf = new LoginForm();
            lf.Show();
            Hide();
        }
    }
}
