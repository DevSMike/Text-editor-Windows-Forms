using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace WindowsFormsApp1
{
   
    public partial class Form2 : Form
    {
        public List<string> links = new List<string>();
        string openFile = String.Empty;
        public Form2()
        {
            InitializeComponent();
            autocompleteMenu1.Items = File.ReadAllLines("cs-reserv-list.dicr");
        }

        private void fastColoredTextBox1_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            string text = fastColoredTextBox1.Text;
            string[] lines = text.Split('\n');
            label1.Text = "Cтрок: "+ lines.Length.ToString();
            label2.Text = "Символов: " + text.Length.ToString();
          
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CSshar source code (*.cs)| *.cs | C++ source code (*.cpp)| *.cpp| Header source code (*.h)| *.h| C cource code (*.c)|*.c";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            fastColoredTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);
            openFile = openFileDialog1.FileName;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
           

          
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "CSshar source code (*.cs)| *.cs";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            File.WriteAllText(saveFileDialog1.FileName, fastColoredTextBox1.Text);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
