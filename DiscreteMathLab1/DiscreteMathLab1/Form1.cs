using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscreteMathLab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        private void button8_Click(object sender, EventArgs e)
        { // SWAP
            String buffer = textBox1.Text;
            textBox1.Text = textBox2.Text;
            textBox2.Text = buffer;
            Solution.Input1 = textBox1.Text.ToCharArray();
            Solution.Input2 = textBox2.Text.ToCharArray();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = checkInputCorrectness(textBox1.Text);
            Solution.Input1 = textBox1.Text.ToCharArray();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = checkInputCorrectness(textBox2.Text);
            Solution.Input2 = textBox2.Text.ToCharArray();
        }

        private String checkInputCorrectness(String input)
        {
            String filtered = "";
            foreach (Char c in input.ToCharArray())
                if (Char.IsLetter(c) && !filtered.Contains(c)) 
                    filtered += Char.ToLower(c);
            return filtered;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = Solution.Task1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = Solution.Task2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Text = Solution.Task3();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Text = Solution.Task4();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox3.Text = Solution.Task5();
        }
    }
}
