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

namespace BulidingRetail.البيانات
{
    public partial class recordBuliding : Form
    {
        MySqlConnection dbconnection;
        public recordBuliding()
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();
                string query = "insert into buildings (BuildingName,BuildingAddress,NoLevel,NoApartment) values (@BuildingName,@BuildingAddress,@NoLevel,@NoApartment)";
                MySqlCommand com = new MySqlCommand(query, dbconnection);
                com.Parameters.AddWithValue("@BuildingName", txtName.Text);
                com.Parameters.AddWithValue("@BuildingAddress", txtAddress.Text);
                com.Parameters.AddWithValue("@NoLevel", txtNoLevel.Text);
                com.Parameters.AddWithValue("@NoApartment", txtNoApartment.Text);

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
                        string query = "delete from buildings where Buildings_ID=" + row1[0].ToString();
                        MySqlCommand comand = new MySqlCommand(query, dbconnection);
                        dbconnection.Open();
                        comand.ExecuteNonQuery();

                        query = "ALTER TABLE buildings AUTO_INCREMENT = 1;";
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

        private void recordBuliding_Load(object sender, EventArgs e)
        {
            try
            {
                displayData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            try
            {
                dbconnection.Open();
                DataRowView rowView = e.Row as DataRowView;
                string query = "update buildings set BuildingName=@BuildingName ,BuildingAddress=@BuildingAddress ,NoLevel=@NoLevel ,NoApartment=@NoApartment where Buildings_ID=" + rowView[0].ToString();
                MySqlCommand com = new MySqlCommand(query, dbconnection);
                com.Parameters.AddWithValue("@BuildingName", rowView[1].ToString());
                com.Parameters.AddWithValue("@BuildingAddress", rowView[2].ToString());
                com.Parameters.AddWithValue("@NoLevel", rowView[3].ToString());
                com.Parameters.AddWithValue("@NoApartment", rowView[4].ToString());

                com.ExecuteNonQuery();
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
            string query = "select Buildings_ID as 'الكود', BuildingName as 'الاسم',BuildingAddress as 'العنوان',NoLevel as 'عدد الطوابق',NoApartment as 'عدد الشقق' from buildings";
            MySqlDataAdapter da = new MySqlDataAdapter(query,dbconnection);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gridControl1.DataSource = dt;
        }

     
    }
}
