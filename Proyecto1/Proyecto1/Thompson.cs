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
        static int Numero_fila;
        static int Numero_columna;
        static int NumO;
        static int ConjuntoChar;
        String ruta;
        StringBuilder grafo;
        static ArrayList AuxLetra = new ArrayList();
        ArrayList SalidaEvaluar = new ArrayList();
        ArrayList Aux = new ArrayList();
        static ArrayList AuxTran = new ArrayList();
        static ArrayList AFD = new ArrayList();
        static ArrayList ListaThom = new ArrayList();
        static ArrayList ListaMueve = new ArrayList();
        static List<List<String>> ListaConjun = new List<List<String>>();
        

        public void Inicio(String filtro)
        {
            Numero_fila = 0;
            Numero_columna = 0;
            NumO = 0;
            ConjuntoChar = 65;
            ruta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            SalidaEvaluar.Clear();
            AuxTran.Clear();
            AuxLetra.Clear();
            AFD.Clear();
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
            ListaMueve.Add(null);
            ListaThom.Add(null);
            //inicializar cerradura
            ListaConjun.Add(new List<String>());
            ListaConjun[0].Add(((char)ConjuntoChar).ToString());
            ListaConjun[0].Add((1).ToString());
            Cerradura(0);
            ConjuntoChar++;
            for (int i = 0; i < ListaThom.Count-1; i++)
            {
                if (ListaThom[i].ToString()!="ε") {
                    int IndexActual = (ConjuntoChar - 65);
                    AuxLetra.Add(ListaThom[i].ToString());
                    AuxLetra.Add(((char)ConjuntoChar).ToString());

                    ListaConjun.Add(new List<String>());
                    ListaConjun[IndexActual].Add(((char)ConjuntoChar).ToString());
                    ListaConjun[IndexActual].Add((i + 2).ToString());
                    Cerradura(i + 1);
                    ConjuntoChar++;
                }
            }
            //llenar tabla de trancisiones
            int alto = (ConjuntoChar - 64);
            int ancho = (AuxLetra.Count / 2)+1;
            Numero_fila = alto;
            Numero_columna = ancho;
            Console.WriteLine("alto = "+alto+" ancho = "+ancho);
            String[,] TablaTransiciones = new String[alto,ancho];
            for (int i = 0; i < alto; i++)
            {
                for (int j = 0; j < ancho; j++)
                {
                    if (i == 0)
                    {
                        if (j == 0) {
                            TablaTransiciones[0, 0] = "Inicio";
                        }
                        else
                        {
                            TablaTransiciones[i, j] = AuxLetra[((j - 1) * 2)].ToString();
                        } 
                    }else if (j==0) {
                        TablaTransiciones[i, j] = ((char)(i + 64)).ToString();
                    }
                    else{
                        TablaTransiciones[i, j] = "--";
                    }
                }
            }
            for (int i = 0; i < AuxLetra.Count; i++)
            {
                Console.WriteLine(AuxLetra[i]);
            }
            //imprimir Lista Conjun
            for (int i = 0; i < ListaConjun.Count; i++)
            {
                    for (int j = 0; j < ListaConjun[i].Count; j++)
                {
                    Console.Write(ListaConjun[i][j]+"--->");
                }
                Console.WriteLine();
            }
            Ir();
            for (int i = 0; i < AuxTran.Count; i++)
            {
                String[] words = AuxTran[i].ToString().Split(',');
                //Console.WriteLine(words[0]+"--->"+ words[1] + "--->" + words[2]);
                TablaTransiciones[Int32.Parse(words[0]),Int32.Parse(words[1])] = words[2];
            }
            //graficar AFD
            graficarAFD();
            //graficar tabla
            graficarTabla(alto,ancho,TablaTransiciones);
            //Salida lexemas
            for (int i = 0; i < Form1.LexemasEvaluar.Count; i++)
            {
                int Val_conjunto = 1;
                for (int j = 0; j < Form1.LexemasEvaluar[i].ToString().Length; j++)
                {
                    Val_conjunto = AceptarLexemas(Val_conjunto,Form1.LexemasEvaluar[i].ToString()[j].ToString(),TablaTransiciones);
                }
            }
            //Imprimir analisis
            for (int i = 0; i < SalidaEvaluar.Count; i++)
            {
                Console.WriteLine(SalidaEvaluar[i]);
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
                        String auxN = (i + 2).ToString();
                        for (int j = i; j < ListaMueve.Count; j++)
                        {
                            if (ListaMueve[j].ToString() == "n5")
                            {
                                auxN = auxN + "," + (j + 2).ToString();
                                ListaMueve[i] = auxN;
                                break;
                            }
                        }
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
        static void Ir() {
            for (int i = 0; i < ListaConjun.Count; i++)
            {
                for (int j = 1; j < ListaConjun[i].Count; j++)
                {
                    int hola = Int32.Parse(ListaConjun[i][j].ToString());
                    if (ListaThom[hola - 1]!=null) {
                        if (ListaThom[hola - 1].ToString() != "ε")
                        {
                            int le = Int32.Parse(ListaConjun[i][j].ToString());
                            AFD.Add(ListaConjun[i][0].ToString() + "->" + BuscarL(ListaThom[le-1].ToString()) + "[label=\"" + ListaThom[le - 1].ToString() + "\"];");
                            int fila=char.Parse(ListaConjun[i][0].ToString())-64;
                            int columna= char.Parse(BuscarL(ListaThom[le - 1].ToString()))-65;
                            String RR= BuscarL(ListaThom[le - 1].ToString());
                            AuxTran.Add(fila.ToString()+","+columna.ToString()+","+RR);
                        }
                    }
                }
            }
        }
        static String BuscarL(String letra) {
            for (int i = 0; i < AuxLetra.Count; i++)
            {
                if (AuxLetra[i].ToString().Equals(letra))
                {
                    return AuxLetra[i + 1].ToString();
                }
            }
            return letra;
        }
        static void Cerradura(int var)
        {
            int IndexActual = (ConjuntoChar - 65);
            if (ListaThom[var] == null)
            {
                AFD.Add((char)ConjuntoChar+ "[peripheries=2 shape=circle];");
                return;
            }
            else if (ListaThom[var].ToString() == "ε")
            {
                bool b = ListaMueve[var].ToString().Contains(",");
                if (b == true)
                {
                    Console.WriteLine("SI TIENE EPSILON CASO 1: " + var);
                    string[] words = ListaMueve[var].ToString().Split(',');
                    addPivote(IndexActual, Int32.Parse(words[0].ToString()));
                    Console.WriteLine("Si agrego: "+ Int32.Parse(words[0].ToString())+" = "+ (char)ConjuntoChar);
                    int auxb = Int32.Parse(words[0].ToString());
                    Cerradura(auxb - 1);
                    addPivote(IndexActual, Int32.Parse(words[1].ToString()));
                    Console.WriteLine("Si agrego: " + Int32.Parse(words[1].ToString()) + " = " + (char)ConjuntoChar);
                    int auxa = Int32.Parse(words[1].ToString());
                    Cerradura(auxa - 1);
                }
                else
                {
                    Console.WriteLine("SI TIENE EPSILON CASO 2: " + var);
                    addPivote(IndexActual,Int32.Parse(ListaMueve[var].ToString()));
                    Cerradura(Int32.Parse(ListaMueve[var].ToString()) - 1);
                }
            }
        }
        static void addPivote(int IndexActual,int var) {
            bool existe = false;
            for (int k = 1; k < ListaConjun[IndexActual].Count; k++)
            {
                if(ListaConjun[IndexActual][k].Equals((var).ToString())){
                    existe = true;
                }
            }
            if (existe==false)
            {
                Console.WriteLine("ADD = " + var);
                ListaConjun[IndexActual].Add((var).ToString());
            }
        }
        public void graficarAFD()
        {
            grafo = new StringBuilder();
            String rdot = ruta + "\\AFD.dot";
            String rpng = ruta + "\\AFD.png";
            grafo.Append("digraph G {");
            grafo.Append("rankdir=\"LR\";");
            for (int i = 0; i < AFD.Count; i++)
            {
                grafo.Append(AFD[i]);
            }
            grafo.Append("}");
            generar_dot(rdot, rpng);
        }
        public void graficarTabla(int f,int c,String[,]Hello)
        {
            grafo = new StringBuilder();
            String rdot = ruta + "\\Transiciones.dot";
            String rpng = ruta + "\\Transiciones.png";
            grafo.Append("digraph G {");
            grafo.Append("A[color=green shape=record label=\"{");
            for (int i = 0; i < c; i++)
            {                
                for (int j = 0; j < f; j++)
                {
                    if (j == 0) {
                        grafo.Append(Hello[j, i]);
                    }
                    else
                    {
                        grafo.Append("|" + Hello[j, i]);
                    }
                    
                }
                if (i !=(c-1))
                {
                    grafo.Append("}|{");
                }
            }
            grafo.Append("}\"];}");
            generar_dot(rdot, rpng);
        }
        public int AceptarLexemas(int fila, String valor, String[,] Hello) {
            bool sig = false;
            for (int j = 1; j < Numero_columna; j++)
            {
                String limpio = Hello[0, j].Replace("'", "");
                if (limpio.Equals(valor)) {
                    if (Hello[fila, j]!="--") {
                        SalidaEvaluar.Add(valor + "Aceptado");
                        int regreso =char.Parse(Hello[fila, j]) - 64;
                        sig = true;
                        return regreso;
                    }
                }
            }
            if (sig==false) {
                SalidaEvaluar.Add(valor + "Error");
            }
            return fila;
        }

    }
}
