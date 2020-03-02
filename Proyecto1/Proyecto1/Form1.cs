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

namespace Proyecto1
{
    public partial class Form1 : Form
    {
        static String filePath = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            int a = tabControl1.SelectedIndex;
            if (a == 0)
            {
                richTextBox1.Text = fileContent;
            }
            else if (a == 1)
            {
                richTextBox2.Text = fileContent;
            }
            else {
                richTextBox3.Text = fileContent;
            }
            //MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int a = tabControl1.SelectedIndex;
            if (a == 0)
            {
                File.WriteAllText(filePath, richTextBox1.Text);
            }
            else if (a == 1)
            {
                File.WriteAllText(filePath, richTextBox2.Text);
            }
            else
            {
                File.WriteAllText(filePath, richTextBox3.Text);
            }
            MessageBox.Show("Archivo Guardado","Mensaje");
            
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void iniciarProceso() {

        }
    }
}
