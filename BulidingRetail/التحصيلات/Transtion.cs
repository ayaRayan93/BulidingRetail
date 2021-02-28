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

namespace BulidingRetail.العقود
{
    public partial class Transtion : Form
    {
        MySqlConnection dbconnection;
        bool flag = false;
        public Transtion()
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

                displayData();
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
                        txtInsurance.Text = dr["InsuranceValue"].ToString();
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
                string query = "insert into collecting (Contract_ID,Collecting_Value,Collecting_Date,CollectingType,Note) values (@Contract_ID,@Collecting_Value,@Collecting_Date,@CollectingType,@Note)";
                MySqlCommand com = new MySqlCommand(query, dbconnection);
                com.Parameters.AddWithValue("@Contract_ID", comContract.Text);
                com.Parameters.AddWithValue("@Collecting_Value", txtPaid.Text);
                com.Parameters.AddWithValue("@Collecting_Date", dateTimePicker1.Value.Date);
                com.Parameters.AddWithValue("@Note", txtNote.Text);
                if (radRetail.Checked)
                {
                    com.Parameters.AddWithValue("@CollectingType", radRetail.Text);
                }
                else
                {
                    com.Parameters.AddWithValue("@CollectingType", radInsurance.Text);
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

        //
        public void displayData()
        {
            string query = "select Collecting_ID as 'الرقم المسلسل',Collecting_Date as 'تاريخ الدفع',CollectingType as 'نوع الدفع',Collecting_Value as 'المبلغ المدفوع',Contract_ID as 'رقم العقد',Note as 'ملاحظة' from collecting";
            MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
            AddRepositorygridView2();
        }
        private void radRetail_CheckedChanged(object sender, EventArgs e)
        {
            if (radRetail.Checked)
            {
                label7.Visible=true;
                txtRetail.Visible = true;
                label8.Visible = false;
                txtInsurance.Visible = false;
            }
            else
            {
                label7.Visible = false;
                txtRetail.Visible = false;
                label8.Visible = true;
                txtInsurance.Visible = true;
            }
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
                        string query = "delete from collecting where Collecting_ID=" + row1[0].ToString();
                        MySqlCommand comand = new MySqlCommand(query, dbconnection);
                        dbconnection.Open();
                        comand.ExecuteNonQuery();

                        query = "ALTER TABLE collecting AUTO_INCREMENT = 1;";
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
                string query = "update collecting set Collecting_Value=@Collecting_Value ,Collecting_Date=@Collecting_Date ,Contract_ID=@Contract_ID ,CollectingType=@CollectingType where Collecting_ID=" + rowView[0].ToString();
                MySqlCommand com = new MySqlCommand(query, dbconnection);
                com.Parameters.AddWithValue("@Collecting_Value", rowView[3].ToString());
                com.Parameters.AddWithValue("@Collecting_Date", Convert.ToDateTime(rowView[1].ToString()));
                com.Parameters.AddWithValue("@Contract_ID", rowView[4].ToString());
                com.Parameters.AddWithValue("@CollectingType", rowView[2].ToString());

                com.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }
        private void AddRepositorygridView2()
        {
            RepositoryItemComboBox edit = new RepositoryItemComboBox();
            string query = "select * from contract";
            MySqlDataAdapter ad = new MySqlDataAdapter(query, dbconnection);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                edit.Items.Add(dt.Rows[i]["Contract_ID"].ToString());
            }

            gridView1.Columns["رقم العقد"].ColumnEdit = edit;
            RepositoryItemComboBox edit1 = new RepositoryItemComboBox();
            edit1.Items.Add("الايجار");
            edit1.Items.Add("رسوم تأمين");
            gridView1.Columns["نوع الدفع"].ColumnEdit = edit1;
        }
    }
}
