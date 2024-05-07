using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Specialized;



namespace WindowsFormsApplication1

{

    public partial class LoginMenu : Form
    {
        public static int adminIdCurrent;
        public static int customerIdCurrent;
        public static string loginDetails = $@"DATA SOURCE = localhost:{OracleMenu.server}; USER ID={OracleMenu.username};PASSWORD={OracleMenu.password}";


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

        public LoginMenu()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 40, 40));
        }

     


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblValue.Text = comboBox1.Text;
        }

        private void LoginMenu_Load(object sender, EventArgs e)
        {
            string conStr = loginDetails;
            connection = new OracleConnection(conStr);
        }

        private void SUBMIT_Click(object sender, EventArgs e)
        {
            try{
                string username = textBox2.Text;
                string password = textBox3.Text;
                string userType = comboBox1.Text;

                string tableName = "";

                switch (userType)
                {
                    case "Admin":
                        tableName = "admin";
                        break;
                    case "Shopkeeper":
                        tableName = "shopkeeper";
                        break;
                    case "Customer":
                        tableName = "customer";
                        break;
                }

                OracleCommand command = new OracleCommand($"SELECT COUNT(*) FROM {tableName} WHERE username = :username AND password = :password", connection);

                command.Parameters.Add(":username", OracleDbType.Varchar2).Value = username;
                command.Parameters.Add(":password", OracleDbType.Varchar2).Value = password;

                if(connection.State == ConnectionState.Closed)
                connection.Open();

                int count = Convert.ToInt32(command.ExecuteScalar());


                if (count > 0)
                {
                    if (count > 2) MessageBox.Show("duplicate accounts found!");
                    MessageBox.Show("Login successful!");

                    OracleCommand command2 = new OracleCommand("SELECT adminId FROM admin WHERE username = :username AND password = :password", connection);
                    OracleCommand command3 = new OracleCommand("SELECT customerId FROM Customer WHERE username = :username AND password = :password", connection);

                    // Add the parameters to the OracleCommand object
                    command2.Parameters.Add(":username", OracleDbType.Varchar2).Value = username;
                    command2.Parameters.Add(":password", OracleDbType.Varchar2).Value = password;
                    command3.Parameters.Add(":username", OracleDbType.Varchar2).Value = username;
                    command3.Parameters.Add(":password", OracleDbType.Varchar2).Value = password;


                    OracleDataReader reader = command2.ExecuteReader();
                    if (reader.Read())
                    {
                        adminIdCurrent = reader.GetInt32(0);
                    }

                    if (userType == "Customer")
                    {
                        OracleDataReader reader2 = command3.ExecuteReader();
                        if (reader2.Read())
                        {
                            customerIdCurrent = reader2.GetInt32(0);

                        }
                    }

                    connection.Close();




                    if (userType == "Admin")
                    {
                        MainMenu.ActiveForm.Hide();
                        AdminMenu f = new AdminMenu();
                        f.Show();
                    }
                    if (userType == "Shopkeeper")
                    {
                        MainMenu.ActiveForm.Hide();
                        ShopkeeperMenu_5 f = new ShopkeeperMenu_5();
                        f.Show();
                    }
                    if (userType == "Customer")
                    {
                        MainMenu.ActiveForm.Hide();
                        CustomerMenu f = new CustomerMenu();
                        f.Show();
                    }

                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                    textBox2.Clear();
                    textBox3.Clear();
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

        private void gradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
