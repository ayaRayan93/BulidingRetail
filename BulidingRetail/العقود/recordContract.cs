using DevExpress.XtraEditors.Repository;
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

namespace BulidingRetail.العقود
{
    public partial class recordContract : Form
    {
        MySqlConnection dbconnection;
        bool flag = false;
        byte[] selectedImage;

        public recordContract()
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
        private void recordContract_Load(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();
                displayData();
                string query = "select * from buildings";
                MySqlDataAdapter ad = new MySqlDataAdapter(query, dbconnection);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                comBuilding.DataSource = dt;
                comBuilding.DisplayMember = dt.Columns["BuildingName"].ToString();
                comBuilding.ValueMember = dt.Columns["Buildings_ID"].ToString();
                comBuilding.Text = "";

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
        private void comBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (flag)
                {
                    string query = "select * from apartments where Buildings_ID="+comBuilding.SelectedValue;
                    MySqlDataAdapter ad = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    comApartment.DataSource = dt;
                    comApartment.DisplayMember = dt.Columns["ApartmentsNo"].ToString();
                    comApartment.ValueMember = dt.Columns["Apartments_ID"].ToString();
                    comApartment.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                string query = "insert into contract (Customer_ID,Apartment_ID,ContractStartDate,ContractEndDate,ValueOfContractPerMonth,IncreaseType,IncreaseValue,InsuranceValue,Note,ContractImage) values (@Customer_ID,@Apartment_ID,@ContractStartDate,@ContractEndDate,@ValueOfContractPerMonth,@IncreaseType,@IncreaseValue,@InsuranceValue,@Note,@ContractImage)";
                MySqlCommand com = new MySqlCommand(query, dbconnection);
                com.Parameters.AddWithValue("@Customer_ID", comCustomer.SelectedValue);
                com.Parameters.AddWithValue("@Apartment_ID",comApartment.SelectedValue);
                com.Parameters.AddWithValue("@ContractStartDate",dateTimePickerStart.Value.Date);
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
                com.Parameters.AddWithValue("@IncreaseValue",Convert.ToDouble(txtRetalValue.Text));
                com.Parameters.AddWithValue("@InsuranceValue", Convert.ToDouble(txtInsuranceValue.Text));
                com.Parameters.AddWithValue("@Note", txtNote.Text);
                com.Parameters.AddWithValue("@ContractImage", selectedImage);
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
                        string query = "delete from contract where Contract_ID=" + row1[0].ToString();
                        MySqlCommand comand = new MySqlCommand(query, dbconnection);
                        dbconnection.Open();
                        comand.ExecuteNonQuery();

                        query = "ALTER TABLE contract AUTO_INCREMENT = 1;";
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
                string query = "update contract set Customer_ID=@Customer_ID,Apartment_ID=@Apartment_ID,ContractStartDate=@ContractStartDate,ContractEndDate=@ContractEndDate,ValueOfContractPerMonth=@ValueOfContractPerMonth,IncreaseType=@IncreaseType,IncreaseValue=@IncreaseValue,InsuranceValue=@InsuranceValue,Note=@Note where Contract_ID=" + rowView[0].ToString();
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
            string query = "select Contract_ID as 'رقم العقد',concat(Customer_Name,' ',customer.Customer_ID) as 'اسم العميل',buildings.NoLevel as 'رقم الطابق',concat(ApartmentsNo,' ',apartments.Apartments_ID) as 'رقم الشقة',concat(BuildingName,' ',buildings.Buildings_ID) as 'اسم المبني',ContractStartDate as 'تاريخ بدء العقد',ContractEndDate as 'تاريخ نهاية العقد',ValueOfContractPerMonth as 'قيمة الايجار الشهري',IncreaseType as 'نوع الزيادة',IncreaseValue as 'قيمة الزيادة',InsuranceValue as 'قيمة التأمين', Note as 'ملاحظة' from contract inner join apartments on apartments.Apartments_ID=contract.Apartment_ID inner join buildings on buildings.Buildings_ID=apartments.Buildings_ID inner join customer on customer.Customer_ID=contract.Customer_ID";
            MySqlDataAdapter ad = new MySqlDataAdapter(query, dbconnection);
            DataTable dt = new DataTable();
            ad.Fill(dt);

            gridControl1.DataSource = dt;
            AddRepositorygridView2();
        }
        private void AddRepositorygridView2()
        {
            RepositoryItemComboBox edit = new RepositoryItemComboBox();
            string query = "select * from customer";
            MySqlDataAdapter ad = new MySqlDataAdapter(query, dbconnection);
            DataTable dt = new DataTable();
            ad.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                edit.Items.Add(dt.Rows[i]["Customer_Name"].ToString() + " " + dt.Rows[i]["Customer_ID"].ToString());
            }

            gridView1.Columns["اسم العميل"].ColumnEdit = edit;

            edit = new RepositoryItemComboBox();
            query = "select * from buildings";
            ad = new MySqlDataAdapter(query, dbconnection);
            dt = new DataTable();
            ad.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                edit.Items.Add(dt.Rows[i]["BuildingName"].ToString() + " " + dt.Rows[i]["Buildings_ID"].ToString());
            }

            gridView1.Columns["اسم المبني"].ColumnEdit = edit;

            edit = new RepositoryItemComboBox();
            query = "select * from apartments";
            ad = new MySqlDataAdapter(query, dbconnection);
            dt = new DataTable();
            ad.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                edit.Items.Add(dt.Rows[i]["ApartmentsNo"].ToString() + " " + dt.Rows[i]["Apartments_ID"].ToString());
            }

            gridView1.Columns["رقم الشقة"].ColumnEdit = edit;
        }

    }
}
