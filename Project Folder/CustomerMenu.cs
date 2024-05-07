using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApplication1
{
    public partial class CustomerMenu : Form
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

        public CustomerMenu()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 40, 40));
        }

        private void CustomerMenu_Load(object sender, EventArgs e)
        {
            string conStr = LoginMenu.loginDetails;
            connection = new OracleConnection(conStr);

            OracleDataAdapter productAdapter;

            DataTable productTable = new DataTable();

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

        private void button_WOC5_Click(object sender, EventArgs e)
        {
            MainMenu.ActiveForm.Hide();
            Cart f = new Cart();
            f.Show();
        }

        private void button_WOC4_Click(object sender, EventArgs e)
        {
            string name = textBox2.Text;
            string categoryName = comboBox2.Text;
            string sqlQuery = $"SELECT p.* FROM product p JOIN category c ON p.categoryid = c.categoryid WHERE p.name LIKE '%{name}%' AND c.productname = '{categoryName}'";
            if(name == "all" || name == "ALL" || name == "everything")
            {
                sqlQuery = $"SELECT p.* FROM product p JOIN category c ON p.categoryid = c.categoryid WHERE c.productname = '{categoryName}'";

            }

            OracleCommand command = new OracleCommand(sqlQuery, connection);

            if (connection.State == ConnectionState.Closed)
                connection.Open();
            OracleDataReader reader = command.ExecuteReader();
            DataTable searchResults = new DataTable();

            searchResults.Columns.Add("ProductId", typeof(int));
            searchResults.Columns.Add("ProductName", typeof(string));
            searchResults.Columns.Add("ProductPrice", typeof(int));
            searchResults.Columns.Add("ProductDescription", typeof(string));
            while (reader.Read())
            {
                DataRow row = searchResults.NewRow();
                row["ProductId"] = Convert.ToInt32(reader["productid"]);
                row["ProductName"] = reader["name"].ToString();
                row["ProductPrice"] = Convert.ToInt32(reader["price"]);
                row["ProductDescription"] = reader["description"].ToString();

                searchResults.Rows.Add(row);
            }
            dataGridView1.DataSource = searchResults;

            connection.Close();


        }

        private void ID_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_WOC3_Click(object sender, EventArgs e)
        {
            try
            {
                string idtemp = ID.Text;
                int id = Int32.Parse(idtemp);
                string name = textBox7.Text;
                string pricetemp = textBox3.Text;
                int price = Int32.Parse(pricetemp);
                int cartId = createCart();

                int rowFounds = rowFound(id);

                if (rowFounds == 0)
                {
                    //insert
                    Random rnd = new Random();
                    int randomNumber = rnd.Next(20);
                    if (randomNumber == 0) randomNumber = rnd.Next(20);

                    OracleCommand command = new OracleCommand($"INSERT INTO CART_ITEM (CARTITEMID, QUANTITY, CARTID, PRODUCTID) VALUES ({randomNumber}, 1, {cartId},{id})", connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    int isInserted = command.ExecuteNonQuery();
                    connection.Close();

                    if (isInserted > 0) { MessageBox.Show("Added successfully!"); }
                    else MessageBox.Show("Unable to Add");


                }
                else if (rowFounds > 0)
                {
                    //update cart_item set quantity=quantity+1 where productid=11
                    string sql = $"UPDATE CART_ITEM SET quantity=quantity+1 WHERE PRODUCTID = {rowFounds}";
                    OracleCommand command = new OracleCommand(sql, connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    int rowsInserted = command.ExecuteNonQuery();

                    if (rowsInserted > 0) { MessageBox.Show("Added successfully!"); }
                    else MessageBox.Show("Unable to Add");

                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL Server exceptions
                MessageBox.Show("A SQL Server error occurred: " + ex.Message);
            }
            catch (Exception ex) { 
                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }

        int rowFound(int idToFound)
        {
            string sql = $"SELECT * FROM CART_ITEM WHERE PRODUCTID = {idToFound}";
            OracleCommand command = new OracleCommand(sql, connection);

            if (connection.State == ConnectionState.Closed)
                connection.Open();
            int rowsFound = command.ExecuteNonQuery();
            connection.Close();

            if (rowsFound > 0) 
            {
                OracleDataReader reader = command.ExecuteReader();
             return Convert.ToInt32(reader["productid"]);
            }
            else return 0;

        }

        int createCart()
        {

            Random rnd = new Random();
            int randomNumber = rnd.Next(20);
            OracleCommand command = new OracleCommand($"INSERT INTO CART (cartid, customerId) VALUES ({randomNumber}, {LoginMenu.customerIdCurrent})", connection);

            if (connection.State == ConnectionState.Closed)
                connection.Open();
            int rowsInserted = command.ExecuteNonQuery();
            connection.Close();

            if (rowsInserted > 0)
            {
                return randomNumber;
            }
            else
            {
                MessageBox.Show("Unable to Create Cart.");
                return -1;
            }
        }

    }
}
