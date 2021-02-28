using DevExpress.XtraGrid.Views.Grid;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BulidingRetail.MainForm;

namespace BulidingRetail.التحصيلات
{
    public partial class paidReturn : Form
    {
        MySqlConnection dbconnection;

        public paidReturn()
        {
            try
            {
                dbconnection = new MySqlConnection(connection.connectionString);
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void paidReturn_Load(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();
                string query = "select * from customer";
                MySqlDataAdapter ad = new MySqlDataAdapter(query, dbconnection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                comCustomer.DataSource = dt;
                comCustomer.DisplayMember = dt.Columns["Customer_Name"].ToString();
                comCustomer.ValueMember = dt.Columns["Customer_ID"].ToString();
                comCustomer.Text = "";

                displayData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();
                string query = "insert into transation (Customer_ID,Date,Amount,TransationType,Note) values (@Customer_ID,@Date,@Amount,@TransationType,@Note)";
                MySqlCommand com = new MySqlCommand(query, dbconnection);
                com.Parameters.AddWithValue("@Customer_ID", comCustomer.SelectedValue);
                com.Parameters.AddWithValue("@Amount", txtPaid.Text);
                com.Parameters.AddWithValue("@Date", dateTimePicker1.Value.Date);
                com.Parameters.AddWithValue("@Note", txtNote.Text);
                if (radPaid.Checked)
                {
                    com.Parameters.AddWithValue("@TransationType", radPaid.Text);
                }
                else
                {
                    com.Parameters.AddWithValue("@TransationType", radioButton2.Text);
                }
                com.ExecuteNonQuery();
                displayData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataRowView row1 = (DataRowView)(((GridView)gridControl1.MainView).GetRow(((GridView)gridControl1.MainView).GetSelectedRows()[0]));
                if (row1 != null)
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete the item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string query = "delete from transation where Transation_ID=" + row1[0].ToString();
                        MySqlCommand comand = new MySqlCommand(query, dbconnection);
                        dbconnection.Open();
                        comand.ExecuteNonQuery();

                        query = "ALTER TABLE transation AUTO_INCREMENT = 1;";
                        MySqlCommand com = new MySqlCommand(query, dbconnection);
                        com.ExecuteNonQuery();
                        displayData();
                    }
                    else if (dialogResult == DialogResult.No)
                    { }

                }
                else
                {
                    MessageBox.Show("you must select an item");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }
       
        //
        public void displayData()
        {
            string query = "select * from transation";
            MySqlDataAdapter da = new MySqlDataAdapter(query,dbconnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

    }
}
