using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class LoginForm : Form
    {
        private SqlConnection conn = null;

        public LoginForm()
        {
            InitializeComponent();
            conn = DatabaseConnectivity.getConnection();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            readData();
        }

        public void storeDate()
        {
            // Get the current date
            DateTime currentDate = DateTime.Now;

            // Define the file path where the date will be stored

            string filePath = @"D:\current_date.txt";

            try
            {
                // Write the current date to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(currentDate.ToString("yyyy-MM-dd")); // Format the date as desired
                }

                //MessageBox.Show("Current date has been saved to the file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void readData()
        {
            // Define the file path from where to read the date
            string filePath = @"D:\current_date.txt";

            try
            {
                // Check if the file exists
                if (!File.Exists(filePath))
                {
                    // Create the file if it doesn't exist and write the current date to it
                    File.WriteAllText(filePath, DateTime.Now.ToString("yyyy-MM-dd"));
                }

                // Read the date from the file
                string dateString = File.ReadAllText(filePath).Trim(); // Trim any leading or trailing whitespace

                // Parse the string into a DateTime object
                DateTime newDate;
                if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out newDate))
                {
                    // Compare newDate with currentDate
                    DateTime currentDate = DateTime.Now;

                    if (newDate.Date != currentDate.Date)
                    {
                        // Assuming you have a MySqlConnection object named 'conn' defined elsewhere
                        try
                        {
                            // Perform the deletion
                            string sqlQuery1 = "DELETE FROM salesreport_temp";
                            SqlCommand command1 = new SqlCommand(sqlQuery1, conn);
                            int rowsAffected = command1.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                //MessageBox.Show("Data deleted successfully!");
                            }
                            else
                            {
                                //MessageBox.Show("No data deleted.");
                            }

                            // Perform the deletion
                            string sqlQuery2 = "DELETE FROM discount_table";
                            SqlCommand command2 = new SqlCommand(sqlQuery2, conn);
                            int rowsAffected1 = command2.ExecuteNonQuery();

                            if (rowsAffected1 > 0)
                            {
                                //MessageBox.Show("Data deleted successfully!");
                            }
                            else
                            {
                                //MessageBox.Show("No data deleted.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Date format in the file is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void threeMonthsOld()
        {
            try
            {

                SqlCommand cmd = new SqlCommand("DELETE FROM salesreport_perm WHERE Date < DATE_SUB(NOW(), INTERVAL 6 MONTHS)", conn);
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Three months old data has been deleted successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void lblCLOSE_Click(object sender, EventArgs e)
        {
        }

        private void btnLOGIN_Click(object sender, EventArgs e)
        {
            // Assuming you have text boxes txtUsername and txtPassword for username and password input
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Add your authentication logic here
            bool isAuthenticated = AuthenticateUser(username, password);

            if (isAuthenticated)
            {
                InsertLoginDetails(username);
                Dashboard db = new Dashboard();
                db.Show();
                Hide();
            }
            else
            {
                // Login failed
                MessageBox.Show("Incorrect Username or Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsername.Clear();
                txtPassword.Clear();
                txtUsername.Focus();
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            string query = "SELECT * FROM userlogin where username = @v1 and password = @v2";
            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@v1", txtUsername.Text);
                command.Parameters.AddWithValue("@v2", txtPassword.Text);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public void InsertLoginDetails(string username)
        {
            try
            {
                {
                    // SQL command to insert login details
                    string query = "INSERT INTO login_details (username, login_time) VALUES (@Username, @LoginTime)";
                    SqlCommand command = new SqlCommand(query, conn);

                    // Parameters
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@LoginTime", DateTime.Now);

                    // Execute the command
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting login details: " + ex.Message);
            }
        }

        private void ClearData()
        {
            txtUsername.Clear();
            txtPassword.Clear();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            AdministratorLOGIN admin = new AdministratorLOGIN();
            admin.Show();
            Hide();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
            storeDate();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            ChangeAccountSetting changeAccountSetting = new ChangeAccountSetting();
            changeAccountSetting.Show();
            Hide();
        }

        private void label10_Click_2(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
