using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
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
    public partial class RecordApartment : Form
    {
        MySqlConnection dbconnection;
        public RecordApartment()
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
        private void RecordApartment_Load(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();
                displayData();
                string query = "select * from buildings";
                MySqlDataAdapter ad = new MySqlDataAdapter(query,dbconnection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                comBuilding.DataSource = dt;
                comBuilding.DisplayMember = dt.Columns["BuildingName"].ToString();
                comBuilding.ValueMember = dt.Columns["Buildings_ID"].ToString();
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
                string query = "insert into apartments (ApartmentsNo,LevelNo,Buildings_ID) values (@ApartmentsNo,@LevelNo,@Buildings_ID)";
                MySqlCommand com = new MySqlCommand(query, dbconnection);
                com.Parameters.AddWithValue("@ApartmentsNo", txtApartmentNo.Text);
                com.Parameters.AddWithValue("@LevelNo", txtLevelNo.Text);
                com.Parameters.AddWithValue("@Buildings_ID", comBuilding.SelectedValue);
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
                        string query = "delete from apartments where Apartments_ID=" + row1[0].ToString();
                        MySqlCommand comand = new MySqlCommand(query, dbconnection);
                        dbconnection.Open();
                        comand.ExecuteNonQuery();

                        query = "ALTER TABLE apartments AUTO_INCREMENT = 1;";
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
                string query = "update apartments set ApartmentsNo=@ApartmentsNo ,LevelNo=@LevelNo ,Buildings_ID=@Buildings_ID where Apartments_ID=" + rowView[0].ToString();
                MySqlCommand com = new MySqlCommand(query, dbconnection);
                com.Parameters.AddWithValue("@ApartmentsNo", rowView[1].ToString());
                com.Parameters.AddWithValue("@LevelNo", rowView[2].ToString());
                int id =Convert.ToInt16(rowView[3].ToString().Split(' ')[1]);
                com.Parameters.AddWithValue("@Buildings_ID", id);

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
            string query = "select Apartments_ID as 'الكود',ApartmentsNo as 'رقم الشقة',LevelNo as 'رقم الطابق',concat(BuildingName,' ',buildings.Buildings_ID) as 'اسم المبني',RelailStatus as 'الحالة' from apartments inner join buildings on apartments.Buildings_ID=buildings.Buildings_ID";
            MySqlDataAdapter ad = new MySqlDataAdapter(query, dbconnection);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            
            gridControl1.DataSource = dt;
            AddRepositorygridView2();
        }
        private void AddRepositorygridView2()
        {

            RepositoryItemComboBox edit=new RepositoryItemComboBox();
            string query = "select * from buildings";
            MySqlDataAdapter ad = new MySqlDataAdapter(query, dbconnection);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                edit.Items.Add(dt.Rows[i]["BuildingName"].ToString()+" "+ dt.Rows[i]["Buildings_ID"].ToString());
            }

            gridView1.Columns["اسم المبني"].ColumnEdit = edit;
        }

       
     }
}
