using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApplication1
{
    public partial class Cart : Form
    {
        OracleConnection connection;
        public Cart()
        {
            InitializeComponent();
        }
        private void Cart_Load(object sender, EventArgs e)
        {
            string conStr = LoginMenu.loginDetails;
            connection = new OracleConnection(conStr);

            OracleDataAdapter cartItemsAdapter = new OracleDataAdapter("SELECT * FROM cart_item", connection);
            DataTable cartItemsTable = new DataTable();
            cartItemsAdapter.Fill(cartItemsTable);
            dataGridView1.DataSource = cartItemsTable;

            showCartStats();

        }

        void showCartStats()
        {
            try
            {
                connection.Open();
                string query = "SELECT SUM(QUANTITY) AS TotalQuantity, SUM(p.PRICE) AS TotalPrice FROM CART_ITEM c INNER JOIN PRODUCT p ON c.PRODUCTID = p.PRODUCTID";
                OracleCommand command = new OracleCommand(query, connection);
                OracleDataReader reader = command.ExecuteReader();

                int totalQuantity = 0;
                decimal totalPrice = 0;

                if (reader.Read())
                {
                    totalQuantity = Convert.ToInt32(reader["TotalQuantity"]);
                    totalPrice = Convert.ToDecimal(reader["TotalPrice"]);
                }

                reader.Close();

                label4.Text = totalQuantity.ToString();
                label3.Text = totalPrice.ToString();
                connection.Close();
            }catch(Exception ex)
            {
                MessageBox.Show("An error occured, maybe you didn't added Items to Cart: " + ex.Message);
            }

        }

        private void button_WOC1_Click(object sender, EventArgs e)
        {
            MainMenu.ActiveForm.Hide();
            CustomerMenu f = new CustomerMenu();
            f.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void EXIT_Click(object sender, EventArgs e)
        {
            try
            {
                OracleCommand command1 = new OracleCommand("DELETE FROM cart", connection);
                
                if(connection.State == ConnectionState.Closed)
                connection.Open();
                
                command1.ExecuteNonQuery();
                connection.Close();


                OracleCommand command = new OracleCommand("DELETE FROM cart_item", connection);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                int rowsInserted = command.ExecuteNonQuery();
                connection.Close();

                if (rowsInserted > 0)
                {
                    DataTable cartItemsTable = (DataTable)dataGridView1.DataSource;
                    cartItemsTable.Clear();
                    OracleDataAdapter cartItemsAdapter = new OracleDataAdapter("SELECT * FROM cart_item", connection);
                    cartItemsAdapter.Fill(cartItemsTable);
                    label3.Text = "0";
                    label4.Text = "0";

                    dataGridView1.DataSource = cartItemsTable;
                }

            }
            catch (OracleException ex)
            {
                MessageBox.Show("Error: \n" + ex.Message);
            }
            Application.Exit();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button_WOC3_Click(object sender, EventArgs e)
        {
            try
            {
                OracleCommand command = new OracleCommand("DELETE FROM cart_item", connection);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                int rowsInserted = command.ExecuteNonQuery();
                connection.Close();

                if (rowsInserted > 0)
                {
                    DataTable cartItemsTable = (DataTable)dataGridView1.DataSource;
                    cartItemsTable.Clear();
                    OracleDataAdapter cartItemsAdapter = new OracleDataAdapter("SELECT * FROM cart_item", connection);
                    cartItemsAdapter.Fill(cartItemsTable);
                    label3.Text = "0";
                    label4.Text = "0";

                    dataGridView1.DataSource = cartItemsTable;
                    MessageBox.Show("Checked Out Successfully!");
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Error: \n" + ex.Message);
            }
        }

        private void button_WOC4_Click(object sender, EventArgs e)
        {
            string id = ID.Text;

            OracleCommand command = new OracleCommand($"DELETE FROM cart_item WHERE cartitemid = :id", connection);
            command.Parameters.Add(":id", OracleDbType.Int64).Value = id;

            if (connection.State == ConnectionState.Closed)
                connection.Open();
            int rowsInserted = command.ExecuteNonQuery();
            connection.Close();

            if (rowsInserted > 0)
            {
                showCartStats();

                DataTable cartitemsTable = (DataTable)dataGridView1.DataSource;
                cartitemsTable.Clear();
                    OracleDataAdapter cartitemsAdapter = new OracleDataAdapter("SELECT * FROM cart_item", connection);
                cartitemsAdapter.Fill(cartitemsTable);
                    dataGridView1.DataSource = cartitemsTable;
                    MessageBox.Show("Deleted successfully!");




                
            }
            else
            {
                MessageBox.Show("Error Deleting.");
            }
        }
    }
}
