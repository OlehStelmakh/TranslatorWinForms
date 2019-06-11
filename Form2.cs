using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public Form2()
        {
            InitializeComponent();
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

            string writePath = "C:/Users/Oleh/Desktop/Words1.txt";
            StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.UTF8);
            if (String.IsNullOrWhiteSpace(english_text) || String.IsNullOrWhiteSpace(ukr_text))
            {
                label3.Text = "Line(s) is null or whitespace!";
            }
            else
            {
                sw.Write("-" + english_text.ToLower().Trim(' ') + "-" + ukr_text.ToLower().Trim(' '));
                label3.Text = "The word was added!";
                textBox1.Text = "";
                textBox2.Text = "";
            }
            sw.Close();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            forBoolCreated(false);
        }
    }
}
