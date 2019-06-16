using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TranslatorWinForms
{
    public partial class Form2 : Form
    {
        public string english_text;
        public string ukr_text;
        //public static string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\Oleh\\Desktop\\MyWordsMdb.mdb;";
        //private OleDbConnection SecondConnection;
        public Form2()
        {
            InitializeComponent();
            //SecondConnection = new OleDbConnection(connectString);
            //SecondConnection.Open();
        }

        MyDelegate forBoolCreated;
        public Form2(MyDelegate sender)
        {
            InitializeComponent();
            forBoolCreated = sender;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            english_text = textBox1.Text;
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            ukr_text = textBox2.Text;
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            //string writePath = "C:/Users/Oleh/Desktop/Words1.txt";
            //StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.UTF8);
            if (String.IsNullOrWhiteSpace(english_text) || String.IsNullOrWhiteSpace(ukr_text))
            {
                label3.Text = "Line(s) is null or whitespace!";
            }
            else
            {
                string connectionString = Connector.str_connect;
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    string emptyOrNot = "SELECT w_id FROM Words WHERE EXIST (SELECT w_id FROM Words WHERE w_id=1)";
                    OleDbCommand command = new OleDbCommand(emptyOrNot, connection);
                    bool NotEmpty = Convert.ToBoolean(command.ExecuteScalar());
                    int currentMaxId = 0;
                    if (NotEmpty)
                    {
                        string currentMaxIdRequest = "SELECT MAX(w_id) FROM Words";
                        OleDbCommand currentMaxIdCommand = new OleDbCommand(currentMaxIdRequest, connection);
                        currentMaxId = Convert.ToInt32(currentMaxIdCommand.ExecuteScalar());
                    }
           
                    string w_eng = english_text.ToLower().Trim(' ');
                    string w_ukr = ukr_text.ToLower().Trim(' ');
                    string insertRequest = $"INSERT INTO Words (w_id, w_english, w_ukrainian, w_correct_repetitions) VALUES ({currentMaxId+1}, '{w_eng}', '{w_ukr}', 0)";
                    OleDbCommand insertCommand = new OleDbCommand(insertRequest, connection);
                    insertCommand.ExecuteNonQuery();
                    connection.Close();
                }

                label3.Text = "The word was added!";
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            forBoolCreated(false);
            //SecondConnection.Close();
        }
    }
}
