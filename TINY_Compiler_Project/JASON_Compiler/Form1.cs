using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TINY_Compiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height;
            this.Location = new Point(0, 0);
            this.Size = new Size(w, h);
            label4.Text = DateTime.Now.ToLongTimeString();
            label5.Text = DateTime.Now.ToShortDateString();
            timer1.Start();
        }


        private void Run(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();    
            TINY_Compiler.TINY_Scanner.Tokens.Clear();
            Errors.Error_List.Clear();
            textBox2.Clear();
            //string Code=textBox1.Text.ToLower();
            string Code = textBox1.Text;
            TINY_Compiler.Start_Compiling(Code);
            PrintTokens();
            //PrintLexemes();
            if (Errors.Error_List.Count == 0)
            {
                textBox2.ForeColor = Color.Green;
                textBox2.Text = "Build Succeeded !!!!!\n";
            }
            else
            {
                textBox2.ForeColor = Color.Red;
                PrintErrors();
                string message = $"                          Build failed !!!           \n                          num of errors = {Errors.Error_List.Count}";
                string title = "Error Message";
                MessageBox.Show(message, title, MessageBoxButtons.YesNo);
            }
            
        }

        void PrintTokens()
        {
            for (int i = 0; i < TINY_Compiler.TINY_Scanner.Tokens.Count; i++)
            {
               dataGridView1.Rows.Add(TINY_Compiler.TINY_Scanner.Tokens.ElementAt(i).lex, TINY_Compiler.TINY_Scanner.Tokens.ElementAt(i).token_type);
            }
        }

        void PrintErrors()
        {
            textBox2.Text += " Build failed !!!!!!!\r\n Errors : \r\n";
            for (int i=0; i<Errors.Error_List.Count; i++)
            {

                textBox2.Text += $"  {i + 1}- ";
                textBox2.Text += Errors.Error_List[i];
                textBox2.Text += "\r\n";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            TINY_Compiler.TokenStream.Clear();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private void Build(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            Errors.Error_List.Clear();
            textBox2.Clear();
            //string Code=textBox1.Text.ToLower();
            string Code = textBox1.Text;
            TINY_Compiler.Start_Compiling(Code);
            //   PrintLexemes();
            if (Errors.Error_List.Count == 0)
            {
                textBox2.ForeColor = Color.Green;
                textBox2.Text = "Build Succeeded !!!!!\n";
            }
            else
            {
                textBox2.ForeColor = Color.Red;
                PrintErrors();
                string message = $"                          Build failed !!!           \n                          num of errors = {Errors.Error_List.Count}";
                string title = "Error Message";
                MessageBox.Show(message, title, MessageBoxButtons.YesNo);

            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            //label4.Text= DateTime.Now.ToLongTimeString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToLongTimeString();
            label5.Text = DateTime.Now.ToShortDateString();
            timer1.Start();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }


        /*  void PrintLexemes()
{
for (int i = 0; i < TINY_Compiler.Lexemes.Count; i++)
{
textBox2.Text += TINY_Compiler.Lexemes.ElementAt(i);
textBox2.Text += Environment.NewLine;
}
}*/
    }
}
