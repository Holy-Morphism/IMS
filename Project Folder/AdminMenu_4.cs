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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApplication1
{
    public partial class AdminMenu : Form
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

        public AdminMenu()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 40, 40));
        }



        private void AdminMenu_Load(object sender, EventArgs e)
        {
            string conStr = LoginMenu.loginDetails;
            connection = new OracleConnection(conStr);

            // Create a new OracleDataAdapter object for each table
            OracleDataAdapter shopkeeperAdapter = new OracleDataAdapter("SELECT * FROM shopkeeper", connection);
            OracleDataAdapter customerAdapter = new OracleDataAdapter("SELECT * FROM customer", connection);
            OracleDataAdapter productAdapter = new OracleDataAdapter("SELECT * FROM product", connection);

            // Create new DataTable objects for each table
            DataTable shopkeeperTable = new DataTable();
            DataTable customerTable = new DataTable();
            DataTable productTable = new DataTable();

            // Fill the DataTable objects with the current data from the respective tables
            shopkeeperAdapter.Fill(shopkeeperTable);
            customerAdapter.Fill(customerTable);
            productAdapter.Fill(productTable);

            // Set the DataSource property of each DataGridView control to the corresponding DataTable object
            dataGridView1.DataSource = shopkeeperTable;
            dataGridView2.DataSource = customerTable;
            dataGridView3.DataSource = productTable;

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button_WOC7_Click(object sender, EventArgs e)
        {
            try
            {
                string id = textBox17.Text;
                string usernameI = textBox16.Text;
                string passwordI = textBox15.Text;
                string userType = comboBox2.Text;
                int currentAdminId = LoginMenu.adminIdCurrent;


                string tableName = "";
                string tableId = "";

                switch (userType)
                {
                    case "Shopkeeper":
                        tableName = "shopkeeper";
                        tableId = "shopkeeperId";

                        break;
                    case "Customer":
                        tableName = "customer";
                        tableId = "customerId";

                        break;
                }

                OracleCommand command = new OracleCommand($"INSERT INTO {tableName} ({tableId}, username, password, adminId) VALUES (:id, :username, :password, :currentAdminId)", connection);


                command.Parameters.Add(":id", OracleDbType.Int64).Value = id;
                command.Parameters.Add(":username", OracleDbType.Varchar2).Value = usernameI;
                command.Parameters.Add(":password", OracleDbType.Varchar2).Value = passwordI;
                command.Parameters.Add(":currentAdminId", OracleDbType.Int64).Value = currentAdminId;

                if (connection.State == ConnectionState.Closed)
                    connection.Open(); int rowsInserted = command.ExecuteNonQuery();
                connection.Close();

                if (rowsInserted > 0)
                {
                    if (userType == "Shopkeeper")
                    {
                        // Refill the DataTable object for the shopkeeper table using the OracleDataAdapter object
                        DataTable shopkeeperTable = (DataTable)dataGridView1.DataSource;
                        shopkeeperTable.Clear();
                        OracleDataAdapter shopkeeperAdapter = new OracleDataAdapter("SELECT * FROM shopkeeper", connection);
                        shopkeeperAdapter.Fill(shopkeeperTable);

                        // Update the DataSource property of the DataGridView control for the shopkeeper table to reflect the changes
                        dataGridView1.DataSource = shopkeeperTable;
                        MessageBox.Show("Added successfully!");
                    }
                    if (userType == "Customer")
                    {

                        DataTable customerTable = (DataTable)dataGridView2.DataSource;
                        customerTable.Clear();
                        OracleDataAdapter customerAdapter = new OracleDataAdapter("SELECT * FROM customer", connection);
                        customerAdapter.Fill(customerTable);

                        dataGridView2.DataSource = customerTable;
                        MessageBox.Show("Added successfully!");
                    }
                }
                else
                {
                    MessageBox.Show("Error Adding.");
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void button_WOC3_Click(object sender, EventArgs e)
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

                DataTable productTable = (DataTable)dataGridView3.DataSource;
                productTable.Clear();
                OracleDataAdapter productTableAdapter = new OracleDataAdapter("SELECT * FROM product", connection);
                productTableAdapter.Fill(productTable);

                dataGridView1.DataSource = productTable;
                MessageBox.Show("Added successfully!");
            }

            else
            {
                MessageBox.Show("Error Adding.");
            }
        }
        int getAdminId()
        {
            string userType = comboBox2.Text;
            string idCurrent = textBox12.Text;

            string tableName = "";
            string tableId = "";

            switch (userType)
            {
                case "Shopkeeper":
                    tableName = "shopkeeper";
                    tableId = "shopkeeperId";

                    break;
                case "Customer":
                    tableName = "customer";
                    tableId = "customerId";

                    break;
            }

            int currentAdminId = -1;

            OracleCommand command2 = new OracleCommand($"SELECT adminId FROM {tableName} WHERE {tableId} = :idCurrent", connection);

            command2.Parameters.Add(":idCurrent", OracleDbType.Int64).Value = idCurrent;

            if (connection.State == ConnectionState.Closed)
                connection.Open();
            OracleDataReader reader = command2.ExecuteReader();
            if (reader.Read())
                currentAdminId = reader.GetInt32(0);

            connection.Close();
            return currentAdminId;
        }
        private void button_WOC6_Click(object sender, EventArgs e)
        {
            try
            {
                string id = textBox17.Text;
                string idCurrent = textBox12.Text;
                string username = textBox16.Text;
                string password = textBox15.Text;
                string userType = comboBox2.Text;
                int currentAdminId = getAdminId();
                if (currentAdminId == -1) MessageBox.Show("can't get admin id");


                string tableName = "";
                string tableId = "";

                switch (userType)
                {
                    case "Shopkeeper":
                        tableName = "shopkeeper";
                        tableId = "shopkeeperId";

                        break;
                    case "Customer":
                        tableName = "customer";
                        tableId = "customerId";

                        break;
                }

                //string sql = $"UPDATE {tableName} SET {tableId} = :id, username = :username, password = :password, adminid = :currentAdminId WHERE {tableId} = :idCurrent";
                string sql = $"UPDATE {tableName} SET {tableId} = :id, username = :username, password = :password, adminId = :currentAdminId WHERE {tableId} = {idCurrent}";

                OracleCommand command = new OracleCommand(sql, connection);

                command.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                command.Parameters.Add(":username", OracleDbType.Varchar2).Value = username;
                command.Parameters.Add(":password", OracleDbType.Varchar2).Value = password;
                command.Parameters.Add(":currentAdminId", OracleDbType.Int32).Value = currentAdminId;

                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                int rowsInserted = command.ExecuteNonQuery();

                if (rowsInserted > 0)
                {
                    if (userType == "Shopkeeper")
                    {
                        DataTable shopkeeperTable = (DataTable)dataGridView1.DataSource;
                        shopkeeperTable.Clear();
                        OracleDataAdapter shopkeeperTableAdapter = new OracleDataAdapter("SELECT * FROM shopkeeper", connection);
                        shopkeeperTableAdapter.Fill(shopkeeperTable);

                        dataGridView1.DataSource = shopkeeperTable;
                        MessageBox.Show($"{tableName} updated successfully!");
                        connection.Close();

                    }
                    if (userType == "Customer")
                    {
                        DataTable CustomerTable = (DataTable)dataGridView2.DataSource;
                        CustomerTable.Clear();
                        OracleDataAdapter CustomerTableAdapter = new OracleDataAdapter("SELECT * FROM customer", connection);
                        CustomerTableAdapter.Fill(CustomerTable);

                        dataGridView2.DataSource = CustomerTable;
                        MessageBox.Show($"{tableName} updated successfully!");
                        connection.Close();


                    }

                }

                else
                {
                    connection.Close();
                    MessageBox.Show($"Error updating {tableName}");
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblValue.Text = comboBox2.Text;
            lblValue.BackColor = Color.Red;
            WaitSomeTime();
        }

        public async void WaitSomeTime()
        {
            await Task.Delay(200);
            lblValue.BackColor = Color.White;
        }

        private void button_WOC2_Click_1(object sender, EventArgs e)
        {
            try
            {
                string id = textBox12.Text;
                string userType = comboBox2.Text;

                string tableName = "";
                string tableId = "";

                switch (userType)
                {
                    case "Shopkeeper":
                        tableName = "shopkeeper";
                        tableId = "shopkeeperId";

                        break;
                    case "Customer":
                        tableName = "customer";
                        tableId = "customerId";

                        break;
                }

                OracleCommand command = new OracleCommand($"DELETE FROM {tableName} WHERE {tableId} = :id", connection);


                command.Parameters.Add(":id", OracleDbType.Int64).Value = id;

                if (connection.State == ConnectionState.Closed)
                    connection.Open(); int rowsInserted = command.ExecuteNonQuery();
                connection.Close();

                if (rowsInserted > 0)
                {
                    if (userType == "Shopkeeper")
                    {
                        // Refill the DataTable object for the shopkeeper table using the OracleDataAdapter object
                        DataTable shopkeeperTable = (DataTable)dataGridView1.DataSource;
                        shopkeeperTable.Clear();
                        OracleDataAdapter shopkeeperAdapter = new OracleDataAdapter("SELECT * FROM shopkeeper", connection);
                        shopkeeperAdapter.Fill(shopkeeperTable);

                        // Update the DataSource property of the DataGridView control for the shopkeeper table to reflect the changes
                        dataGridView1.DataSource = shopkeeperTable;
                        MessageBox.Show("Deleted successfully!");
                    }
                    if (userType == "Customer")
                    {

                        DataTable customerTable = (DataTable)dataGridView2.DataSource;
                        customerTable.Clear();
                        OracleDataAdapter customerAdapter = new OracleDataAdapter("SELECT * FROM customer", connection);
                        customerAdapter.Fill(customerTable);

                        dataGridView2.DataSource = customerTable;
                        MessageBox.Show("Deleted successfully!");
                    }
                }
                else
                {
                    MessageBox.Show("Error Deleting.");
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("You can't delete Parent key having childs! \n First delete all child of this then Delete!\n" + ex.Message);
            }
        }

        private void button_WOC5_Click(object sender, EventArgs e)
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

                DataTable productTable = (DataTable)dataGridView3.DataSource;
                productTable.Clear();
                OracleDataAdapter productTableAdapter = new OracleDataAdapter("SELECT * FROM product", connection);
                productTableAdapter.Fill(productTable);

                dataGridView3.DataSource = productTable;
                MessageBox.Show("Product deleted successfully!");
            }

            else
            {
                MessageBox.Show("Error deleting Product.");
            }

        }

        private void button_WOC4_Click(object sender, EventArgs e)
        {
            string productid = ID.Text;
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

                DataTable productTable = (DataTable)dataGridView3.DataSource;
                productTable.Clear();
                OracleDataAdapter productTableAdapter = new OracleDataAdapter("SELECT * FROM product", connection);
                productTableAdapter.Fill(productTable);
                dataGridView3.DataSource = productTable;
                MessageBox.Show("Product updated successfully!");
            }

            else
            {
                MessageBox.Show("Error updated Product.");
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

 


