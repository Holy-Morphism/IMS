using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace WindowsFormsApplication1
{
    public partial class RegisterMenu : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(

 int nLeftReact,
int ntopReact,
int nBottomReact,
int nRightReact,
int nWidthElipse,
int nHeightElipse

  );

        OracleConnection connection;

        public RegisterMenu()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 40, 40));
        }


        private void RegisterMenu_Load(object sender, EventArgs e)
        {
            string conStr = LoginMenu.loginDetails;
            connection = new OracleConnection(conStr);
        }

        private void SUBMIT_Click(object sender, EventArgs e)
        {

            try
            {
                string username = textBox2.Text;
                string password = textBox3.Text;
                string userType = comboBox1.Text;

                string tableName = "";
                string tableId = "";

                switch (userType)
                {
                    case "Admin":
                        tableName = "admin";
                        tableId = "adminId";
                        break;
                    case "Shopkeeper":
                        tableName = "shopkeeper";
                        tableId = "shopkeeperId";
                        break;
                    case "Customer":
                        tableName = "customer";
                        tableId = "customerId";
                        break;
                    default:
                        MessageBox.Show("Invalid user type selected.");
                        return;
                }

                Random rnd = new Random();
                int i = rnd.Next(10);
                if (userType == "Admin")
                {
                    OracleCommand command = new OracleCommand($"INSERT INTO {tableName} ({tableId}, username, password) VALUES (:i, :username, :password)", connection);
                    command.Parameters.Add(":adminId", OracleDbType.Int64).Value = i;
                    command.Parameters.Add(":username", OracleDbType.Varchar2).Value = username;
                    command.Parameters.Add(":password", OracleDbType.Varchar2).Value = password;

                    if (connection.State == ConnectionState.Closed)
                        connection.Open(); int rowsInserted = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsInserted > 0)
                    {
                        MessageBox.Show("User registered successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Error registering user.");
                    }
                }
                else
                {
                    int defaultAdmin = 5;
                    OracleCommand command = new OracleCommand($"INSERT INTO {tableName} ({tableId}, username, password, adminid) VALUES (:i, :username, :password, :admin)", connection);
                    command.Parameters.Add(":adminId", OracleDbType.Int64).Value = i;
                    command.Parameters.Add(":username", OracleDbType.Varchar2).Value = username;
                    command.Parameters.Add(":password", OracleDbType.Varchar2).Value = password;
                    command.Parameters.Add(":admin", OracleDbType.Int64).Value = defaultAdmin;

                    if (connection.State == ConnectionState.Closed)
                        connection.Open(); int rowsInserted = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsInserted > 0)
                    {
                        MessageBox.Show("User registered successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Error registering user.");
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }



        }

        private void button_WOC1_Click(object sender, EventArgs e)
        {
            MainMenu.ActiveForm.Hide();
            MainMenu f = new MainMenu();
            f.Show();
        }

        private void button_WOC2_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void gradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblValue_Click(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblValue.Text = comboBox1.Text;
            lblValue.BackColor = Color.Red;
            WaitSomeTime();          
            


        }

        public async void WaitSomeTime()
        {
            await Task.Delay(200);
            lblValue.BackColor = Color.White;
        }
    }
}
