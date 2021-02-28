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

namespace BulidingRetail
{
    public partial class Login : Form
    {
        MySqlConnection conn;
        public Login()
        {
            conn = new MySqlConnection(MainForm.connection.connectionString);
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "select User_ID from user where User_Name=@Name and Password=@Pass";
            conn.Open();
            MySqlCommand comand = new MySqlCommand(query, conn);
            comand.Parameters.AddWithValue("@Name", txtName.Text);
            comand.Parameters.AddWithValue("@Pass", txtPassword.Text);
            MySqlDataReader result = comand.ExecuteReader();

            if (result.HasRows)
            {
                MainForm f = new MainForm();
                f.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("اسم المستخدم او كلمة المرور غير صحيحة");
            }
        }
    }
}
