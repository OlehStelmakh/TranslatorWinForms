using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public string answer;
        public string result = "None";
        public string trueAnswer;
        public bool createdForm2 = false;
        public int[] correctness;
        public int index;
        public bool clickedNewWord = false;
        public Translator()
        {
            InitializeComponent();
            
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
                    if (string.Equals(trueAnswer, answer))
                    {
                        result = "Result: true!";
                        label2.Text = result;
                        correctness[index / 2] += 1;
                        if (correctness[index / 2] >= 3)
                        {
                            label2.Text = "Result: true!\nDelete this word?";
                            deleteWord.Visible = true;
                        }

                    }
                    else
                    {
                        result = "Result: false!\nRight answer: " + trueAnswer;
                        label2.Text = result;
                        correctness[index / 2] -= 1;
                        
                    }
                    clickedNewWord = false;
                }
                catch
                {
                    label2.Text = "Error!\nDownload the word!";
                }
            }
            
            
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            answer = textBox1.Text.ToLower().Trim(' ');
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            StreamReader stream = new StreamReader("C:/Users/Oleh/Desktop/Words1.txt", System.Text.Encoding.UTF8);
            string outText = stream.ReadToEnd();
            words = outText.Split('-');
            stream.Close();
            for (int i=0;i<words.Length;i++)
            {
                words[i] = words[i].Trim(' ');
            }
            correctness = new int[words.Length/2];
            for (int i=0;i<words.Length/2;i++)
            {
                correctness[i] = 0;
            }
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
            Random rnd = new Random();

            try
            {
                index = rnd.Next(0, words.Length / 2) * 2 + 1;
                string question = words[index];
                trueAnswer = words[index - 1];
                string newText = "Translate (" + question + "): ";
                label1.Text = newText;
                label2.Text = "Result: ";
                clickedNewWord = true;
                deleteWord.Visible = false;
                
            }
            catch
            {
                label2.Text = "Error!\nYou must download the words!";
            }
            textBox1.Text = "";


        }

        private void DeleteWord_Click(object sender, EventArgs e)
        {
            try
            {
                var temp = new List<string>(words);
                temp.RemoveAt(index - 1);
                temp.RemoveAt(index - 1);
                words = temp.ToArray<string>();

                var secondTemp = new List<int>(correctness);
                secondTemp.RemoveAt(index / 2);
                correctness = secondTemp.ToArray<int>();

                deleteWord.Visible = false;
                string writePath = "C:/Users/Oleh/Desktop/Words1.txt";
                StreamWriter writeNewWords = new StreamWriter(writePath, false, System.Text.Encoding.UTF8);
                for (int i=0;i<words.Length;i++)
                {
                    if (i!=words.Length-1)
                    {
                        writeNewWords.Write(words[i] + "-");
                    }
                    else
                    {
                        writeNewWords.Write(words[i]);
                    }
                    
                }
                writeNewWords.Close();
                if (correctness.Length==0 && words.Length==0)
                {
                    label1.Text = "Words are missing!";
                    label2.Text = "You have successfully studied all the words!\nAdd new ones!";
                }
                else
                {
                    label1.Text = "Choose a new word!";
                    label2.Text = "Result: deletion is successful!";
                }
                textBox1.Text = "";

            }
            catch
            {
                label2.Text = "Error!\nChoose a word!";
            }
            
        }

        
    }

    
}
