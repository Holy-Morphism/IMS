using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Runtime.InteropServices;


namespace WindowsFormsApplication1
{
    public partial class MainMenu : Form
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

        OracleConnection con;
        public MainMenu()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 40, 40));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string conStr =LoginMenu.loginDetails;
            con = new OracleConnection(conStr);
            updateGrid();

            OracleCommand command = new OracleCommand("DELETE FROM cart_item", con);
            if (con.State == ConnectionState.Closed)
                con.Open(); int rowsInserted = command.ExecuteNonQuery();
            con.Close();
        }

        private void updateGrid()
        {
            if (con.State == ConnectionState.Closed)
                con.Open(); OracleCommand getEmps = con.CreateCommand();
            getEmps.CommandText = "SELECT * FROM DEPT";
            getEmps.CommandType = CommandType.Text;
            OracleDataReader empDR = getEmps.ExecuteReader();
            DataTable empDT = new DataTable();
            empDT.Load(empDR);
            //dataGridView1.DataSource = empDT;

            con.Close();

        }



        private void button_WOC2_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void button_WOC1_Click(object sender, EventArgs e)
        {
            MainMenu.ActiveForm.Hide();
            LoginMenu f = new LoginMenu();
            f.Show();
        }

        private void button_WOC3_Click(object sender, EventArgs e)
        {
            MainMenu.ActiveForm.Hide();
            RegisterMenu f = new RegisterMenu();
            f.Show();
        }

        private void gradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
