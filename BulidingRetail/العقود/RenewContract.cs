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

namespace BulidingRetail.العقود
{
    public partial class RenewContract : Form
    {
        MySqlConnection dbconnection;
        byte[] selectedImage;
        bool flag=true;

        public RenewContract()
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

        private void RenewContract_Load(object sender, EventArgs e)
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
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();
                string query = "insert into oldcontract (Contract_ID,Customer_ID,Apartment_ID,ContractStartDate,ContractEndDate,ValueOfContractPerMonth,IncreaseType,IncreaseValue,InsuranceValue,Note,ContractImage) values (@Contract_ID,@Customer_ID,@Apartment_ID,@ContractStartDate,@ContractEndDate,@ValueOfContractPerMonth,@IncreaseType,@IncreaseValue,@InsuranceValue,@Note,@ContractImage)";
                MySqlCommand com = new MySqlCommand(query, dbconnection);
                com.Parameters.AddWithValue("@Contract_ID", comContract.Text);
                com.Parameters.AddWithValue("@Customer_ID", comCustomer.SelectedValue);
                com.Parameters.AddWithValue("@Apartment_ID", comApartment.SelectedValue);
                com.Parameters.AddWithValue("@ContractStartDate", dateTimePickerStart.Value.Date);
                com.Parameters.AddWithValue("@ContractEndDate", dateTimePickerEnd.Value.Date);
                com.Parameters.AddWithValue("@ValueOfContractPerMonth", txtRetalValue.Text);
                if (rdValue.Checked)
                {
                    com.Parameters.AddWithValue("@IncreaseType", "قيمة");
                }
                else
                {
                    com.Parameters.AddWithValue("@IncreaseType", "نسبة");
                }
                com.Parameters.AddWithValue("@IncreaseValue", Convert.ToDouble(txtRetalValue.Text));
                com.Parameters.AddWithValue("@InsuranceValue", Convert.ToDouble(txtInsuranceValue.Text));
                com.Parameters.AddWithValue("@Note", txtNote.Text);
                com.Parameters.AddWithValue("@ContractImage", selectedImage);
                com.ExecuteNonQuery();
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
                string query = "update contract set Customer_ID=@Customer_ID,Apartment_ID=@Apartment_ID,ContractStartDate=@ContractStartDate,ContractEndDate=@ContractEndDate,ValueOfContractPerMonth=@ValueOfContractPerMonth,IncreaseType=@IncreaseType,IncreaseValue=@IncreaseValue,InsuranceValue=@InsuranceValue,ContractImage=@ContractImage where Contract_ID=" + rowView[0].ToString();
                MySqlCommand com = new MySqlCommand(query, dbconnection);
                int id = Convert.ToInt16(rowView[1].ToString().Split(' ')[1]);
                com.Parameters.AddWithValue("@Customer_ID", id);
                id = Convert.ToInt16(rowView[3].ToString().Split(' ')[1]);
                com.Parameters.AddWithValue("@Apartment_ID", id);
                com.Parameters.AddWithValue("@ContractStartDate", rowView[5].ToString());
                com.Parameters.AddWithValue("@ContractEndDate", rowView[6].ToString());
                com.Parameters.AddWithValue("@ValueOfContractPerMonth", Convert.ToDouble(rowView[7].ToString()));
                com.Parameters.AddWithValue("@IncreaseType", rowView[8].ToString());
                com.Parameters.AddWithValue("@IncreaseValue", Convert.ToDouble(rowView[9].ToString()));
                com.Parameters.AddWithValue("@InsuranceValue", Convert.ToDouble(rowView[10].ToString()));
                com.Parameters.AddWithValue("@Note", Convert.ToDouble(rowView[11].ToString()));
                com.Parameters.AddWithValue("@ContractImage", selectedImage);
                com.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }

    }
}
