using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml.Linq;

namespace WindowsFormsApplication1
{
    public partial class ShopkeeperMenu_5 : Form
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

        public ShopkeeperMenu_5()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 40, 40));
        }


    

        private void ShopkeeperMenu_5_Load(object sender, EventArgs e)
        {
            string conStr = LoginMenu.loginDetails;
            connection = new OracleConnection(conStr);


            // Create a new OracleDataAdapter object for each table
            OracleDataAdapter productAdapter = new OracleDataAdapter("SELECT * FROM product", connection);

            // Create new DataTable objects for each table
            DataTable productTable = new DataTable();

            // Fill the DataTable objects with the current data from the respective tables
            productAdapter.Fill(productTable);

            // Set the DataSource property of each DataGridView control to the corresponding DataTable object
            dataGridView1.DataSource = productTable;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button_WOC2_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void button_WOC1_Click(object sender, EventArgs e)
        {
            MainMenu.ActiveForm.Hide();
            MainMenu f = new MainMenu();
            f.Show();
        }

        private void button_WOC3_Click(object sender, EventArgs e)
        {
            try
            {

                string productid = textBox11.Text;
                string name = textBox10.Text;
                string price = textBox8.Text;
                string description = textBox9.Text;
                string shopkeeperid = textBox6.Text;
                int currentAdminId = LoginMenu.adminIdCurrent;
                string categoryid = textBox7.Text;


                OracleCommand command = new OracleCommand("INSERT INTO product (productid, name, price, description, shopkeeperid, adminId, categoryid) VALUES (:productid, :name, :price, :description, :shopekeeperid, :currentAdminId, :categoryid)", connection);


                command.Parameters.Add(":productid", OracleDbType.Int64).Value = productid;
                command.Parameters.Add(":name", OracleDbType.Varchar2).Value = name;
                command.Parameters.Add(":price", OracleDbType.Int64).Value = price;
                command.Parameters.Add(":description", OracleDbType.Varchar2).Value = description;
                command.Parameters.Add(":shopkeeperid", OracleDbType.Int64).Value = shopkeeperid;
                command.Parameters.Add(":currentAdminId", OracleDbType.Int64).Value = currentAdminId;
                command.Parameters.Add(":categoryid", OracleDbType.Int64).Value = categoryid;

                if (connection.State == ConnectionState.Closed)
                    connection.Open(); int rowsInserted = command.ExecuteNonQuery();
                connection.Close();

                if (rowsInserted > 0)
                {

                    DataTable productTable = (DataTable)dataGridView1.DataSource;
                    productTable.Clear();
                    OracleDataAdapter productTableAdapter = new OracleDataAdapter("SELECT * FROM product", connection);
                    productTableAdapter.Fill(productTable);

                    dataGridView1.DataSource = productTable;
                    MessageBox.Show("Product created successfully!");
                }

                else
                {
                    MessageBox.Show("Error creating Product.");
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void gradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button_WOC5_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedProductId = ID.Text;
                string sql = "DELETE FROM product WHERE PRODUCTID = :id";

                OracleCommand command = new OracleCommand(sql, connection);

                command.Parameters.Add(":id", OracleDbType.Int64).Value = selectedProductId;
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                int rowsInserted = command.ExecuteNonQuery();
                connection.Close();

                if (rowsInserted > 0)
                {

                    DataTable productTable = (DataTable)dataGridView1.DataSource;
                    productTable.Clear();
                    OracleDataAdapter productTableAdapter = new OracleDataAdapter("SELECT * FROM product", connection);
                    productTableAdapter.Fill(productTable);

                    dataGridView1.DataSource = productTable;
                    MessageBox.Show("Product deleted successfully!");
                }

                else
                {
                    MessageBox.Show("Error deleting Product.");
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void button_WOC4_Click(object sender, EventArgs e)
        {
            try
            {
                string productid = textBox11.Text;
                string name = textBox10.Text;
                string price = textBox8.Text;
                string description = textBox9.Text;
                string shopkeeperid = textBox6.Text;
                int currentAdminId = LoginMenu.adminIdCurrent;
                string categoryid = textBox7.Text;

                string sql = "UPDATE product SET productid = :productid, name = :name, price = :price, description = :description, shopkeeperid = :shopkeeperid, adminId = :currentAdminId, categoryid = :categoryid WHERE productid = :productid";

                OracleCommand command = new OracleCommand(sql, connection);


                command.Parameters.Add(":productid", OracleDbType.Int64).Value = productid;
                command.Parameters.Add(":name", OracleDbType.Varchar2).Value = name;
                command.Parameters.Add(":price", OracleDbType.Int64).Value = price;
                command.Parameters.Add(":description", OracleDbType.Varchar2).Value = description;
                command.Parameters.Add(":shopkeeperid", OracleDbType.Int64).Value = shopkeeperid;
                command.Parameters.Add(":currentAdminId", OracleDbType.Int64).Value = currentAdminId;
                command.Parameters.Add(":categoryid", OracleDbType.Int64).Value = categoryid;

                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                int rowsInserted = command.ExecuteNonQuery();
                connection.Close();

                if (rowsInserted > 0)
                {

                    DataTable productTable = (DataTable)dataGridView1.DataSource;
                    productTable.Clear();
                    OracleDataAdapter productTableAdapter = new OracleDataAdapter("SELECT * FROM product", connection);
                    productTableAdapter.Fill(productTable);

                    dataGridView1.DataSource = productTable;
                    MessageBox.Show("Product updated successfully!");
                }

                else
                {
                    MessageBox.Show("Error updated Product.");

                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
