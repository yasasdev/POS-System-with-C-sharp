using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class LogHistoryForm : Form
    {
        private SqlConnection conn = null;
        public LogHistoryForm()
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
            Hide();
        }

        private void lblCLOSE_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            Hide();
        }

        private void pictureboxADMIN_Click(object sender, EventArgs e)
        {
            AdministratorLOGIN admin = new AdministratorLOGIN();
            admin.Show();
            Hide();
        }

        private void LogHistoryForm_Load(object sender, EventArgs e)
        {
            FillListView();
        }

        private void InitializeListView()
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            listView1.Columns.Add("username", 645);
            listView1.Columns.Add("login_time", 645);
        }

        private void FillListView()
        {
            try
            {
                string sqlQuery = "SELECT * FROM login_details";
                SqlCommand command = new SqlCommand(sqlQuery, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["username"].ToString());
                    item.SubItems.Add(reader["login_time"].ToString());

                    listView1.Items.Add(item);
                }
                reader.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
