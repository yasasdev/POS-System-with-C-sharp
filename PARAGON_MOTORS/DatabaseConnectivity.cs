using System.Data.SqlClient;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    internal class DatabaseConnectivity
    {
        private static SqlConnection conn = null;

        public static SqlConnection getConnection()
        {
            if (conn == null)
            {
                try
                {
                    string connectionString = "Connection-String";
                    conn = new SqlConnection(connectionString);
                    conn.Open();
                }
                catch (SqlException ee)
                {
                    MessageBox.Show(ee.Message, "Something went wrong with the database connection!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return conn;
        }
    }
}