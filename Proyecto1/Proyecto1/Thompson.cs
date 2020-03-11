using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto1
{

    class Thompson
    {
        static int NumO;
        static int ConjuntoChar;
        String ruta;
        StringBuilder grafo;
        ArrayList Aux = new ArrayList();
        static ArrayList ListaTerminales = new ArrayList();
        static ArrayList ListaThom = new ArrayList();
        static ArrayList ListaMueve = new ArrayList();
        static List<List<String>> ListaConjun = new List<List<String>>();
        public void Inicio(String filtro)
        {
            NumO = 0;
            ConjuntoChar = 65;
            ruta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ListaTerminales.Clear();
            Aux.Clear();
            ListaConjun.Clear();
            ListaMueve.Clear();
            ListaThom.Clear();
            //llenar aux con la expresion regular
            for (int i = 0; i < Form1.ER.Count; i++)
            {
                if (Form1.NameER[i].Equals(filtro))
                {
                    Aux.Add(Form1.ER[i]);
                }
            }
            //Cargar lista con thompson
            CargarThompson();
            //graficar
            graficar();
            //inicializar cerradura
            ListaConjun.Add(new List<string>());
            ListaConjun[0].Add(((char)ConjuntoChar).ToString());
            Cerradura(0);
            //imprimir cerradura
            for (int i = 0; i < ListaConjun[0].Count; i++)
            {
                Console.WriteLine(ListaConjun[0][i]);
            }
        }
        public void CargarThompson()
        {
            for (int i = 0; i < Aux.Count; i++)
            {
                if (Aux[i].Equals("and"))
                {
                    for (int j = 0; j < ListaThom.Count; j++)
                    {
                        if (ListaThom[j] == null)
                        {
                            ListaMueve[j] = "n";
                            ListaMueve.Insert(j, "n");
                            ListaThom[j] = null;
                            ListaThom.Insert(j, null);
                            break;
                        }
                    }
                    if (ListaThom.Count == 0)
                    {
                        ListaMueve.Insert(0, "n");
                        ListaMueve.Insert(1, "n");
                        ListaThom.Insert(0, null);
                        ListaThom.Insert(1, null);
                    }
                }
                else if (Aux[i].Equals("or"))
                {
                    for (int j = 0; j < ListaThom.Count; j++)
                    {
                        if (ListaThom[j] == null)
                        {
                            ListaMueve[j] = NumO+"n3";
                            ListaMueve.Insert(j, "n");
                            ListaMueve.Insert(j, NumO + "n2");
                            ListaMueve.Insert(j, "n");
                            ListaMueve.Insert(j, NumO + "n1");
                            ListaThom[j] = "ε";
                            ListaThom.Insert(j, null);
                            ListaThom.Insert(j, "ε");
                            ListaThom.Insert(j, null);
                            ListaThom.Insert(j, "ε");
                            break;
                        }
                    }
                    if (ListaThom.Count == 0)
                    {
                        ListaMueve.Insert(0, NumO + "n1");
                        ListaMueve.Insert(1, "n");
                        ListaMueve.Insert(2, NumO + "n2");
                        ListaMueve.Insert(3, "n");
                        ListaMueve.Insert(4, NumO + "n3");
                        ListaThom.Insert(0, "ε");
                        ListaThom.Insert(1, null);
                        ListaThom.Insert(2, "ε");
                        ListaThom.Insert(3, null);
                        ListaThom.Insert(4, "ε");
                    }
                    NumO++;
                }
                else if (Aux[i].Equals("?"))
                {
                    for (int j = 0; j < ListaThom.Count; j++)
                    {
                        if (ListaThom[j] == null)
                        {
                            ListaMueve[j] = "n5";
                            ListaMueve.Insert(j, "n");
                            ListaMueve.Insert(j, "n4");
                            ListaThom[j] = "ε";
                            ListaThom.Insert(j, null);
                            ListaThom.Insert(j, "ε");
                            break;
                        }
                    }
                    if (ListaThom.Count == 0)
                    {
                        ListaMueve.Insert(0, "n4");
                        ListaMueve.Insert(1, "n");
                        ListaMueve.Insert(2, "n5");
                        ListaThom.Insert(0, "ε");
                        ListaThom.Insert(1, null);
                        ListaThom.Insert(2, "ε");
                    }
                }
                else if (Aux[i].Equals("+"))
                {
                    for (int j = 0; j < ListaThom.Count; j++)
                    {
                        if (ListaThom[j] == null)
                        {
                            ListaMueve[j] ="n7";
                            ListaMueve.Insert(j, "n");
                            ListaMueve.Insert(j, "n6");
                            ListaThom[j] = "ε";
                            ListaThom.Insert(j, null);
                            ListaThom.Insert(j, "ε");
                            break;
                        }
                    }
                    if (ListaThom.Count == 0)
                    {
                        ListaMueve.Insert(0, "n6");
                        ListaMueve.Insert(1, "n");
                        ListaMueve.Insert(2, "n7");
                        ListaThom.Insert(0, "ε");
                        ListaThom.Insert(1, null);
                        ListaThom.Insert(2, "ε");
                    }
                }
                else if (Aux[i].Equals("*"))
                {
                    for (int j = 0; j < ListaThom.Count; j++)
                    {
                        if (ListaThom[j] == null)
                        {
                            ListaMueve[j] = "n9";
                            ListaMueve.Insert(j, "n");
                            ListaMueve.Insert(j, "n8");
                            ListaThom[j] = "ε";
                            ListaThom.Insert(j, null);
                            ListaThom.Insert(j, "ε");
                            break;
                        }
                    }
                    if (ListaThom.Count == 0)
                    {
                        ListaMueve.Insert(0, "n8");
                        ListaMueve.Insert(1, "n");
                        ListaMueve.Insert(2, "n9");
                        ListaThom.Insert(0, "ε");
                        ListaThom.Insert(1, null);
                        ListaThom.Insert(2, "ε");
                    }
                }
                else
                {
                    for (int j = 0; j < ListaThom.Count; j++)
                    {
                        if (ListaThom[j] == null)
                        {
                            ListaTerminales.Add(Aux[i]);
                            ListaThom[j] = Aux[i];
                            ListaMueve[j] = j + 2;
                            break;
                        }
                    }
                }
            }
            String N2 = "";
            String N8 = "";
            String N6 = "";
            for (int i = 0; i < ListaMueve.Count; i++)
            {
                try {
                    if (ListaMueve[i].ToString() == "n4")
                    {
                        ListaMueve[i] = (i + 2)+","+(i+4);
                    }
                    else if (ListaMueve[i].ToString() == "n5")
                    {
                        ListaMueve[i] = i + 2;
                    }
                    else if (ListaMueve[i].ToString() == "n6")
                    {
                        ListaMueve[i] = i + 2;
                        N6 = (i + 2).ToString();
                    }
                    else if (ListaMueve[i].ToString() == "n7")
                    {
                        ListaMueve[i] = N6+","+(i + 2);
                        N6 = "";
                    }
                    else if (ListaMueve[i].ToString() == "n8")
                    {
                        String auxN = (i + 2).ToString();
                        for (int j = i; j < ListaMueve.Count ; j++)
                        {
                            if (ListaMueve[j].ToString() == "n9")
                            {
                                auxN = auxN + "," + (j + 2).ToString();
                                N8 = auxN;
                                ListaMueve[i] = auxN;
                                break;
                            }
                        }
                    }
                    else if (ListaMueve[i].ToString() == "n9")
                    {
                        ListaMueve[i] = N8;
                        N8 = "";
                    }
                    else if (ListaMueve[i].ToString()[2] == '1')
                    {
                        for (int j = i; j < ListaMueve.Count; j++)
                        {
                            N2 = (i + 2).ToString();
                            String aValuar = ListaMueve[i].ToString()[0] + "n2";
                            if (ListaMueve[j].ToString().Equals(aValuar))
                            {
                                ListaMueve[i] = N2 + "," + (j + 2).ToString();
                                N2 = "";
                                break;
                            }
                        }
                    }
                    else if (ListaMueve[i].ToString()[2] == '2')
                    {
                        for (int j = i; j < ListaMueve.Count; j++)
                        {
                            if (ListaMueve[j].ToString() == ListaMueve[i].ToString()[0] + "n3")
                            {
                                ListaMueve[i] = j + 2;
                                break;
                            }
                        }
                    }
                    else if (ListaMueve[i].ToString()[2] == '3')
                    {
                        ListaMueve[i] = i + 2;
                    }
                }
                catch { }
            }
        }
        public void generar_dot(String rdot,String rpng)
        {
            System.IO.File.WriteAllText(rdot,grafo.ToString());
            String sa = Application.StartupPath;
            String comandoDot = "dot.exe -Tpng "+rdot+" -o "+rpng+" ";
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + comandoDot);
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = false;
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            string result = proc.StandardOutput.ReadToEnd();
            Console.WriteLine(result);
        }
        public void graficar()
        {
            grafo = new StringBuilder();
            String rdot = ruta+"\\Thompson.dot";
            String rpng = ruta+"\\Thompson.png";
            grafo.Append("digraph G {");
            grafo.Append("rankdir=\"LR\";");
            for (int i = 0; i < ListaThom.Count; i++)
            {
                grafo.Append((i+1)+ "->"+ListaMueve[i]+"[label=\""+ ListaThom[i] + "\"];");
            }
            grafo.Append((ListaThom.Count + 1).ToString()+ "[peripheries=2 shape=circle];");
            grafo.Append("}");
            generar_dot(rdot,rpng);
        }
        static void Cerradura(int var) {
            Console.WriteLine("Conjunto actual = "+(char)ConjuntoChar);
            Console.WriteLine("Var = " + var);
            int IndexActual = (ConjuntoChar - 65);
            addPivote(var,IndexActual);
            if (ListaThom[var].ToString() == "ε")
            {
                if (ListaMueve[var].ToString().Length > 1)
                {
                    string[] words = ListaMueve[var].ToString().Split(',');
                    ListaConjun[IndexActual].Add(words[0]);
                    int auxb = Int32.Parse(words[0].ToString());
                    Cerradura(auxb - 1);
                    ListaConjun[IndexActual].Add(words[1]);
                    int auxa = Int32.Parse(words[1].ToString());
                    Cerradura(auxa - 1);
                }
                else
                {
                    ListaConjun[IndexActual].Add(ListaMueve[var].ToString());
                    Cerradura(Int32.Parse(ListaMueve[var].ToString()) - 1);
                }
            }
            else {
                return;
            }
            Console.WriteLine("Si salio del bucle");
            String con = ((char)ConjuntoChar).ToString();
            for (int i = 0; i < ListaTerminales.Count; i++)
            {
                Ir(con,ListaTerminales[i].ToString());
            }
        }
        static void Ir(String Conjunto, String Simbolo) {
            Console.WriteLine("Ir("+Conjunto+","+Simbolo+")");
            for (int i=0;i<ListaConjun.Count;i++) {
                Console.WriteLine("i del bucle conjunto: "+i+" conjunto: "+Conjunto);
                if (ListaConjun[i][0].Equals(Conjunto)) {
                    Console.WriteLine("Si entro a conjunto");
                    for (int j = 1; j < ListaConjun[i].Count; j++)
                    {
                        int valu = Int32.Parse(ListaConjun[i][j].ToString())-1;
                        if (ListaThom[valu].ToString()==Simbolo) {
                            Console.WriteLine("Si reconoce "+Simbolo);
                            ConjuntoChar++;
                            ListaConjun.Add(new List<string>());
                            int IndexActual = (ConjuntoChar - 65);
                            ListaConjun[IndexActual].Add(((char)ConjuntoChar).ToString());
                            Cerradura(Int32.Parse(ListaMueve[valu].ToString())-1);
                            return;
                        } 
                    }
                    break;
                }
            } 
        }
        static void addPivote(int var,int IndexActual) {
            bool existe = false;
            for (int k = 0; k < ListaConjun[IndexActual].Count; k++)
            {
                if(ListaConjun[IndexActual][k].Equals((var + 1).ToString())){
                    existe = true;
                }
            }
            if (existe==false)
            {
                ListaConjun[IndexActual].Add((var + 1).ToString());
            }
        }
    }
}
