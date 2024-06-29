using System;
using System.Windows.Forms;

namespace PARAGON_MOTORS
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();

            Timer timer = new Timer();
            timer.Interval = 1000; 
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateDateTime();
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

        private void lblCLOSE_Click(object sender, EventArgs e)
        {
            LoginForm log = new LoginForm();
            log.Show();
            Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void pictureboxHOME_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GRNform gRNform = new GRNform();
            gRNform.Show();
            Hide();
            /*AdministratorLOGIN loginForm = new AdministratorLOGIN();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
            }*/
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            StockForm stockForm = new StockForm();
            stockForm.Show();
            Hide();
        }

        private void btnGRNHistory_Click(object sender, EventArgs e)
        {
            GRNHistoryForm grnhis = new GRNHistoryForm();
            grnhis.Show();
            Hide();
        }

        private void btnGRNReturn_Click(object sender, EventArgs e)
        {
            GRNRetrunForm grnreturn = new GRNRetrunForm();
            grnreturn.Show();
            Hide();
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            SuppliersForm suppliersForm = new SuppliersForm();
            suppliersForm.Show();
            Hide();
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            InvoiceForm invoiceForm = new InvoiceForm();
            invoiceForm.Show();
            Hide();
        }

        private void btnInvoiceHistory_Click(object sender, EventArgs e)
        {
            InvoiceHistoryForm ih = new InvoiceHistoryForm();
            ih.Show();
            Hide();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            LogHistoryForm logHistoryForm = new LogHistoryForm();
            logHistoryForm.Show();
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddNewUserForm add = new AddNewUserForm();
            add.Show();
            Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddQuantityForm grn = new AddQuantityForm();
            grn.Show();
            Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SalesReport sp = new SalesReport();
            sp.Show();
            Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ChangeAccountSetting setting = new ChangeAccountSetting();
            setting.Show();
            Hide();
        }

        private void btnQuantity_Click(object sender, EventArgs e)
        {
            AdministratorLOGIN loginForm = new AdministratorLOGIN();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                AddQuantityForm quantityForm = new AddQuantityForm();
                quantityForm.Show();
                Hide();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            InvoiceReturn invoiceReturn = new InvoiceReturn();
            invoiceReturn.Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LowStockForm lw = new LowStockForm();
            lw.Show();
            Hide();
        }

        private void pictureboxHOTLINE_Click(object sender, EventArgs e)
        {
            ContactUS contactUS = new ContactUS();
            contactUS.Show();
            Hide();
        }

        private void lblCLOSE_Click_1(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }
    }
}
