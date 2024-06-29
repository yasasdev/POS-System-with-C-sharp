using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class ContactUS : Form
    {
        public ContactUS()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
            Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}
