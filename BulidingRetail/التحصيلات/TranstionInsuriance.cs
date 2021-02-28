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
    public partial class TranstionInsuriance : Form
    {
        MySqlConnection dbconnection;
        bool flag = false;
        public TranstionInsuriance()
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

        private void Transtion_Load(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();
                string query = "select * from buildings";
                MySqlDataAdapter ad = new MySqlDataAdapter(query, dbconnection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                comBuilding.DataSource = dt;
                comBuilding.DisplayMember = dt.Columns["BuildingName"].ToString();
                comBuilding.ValueMember = dt.Columns["Buildings_ID"].ToString();
                comBuilding.Text = "";

                query = "select * from contract";
                ad = new MySqlDataAdapter(query, dbconnection);
                dt = new DataTable();
                ad.Fill(dt);
                comContract.DataSource = dt;
                comContract.DisplayMember = dt.Columns["Contract_ID"].ToString();
                comContract.Text = "";

                query = "select * from customer";
                ad = new MySqlDataAdapter(query, dbconnection);
                dt = new DataTable();
                ad.Fill(dt);
                comCustomer.DataSource = dt;
                comCustomer.DisplayMember = dt.Columns["Customer_Name"].ToString();
                comCustomer.ValueMember = dt.Columns["Customer_ID"].ToString();
                comCustomer.Text = "";

                query = "select * from apartments";
                ad = new MySqlDataAdapter(query, dbconnection);
                dt = new DataTable();
                ad.Fill(dt);
                comApartment.DataSource = dt;
                comApartment.DisplayMember = dt.Columns["ApartmentsNo"].ToString();
                comApartment.ValueMember = dt.Columns["Apartments_ID"].ToString();
                comApartment.Text = "";
                flag = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }

        private void comContract_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (flag)
                {
                    dbconnection.Open();
                    string query = "select * from contract inner join apartments on apartments.Apartments_ID=contract.Apartment_ID inner join buildings on buildings.Buildings_ID=apartments.Buildings_ID inner join customer on customer.Customer_ID=contract.Customer_ID where Contract_ID=" + comContract.Text;
                    MySqlCommand com = new MySqlCommand(query, dbconnection);
                    MySqlDataReader dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        comCustomer.Text = dr["Customer_Name"].ToString();
                        comBuilding.Text = dr["BuildingName"].ToString();
                        comApartment.Text = dr["ApartmentsNo"].ToString();
                        txtRetail.Text = dr["ValueOfContractPerMonth"].ToString();
                    }
                }
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
                string query = "insert into collecting (Contract_ID,Collecting_Value,Collecting_Date) values (@Contract_ID,@Collecting_Value,@Collecting_Date)";
                MySqlCommand com = new MySqlCommand(query, dbconnection);
                com.Parameters.AddWithValue("@Contract_ID", comContract.Text);
                com.Parameters.AddWithValue("@Collecting_Value", txtPaid.Text);
                com.Parameters.AddWithValue("@Collecting_Date", dateTimePicker1.Value.Date);

                com.ExecuteNonQuery();

                displayData();
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
            string query = "select Collecting_ID as 'الرقم المسلسل',Collecting_Date as 'تاريخ الدفع',Collecting_Value as 'المبلغ المدفوع',Contract_ID as 'رقم العقد' from collecting";
            MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }
    }
}
