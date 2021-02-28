using DevExpress.XtraGrid.Views.Grid;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BulidingRetail.MainForm;

namespace BulidingRetail.البيانات
{
    public partial class recordCustomer : Form
    {
        MySqlConnection dbconnection;
        byte[] selectedImage;

        public recordCustomer()
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
        private void recordCustomer_Load(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();
                displayData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }
        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string selectedFile = openFileDialog1.FileName;

                    pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);

                    selectedImage = File.ReadAllBytes(selectedFile);
                }
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
                string query = "insert into customer (Customer_Name,Customer_Phone,NationalID,Employment,NationalIDImage) values (@Customer_Name,@Customer_Phone,@NationalID,@Employment,@NationalIDImage)";
                MySqlCommand com = new MySqlCommand(query, dbconnection);
                com.Parameters.AddWithValue("@Customer_Name", txtCustomer.Text);
                com.Parameters.AddWithValue("@Customer_Phone", txtPhone.Text);
                com.Parameters.AddWithValue("@NationalID", txtNationalID.Text);
                com.Parameters.AddWithValue("@Employment", txtEmployment.Text);
                com.Parameters.AddWithValue("@NationalIDImage", selectedImage);
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
                        string query = "delete from customer where Customer_ID=" + row1[0].ToString();
                        MySqlCommand comand = new MySqlCommand(query, dbconnection);
                        dbconnection.Open();
                        comand.ExecuteNonQuery();

                        query = "ALTER TABLE customer AUTO_INCREMENT = 1;";
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
        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            try
            {
                dbconnection.Open();
                DataRowView rowView = e.Row as DataRowView;
                string query = "update customer set Customer_Name=@Customer_Name ,Customer_Phone=@Customer_Phone ,NationalID=@NationalID ,Employment=@Employment where Customer_ID=" + rowView[0].ToString();
                MySqlCommand com = new MySqlCommand(query, dbconnection);
                com.Parameters.AddWithValue("@Customer_Name", rowView[1].ToString());
                com.Parameters.AddWithValue("@Customer_Phone", rowView[2].ToString());
                com.Parameters.AddWithValue("@NationalID", rowView[3].ToString());
                com.Parameters.AddWithValue("@Employment", rowView[4].ToString());
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
            string query = "select Customer_ID as 'الكود', Customer_Name as 'المستأجر',Customer_Phone as 'التلفون',NationalID as 'الرقم القومي',Employment as 'الوظيفة' from customer";
            MySqlDataAdapter ad = new MySqlDataAdapter(query, dbconnection);
            DataTable dt = new DataTable();
            ad.Fill(dt);

            gridControl1.DataSource = dt;
        }

    
    }
}
