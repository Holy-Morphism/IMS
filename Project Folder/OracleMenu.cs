using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApplication1
{
    public partial class OracleMenu : Form
    {
        public static string loginDetails ;

        public static string server;
        public static string username;
        public static string password;

        public OracleMenu()
        {
            InitializeComponent();
        }

        private void button_WOC2_Click(object sender, EventArgs e)
        {
           
             server = textBox2.Text;
             username = textBox3.Text;
             password = textBox1.Text;


        OracleMenu.ActiveForm.Hide();
            MainMenu f = new MainMenu();
            f.Show();
        }

        private void button_WOC1_Click(object sender, EventArgs e)
        {
            server = "1521/xe";
            username = "F219281";
            password = "abc@1234";

            OracleMenu.ActiveForm.Hide();
            MainMenu f = new MainMenu();
            f.Show();
        }
    }
}
