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
using System.Collections;

namespace Proyecto1
{
    public partial class Form1 : Form
    {
        static String filePath = string.Empty;
        private char c;
        private String fuente = "";
        private String auxLex = "";
        private int estado = 0;
        ArrayList listaLexema = new ArrayList();
        ArrayList listaToken = new ArrayList();
        static bool Sim = false;
        static String ConActual = "";
        static String ErActual = "";
        private String auxEr = "";
        static int Imacro = 0;
        static int Fmacro = 0;
        ArrayList Conjunto = new ArrayList();
        ArrayList DatCon = new ArrayList();
        internal static ArrayList ER = new ArrayList();
        internal static ArrayList NameER = new ArrayList();
        public Form1()
        {
            InitializeComponent();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\Users\\Davis\\Documents";
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
            for (int i = 0; i < fuente.Length; i++)
            {
                c = fuente[i];
                switch (estado)
                {
                    case 1:
                        {
                            if (Char.IsLetter(c))
                            {
                                auxLex += c;
                                estado = 1;
                            }
                            else if (Char.IsDigit(c))
                            {
                                auxLex += c;
                                estado = 1;
                            }
                            else if (auxLex.Equals("CONJ"))
                            {
                                AddList(auxLex, "Palabra Reservada");
                                estado = 0;
                                i = i - 1;
                            }
                            else
                            {
                                ErActual = auxLex;
                                AddList(auxLex, "Id");
                                estado = 0;
                                i = i - 1;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (Char.IsLetter(c))
                            {
                                auxLex += c;
                                estado = 3;
                            } else if (c == (char)34) {
                                auxEr = Char.ToString((char)39);
                                estado = 15;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (Char.IsLetter(c))
                            {
                                auxLex += c;
                                estado = 3;
                            }
                            else if (Char.IsDigit(c))
                            {
                                auxLex += c;
                                estado = 3;
                            }
                            else
                            {
                                ConActual = auxLex;
                                AddList(auxLex, "Id conjunto");
                                estado = 0;
                                i = i - 1;
                            }
                            break;
                        }
                    case 4:
                        {
                            if (c == '>')
                            {
                                auxLex += c;
                                AddList(auxLex, "Asignacion");
                                estado = 5;
                            }
                            else
                            {
                                auxLex += c;
                                AddList(auxLex, "error");
                                estado = 0;
                            }
                            break;
                        }
                    case 5:
                        {
                            if (Char.IsLetter(c))
                            {
                                auxLex += c;
                                estado = 6;
                            }
                            else if (Char.IsDigit(c))
                            {
                                auxLex += c;
                                estado = 6;
                            }
                            else if (c == (char)34)
                            {
                                auxLex += c;
                                auxEr = Char.ToString((char)39);
                                estado = 12;
                            }
                            else if (c == '%')
                            {
                                NameER.Add(ErActual);
                                ER.Add(auxEr);
                                auxEr = "";
                                AddList(auxLex, "ER");
                                estado = 0;
                                i = i - 1;
                            }
                            else if (c == '.')
                            {
                                auxLex += c;
                                NameER.Add(ErActual);
                                ER.Add("and");
                                estado = 5;
                            }
                            else if (c == '|')
                            {
                                auxLex += c;
                                NameER.Add(ErActual);
                                ER.Add("or");
                                estado = 5;
                            }
                            else if (c == '?')
                            {
                                auxLex += c;
                                NameER.Add(ErActual);
                                ER.Add("?");
                                estado = 5;
                            }
                            else if (c == '*')
                            {
                                auxLex += c;
                                NameER.Add(ErActual);
                                ER.Add("*");
                                estado = 5;
                            }
                            else if (c == '+')
                            {
                                auxLex += c;
                                NameER.Add(ErActual);
                                ER.Add("+");
                                estado = 5;
                            }
                            else if (c == '{')
                            {
                                auxLex += c;
                                estado = 14;
                            }
                            else if (c == ';')
                            {
                                AddList(auxLex, "ER");
                                AddList(";", "punto y coma");
                                estado = 0;
                            }
                            else if (esEspacio(c))
                            {
                                estado = 5;
                            }
                            else
                            {
                                auxEr += c;
                                auxLex += c;
                                estado = 6;
                            }
                            break;
                        }
                    case 6:
                        {
                            if (c == ',')
                            {
                                Conjunto.Add(ConActual);
                                DatCon.Add(auxLex);
                                AddList(auxLex, "simbolo conjunto");
                                AddList(",", "coma");
                                estado = 5;
                            }
                            else if (c == '~')
                            {
                                if (Char.IsDigit(auxLex[0]) || Char.IsLetter(auxLex[0]))
                                {
                                    Sim = false;
                                }
                                else
                                {
                                    Sim = true;
                                }
                                Imacro = (int)auxLex[0];
                                AddList(auxLex, "macro conjunto");
                                AddList("~", "virgulilla");
                                estado = 7;
                            }
                            else if (esEspacio(c))
                            {
                                estado = 6;
                            }
                            else
                            {
                                Conjunto.Add(ConActual);
                                DatCon.Add(auxLex);
                                AddList(auxLex, "simbolo conjunto");
                                i = i - 1;
                                estado = 0;
                            }
                            break;
                        }
                    case 7:
                        {
                            if (esEspacio(c))
                            {
                                estado = 7;
                            }
                            else
                            {
                                auxLex += c;
                                Fmacro = (int)auxLex[0];
                                for (int j = Imacro; j <= Fmacro; j++)
                                {
                                    if (Sim == true)
                                    {
                                        char v = (char)j;
                                        if (Char.IsDigit(v) || Char.IsLetter(v))
                                        {
                                        }
                                        else
                                        {
                                            Conjunto.Add(ConActual);
                                            DatCon.Add(Char.ToString(v));
                                        }
                                    }
                                    else
                                    {
                                        char v = (char)j;
                                        Conjunto.Add(ConActual);
                                        DatCon.Add(Char.ToString(v));
                                    }
                                }
                                AddList(auxLex, "macro conjunto");
                                estado = 0;
                            }
                            break;
                        }
                    case 8:
                        {
                            if (c == '/')
                            {
                                auxLex += c;
                                estado = 8;
                            }
                            else if (c == '\n' || c == '\t')
                            {
                                AddList(auxLex, "comentario");
                                estado = 0;
                            }
                            else
                            {
                                auxLex += c;
                                estado = 8;
                            }
                            break;
                        }
                    case 9:
                        {
                            if (c == '!')
                            {
                                auxLex += c;
                                estado = 10;
                            }
                            else
                            {
                                auxLex += c;
                                AddList(auxLex, "error");
                                estado = 0;
                            }
                            break;
                        }
                    case 10:
                        {
                            if (c == '!')
                            {
                                auxLex += c;
                                estado = 11;
                            }else if (c=='\n')
                            {
                                estado = 10;
                            }
                            else
                            {
                                auxLex += c;
                                estado = 10;
                            }
                            break;
                        }
                    case 11:
                        {
                            if (c == '>')
                            {
                                auxLex += "&gt;";
                                AddList(auxLex, "comentario miltilinea");
                                estado = 0;
                            }
                            else
                            {
                                auxLex += c;
                                AddList(auxLex, "error");
                                estado = 0;
                            }
                            break;
                        }
                    case 12:
                        {
                            if (c == (char)34)
                            {
                                auxEr += Char.ToString((char)39);
                                NameER.Add(ErActual);
                                ER.Add(auxEr);
                                auxEr = "";
                                auxLex += c;
                                estado = 5;
                                
                            }
                            else
                            {
                                auxEr += c;
                                auxLex += c;
                                estado = 12;
                            }
                            break;
                        }
                    case 13:
                        {
                            if (c == '%')
                            {
                                auxLex += c;
                                estado = 13;
                            }
                            else if (c == '\n' || c == '\t')
                            {
                                estado = 13;
                            }
                            else if (auxLex.Equals("%%%%",StringComparison.OrdinalIgnoreCase))
                            {
                                AddList(auxLex, "Division entre partes");
                                estado = 0;
                                i = i - 1;
                            }
                            else
                            {
                                auxLex += c;
                                AddList(auxLex, "error");
                                estado = 0;
                            }
                            break;
                        }
                    case 14:
                        {
                            if (c == '}')
                            {
                                NameER.Add(ErActual);
                                ER.Add(auxEr);
                                auxEr = "";
                                auxLex += c;
                                estado = 5;
                            }
                            else
                            {
                                auxEr += c;
                                auxLex += c;
                                estado = 14;
                            }
                            break;
                        }
                    case 15:
                        {
                            if (c == (char)34)
                            {
                                AddList(auxLex, "Lexema a evaluar");
                                estado = 0;
                            }
                            else
                            {
                                auxLex += c;
                                estado = 15;
                            }
                            break;
                        }
                    case 0:
                        {
                            if (Char.IsLetter(c))
                            {
                                auxLex += c;
                                estado = 1;
                            }
                            else if (c == ':')
                            {
                                auxLex += c;
                                AddList(auxLex, "dos puntos");
                                estado = 2; 
                            }
                            else if (c == ';')
                            {
                                auxLex += c;
                                AddList(auxLex, "punto y coma");
                                estado = 0;
                            }
                            else if (c == '-')
                            {
                                auxLex += c;
                                estado = 4;
                            }
                            else if (c == '{')
                            {
                                auxLex += c;
                                AddList(auxLex, "llave izq");
                                estado = 0;
                            }
                            else if (c == '/')
                            {
                                auxLex += c;
                                estado = 8;
                            }
                            else if (c == '<')
                            {
                                auxLex += "&lt;";
                                estado = 9;
                            }
                            else if (c == '%')
                            {
                                auxLex += c;
                                estado = 13;
                            }
                            else if (esEspacio(c))
                            {
                                estado = 0;
                            }
                            else if (c == '}')
                            {
                                auxLex += c;
                                AddList(auxLex, "llave der");
                                estado = 0;
                            }
                            else
                            {
                                auxLex += c;
                                AddList(auxLex, "error");
                                estado = 0;
                            }
                            break;
                        }
                }
            }
        }
        private void AddList(String lex, String token)
        {
            listaLexema.Add(lex);
            listaToken.Add(token);
            auxLex = "";
        }
        private bool esEspacio(char c)
        {
            return c == '\n' || c == '\t' || c == ' ';
        }
        private void ReporteHTML() {
            string dir = @"Reporte_HTML_201700972.html";
            if (!File.Exists(dir))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(dir))
                {
                    sw.WriteLine("");
                    sw.WriteLine("<!DOCTYPE HTML5>");
                    sw.WriteLine("<html>");
                    sw.WriteLine("<head>");
                    sw.WriteLine("<title>Reporte_HTML_201700972</title>");
                    sw.WriteLine("</head>");
                    sw.WriteLine("<body>");
                    sw.WriteLine("<table border=\"1\">");
                    sw.WriteLine("<caption>Reporte Archivo</caption>");
                    sw.WriteLine("<tr>");
                    sw.WriteLine("<th>Token</th>");
                    sw.WriteLine("<th>Lexema</th>");
                    sw.WriteLine("</tr>");
                    for (int i = 0; i < listaLexema.Count; i++)
                    {
                        sw.WriteLine("<tr>");
                        sw.WriteLine("<th>"+ listaToken[i] + "</th>");
                        sw.WriteLine("<th>"+ listaLexema[i] + "</th>");
                        sw.WriteLine("</tr>");
                    }
                    sw.WriteLine("</table>");
                    sw.WriteLine("</body>");
                    sw.WriteLine("</html>");
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //Clean
            selector.Items.Clear();
            Sim = false;
            Imacro = 0;
            Fmacro = 0;
            estado = 0;
            auxLex = "";
            auxEr = "";
            ConActual = "";
            ErActual = "";
            listaLexema.Clear();
            listaToken.Clear();
            Conjunto.Clear();
            DatCon.Clear();
            ER.Clear();
            NameER.Clear();
            int a = tabControl1.SelectedIndex;
            if (a == 0)
            {
                fuente = richTextBox1.Text;
            }
            else if (a == 1)
            {
                fuente = richTextBox2.Text;
            }
            else
            {
                fuente = richTextBox3.Text;
            }
            fuente = fuente.Trim();
            //inicio
            if (fuente.Length == 0)
            {
               MessageBox.Show("El cuadro de entrada no contiene caracteres a evaluar. ","vacio");

            }
            else
            {
                iniciarProceso();
                for (int i = 0; i < NameER.Count; i++)
                {
                    if (i == 0)
                    {
                        
                        selector.Items.Add(NameER[0]);
                    }
                    else if (!NameER[i].Equals(NameER[i - 1]))
                    {
                        selector.Items.Add(NameER[i]);
                    }
                }
                //Mostrar conjuntos
                ArrayList ayudaCon = new ArrayList();
            for (int i = 0; i < Conjunto.Count; i++)
            {
                if (i == 0)
                {
                    ayudaCon.Add(Conjunto[0]);
                }
                else if (!Conjunto[i].Equals(Conjunto[i - 1]))
                {
                    ayudaCon.Add(Conjunto[i]);
                }
            }
            String auxiliarCon = "";
            for (int i = 0; i < ayudaCon.Count; i++)
            {
                String auxCon = "";
                for (int j = 0; j < Conjunto.Count; j++)
                {
                    if (Conjunto[j].Equals(ayudaCon[i]))
                    {
                        auxCon += DatCon[j] + ", ";
                    }
                }
                auxiliarCon += ayudaCon[i] + "----->" + auxCon + "\n";
            }
            MostrarConjuntos.Text=auxiliarCon;
            ayudaCon.Clear();
        }
    }

        private void button2_Click(object sender, EventArgs e)
        {
            ReporteHTML();
        }

        private void MostrarConjuntos_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thompson v = new Thompson();
            v.Inicio(selector.SelectedItem.ToString());
        }
    }
}
