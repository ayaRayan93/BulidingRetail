using BulidingRetail.البيانات;
using BulidingRetail.العقود;
using BulidingRetail.التحصيلات;
using DevExpress.XtraBars.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BulidingRetail
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            DisplayDataForms();
            DisplayContractForms();
            DisplayCollectingForms();
            //DisplayReportForms();
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.BackColor = Color.White;
            btnContact.BackColor = Color.FromArgb(151, 158, 202);
            btnReport.BackColor = Color.FromArgb(151, 158, 202);
            btnCollecting.BackColor = Color.FromArgb(151, 158, 202);
            btnExpenses.BackColor = Color.FromArgb(151, 158, 202);
            tabControl1.SelectedIndex = 0;
        }
        private void btnContract_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.BackColor = Color.White;
            btnData.BackColor = Color.FromArgb(151, 158, 202);
            btnReport.BackColor = Color.FromArgb(151, 158, 202);
            btnCollecting.BackColor = Color.FromArgb(151, 158, 202);
            btnExpenses.BackColor = Color.FromArgb(151, 158, 202);
            tabControl1.SelectedIndex = 1;
        }
        private void btnCollecting_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.BackColor = Color.White;
            btnContact.BackColor = Color.FromArgb(151, 158, 202);
            btnData.BackColor = Color.FromArgb(151, 158, 202);
            btnReport.BackColor = Color.FromArgb(151, 158, 202);
            btnExpenses.BackColor = Color.FromArgb(151, 158, 202);
            tabControl1.SelectedIndex = 3;
        }
        private void btnExpenses_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.BackColor = Color.White;
            btnContact.BackColor = Color.FromArgb(151, 158, 202);
            btnData.BackColor = Color.FromArgb(151, 158, 202);
            btnReport.BackColor = Color.FromArgb(151, 158, 202);
            btnCollecting.BackColor = Color.FromArgb(151, 158, 202);
            tabControl1.SelectedIndex = 4;
        }
        private void btnReport_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.BackColor = Color.White;
            btnContact.BackColor = Color.FromArgb(151, 158, 202);
            btnData.BackColor = Color.FromArgb(151, 158, 202);
            btnCollecting.BackColor = Color.FromArgb(151, 158, 202);
            btnExpenses.BackColor = Color.FromArgb(151, 158, 202);
            tabControl1.SelectedIndex = 2;
        }

        public void DisplayDataForms()
        {
            recordBuliding objFormExpenses = new recordBuliding();
            objFormExpenses.TopLevel = false;

            navigationPane1.Controls["navigationPage1"].Controls.Add(objFormExpenses);
            objFormExpenses.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            objFormExpenses.Dock = DockStyle.Fill;
            objFormExpenses.Show();

            RecordApartment objFormExpenses1 = new RecordApartment();
            objFormExpenses1.TopLevel = false;
            navigationPane1.Controls["navigationPage2"].Controls.Add(objFormExpenses1);
            objFormExpenses1.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            objFormExpenses1.Dock = DockStyle.Fill;
            objFormExpenses1.Show();

            recordCustomer objFormExpenses2 = new recordCustomer();
            objFormExpenses2.TopLevel = false;
            navigationPane1.Controls["navigationPage3"].Controls.Add(objFormExpenses2);
            objFormExpenses2.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            objFormExpenses2.Dock = DockStyle.Fill;
            objFormExpenses2.Show();
        }
        public void DisplayContractForms()
        {
            recordContract objFormExpenses = new recordContract();
            objFormExpenses.TopLevel = false;

            navigationPane2.Controls["navigationPage4"].Controls.Add(objFormExpenses);
            objFormExpenses.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            objFormExpenses.Dock = DockStyle.Fill;
            objFormExpenses.Show();

            RenewContract objFormExpenses1 = new RenewContract();
            objFormExpenses1.TopLevel = false;
            navigationPane2.Controls["navigationPage5"].Controls.Add(objFormExpenses1);
            objFormExpenses1.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            objFormExpenses1.Dock = DockStyle.Fill;
            objFormExpenses1.Show();

      
            
        }
        public void DisplayCollectingForms()
        {
            Transtion objFormExpenses2 = new Transtion();
            objFormExpenses2.TopLevel = false;
            navigationPane4.Controls["navigationPage8"].Controls.Add(objFormExpenses2);
            objFormExpenses2.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            objFormExpenses2.Dock = DockStyle.Fill;
            objFormExpenses2.Show();

            paidReturn objFormExpenses3 = new paidReturn();
            objFormExpenses3.TopLevel = false;
            navigationPane4.Controls["navigationPage9"].Controls.Add(objFormExpenses3);
            objFormExpenses3.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            objFormExpenses3.Dock = DockStyle.Fill;
            objFormExpenses3.Show();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static class connection
        {
            public static string connectionString = "SERVER=192.168.1.200;DATABASE=buildingdb;user=root;PASSWORD=A!S#D37;CHARSET=utf8";                                                                                                         
        }

    }
}
