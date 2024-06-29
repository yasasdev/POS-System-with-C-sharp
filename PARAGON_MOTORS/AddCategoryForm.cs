using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class AddCategoryForm : Form
    {
        private SqlConnection conn = null;

        public AddCategoryForm()
        {
            InitializeComponent();
            conn = DatabaseConnectivity.getConnection();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO category VALUES (@val1)", conn);
                cmd.Parameters.AddWithValue("@val1", txtCategory.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Category Added successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void lblClose_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
            Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
            Hide();
        }
    }
}
