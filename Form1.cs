using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TranslatorWinForms
{
    public delegate void MyDelegate(bool data);

    public partial class Translator : Form
    {
        public string[] words;
        public string answer="";
        public string result = "None";
        public string trueAnswer;
        public bool createdForm2 = false;
        //public int[] correctness;
        public int index;
        public bool clickedNewWord = false;
        public static string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\Oleh\\Desktop\\MyWordsMdb.mdb;";
        //private OleDbConnection MyConnection;
        public Translator()
        {
            InitializeComponent();
            Connector.str_connect = connectString;
            //MyConnection = new OleDbConnection(connectString);
            //MyConnection.Open();
            
        }
        void func(bool createdOrNot)
        {
            createdForm2 = createdOrNot;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (clickedNewWord)
            {
                try
                {
                    using (OleDbConnection connection = new OleDbConnection(connectString))
                    {
                        connection.Open();
                        if (string.Equals(trueAnswer, answer))
                        {
                            result = "Result: true!";
                            label2.Text = result;
                            string correctnessRequest = "UPDATE Words SET w_correct_repetitions=w_correct_repetitions+1 WHERE w_id=" + index.ToString();
                            OleDbCommand correctnessCommand = new OleDbCommand(correctnessRequest, connection);
                            correctnessCommand.ExecuteNonQuery();
                            string getQuantity = "SELECT w_correct_repetitions FROM Words WHERE w_id=" + index.ToString();
                            OleDbCommand getQuantityCommand = new OleDbCommand(getQuantity, connection);
                            int correctness = Convert.ToInt32(getQuantityCommand.ExecuteScalar());
                            if (correctness >= 3)
                            {
                                label2.Text = "Result: true!\nDelete this word?";
                                deleteWord.Visible = true;
                            }

                        }
                        else
                        {
                            result = "Result: false!\nRight answer: " + trueAnswer;
                            label2.Text = result;
                            string correctnessRequest = "UPDATE Words SET w_correct_repetitions=w_correct_repetitions-1 WHERE w_id=" + index.ToString();
                            OleDbCommand correctnessCommand = new OleDbCommand(correctnessRequest, connection);
                            correctnessCommand.ExecuteNonQuery();

                        }
                        connection.Close();
                        clickedNewWord = false;
                    }
                        
                }
                catch
                {
                    label2.Text = "Error!\nWe are going to fix it!";
                }
            }
            
            
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            answer = textBox1.Text.ToLower().Trim(' ');
        }

        

        private void Button3_Click(object sender, EventArgs e)
        {
            
            if (!createdForm2)
            {
                createdForm2 = true;
                Form2 f2 = new Form2(new MyDelegate(func));
                f2.Show();
            }

        }

        private void NewWord_Click(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectString))
                {
                    connection.Open();
                    string emptyOrNot = "SELECT w_id FROM Words WHERE EXIST (SELECT w_id FROM Words WHERE w_id=1)";
                    OleDbCommand emptyCommand = new OleDbCommand(emptyOrNot, connection);
                    bool NotEmpty = Convert.ToBoolean(emptyCommand.ExecuteScalar());
                    if (NotEmpty)
                    {
                        Random rnd = new Random();
                        string query = "SELECT MAX(w_id) FROM Words";
                        OleDbCommand command = new OleDbCommand(query, connection);
                        int amountOfWords = Convert.ToInt32(command.ExecuteScalar());
                        index = rnd.Next(1, amountOfWords + 1);
                        string questionRequest = "SELECT w_ukrainian FROM Words WHERE w_id=" + index.ToString();
                        OleDbCommand requestCommand = new OleDbCommand(questionRequest, connection);
                        string question = (requestCommand.ExecuteScalar()).ToString();
                        //trueAnswer = words[index - 1];
                        string newText = "Translate (" + question + "): ";
                        label1.Text = newText;
                        string answerRequest = "SELECT w_english FROM Words WHERE w_id=" + index.ToString();
                        OleDbCommand answerCommand = new OleDbCommand(answerRequest, connection);
                        trueAnswer = (answerCommand.ExecuteScalar()).ToString();
                        label2.Text = "Result: ";
                        clickedNewWord = true;
                        deleteWord.Visible = false;
                    }
                    else
                    {
                        label1.Text = "Words are missing!";
                        label2.Text = "You have studied all the words!\nAdd new ones!";
                    }
                    connection.Close();
                }
                
                
            }
            catch
            {
                label2.Text = "Error!\nSorry!";
            }
            textBox1.Text = "";


        }

        private void DeleteWord_Click(object sender, EventArgs e)
        {

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectString))
                {
                    connection.Open();
                    string deleteRequest = "DELETE FROM Words WHERE w_id=" + index.ToString();
                    OleDbCommand deleteCommand = new OleDbCommand(deleteRequest, connection);
                    deleteCommand.ExecuteNonQuery();
                    string updateAfterDeleteRequest = "UPDATE Words SET w_id=w_id-1 WHERE w_id>" + index.ToString();
                    OleDbCommand updateAfterDeleteCommand = new OleDbCommand(updateAfterDeleteRequest, connection);
                    updateAfterDeleteCommand.ExecuteNonQuery();

                    deleteWord.Visible = false;
                    string emptyOrNot = "SELECT w_id FROM Words WHERE EXIST (SELECT w_id FROM Words WHERE w_id=1)";
                    OleDbCommand command = new OleDbCommand(emptyOrNot, connection);
                    bool empty = Convert.ToBoolean(command.ExecuteScalar());
                    if (!empty)
                    {
                        label1.Text = "Words are missing!";
                        label2.Text = "You have studied all the words!\nAdd new ones!";
                    }
                    else
                    {
                        label1.Text = "Choose a new word!";
                        label2.Text = "Result: deletion is successful!";
                    }
                    textBox1.Text = "";
                    connection.Close();
                }

            }
            catch
            {
                label2.Text = "Error!\nChoose a word!";
            }
            
        }

        private void Translator_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MyConnection.Close();
        }
    }

    
}
