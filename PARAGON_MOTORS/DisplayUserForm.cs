using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class DisplayUserForm : Form
    {
        private SqlConnection conn = null;
        public DisplayUserForm()
        {
            InitializeComponent();
            conn = DatabaseConnectivity.getConnection();

            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            listView1.Columns.Add("Title", 50);
            listView1.Columns.Add("First_Name", 100);
            listView1.Columns.Add("Last_Name", 100);
            listView1.Columns.Add("NIC", 100);
            listView1.Columns.Add("DOB", 80);
            listView1.Columns.Add("Gender", 80);
            listView1.Columns.Add("Address", 120);
            listView1.Columns.Add("City", 100);
            listView1.Columns.Add("Mobile_Number", 80);
            listView1.Columns.Add("Office_Number", 100);
            listView1.Columns.Add("Home_Number", 100);
            listView1.Columns.Add("Email", 150);
        }

        private void lblCLOSE_Click(object sender, EventArgs e)
        {
            AddNewUserForm add = new AddNewUserForm();
            add.Show();
            Hide();
        }

        private void FillListView()
        {
            try
            {
                string sqlQuery = "SELECT * FROM employee";
                SqlCommand command = new SqlCommand(sqlQuery, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["Title"].ToString());
                    item.SubItems.Add(reader["First_Name"].ToString());
                    item.SubItems.Add(reader["Last_Name"].ToString());
                    item.SubItems.Add(reader["NIC"].ToString());
                    item.SubItems.Add(reader["DOB"].ToString());
                    item.SubItems.Add(reader["Gender"].ToString());
                    item.SubItems.Add(reader["Address"].ToString());
                    item.SubItems.Add(reader["City"].ToString());
                    item.SubItems.Add(reader["Mobile"].ToString());
                    item.SubItems.Add(reader["Office_Number"].ToString());
                    item.SubItems.Add(reader["Home_Number"].ToString());
                    item.SubItems.Add(reader["Email"].ToString());

                    listView1.Items.Add(item);
                }
                reader.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayUserForm_Load(object sender, EventArgs e)
        {
            FillListView();
        }

        private void DisplayUserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }

        private void pictureboxHOME_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            Hide();
        }
    }
}
