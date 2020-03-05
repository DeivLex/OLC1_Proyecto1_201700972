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
        String ruta;
        StringBuilder grafo;
        ArrayList Aux = new ArrayList();
        ArrayList ListaThom = new ArrayList();
        ArrayList ListaMueve = new ArrayList();
        public void Inicio(String filtro)
        {
            ruta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Aux.Clear();
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
            graficar();


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
                            ListaMueve.Insert(j,"n4");
                            ListaThom.Insert(j, null);
                            break;
                        }
                    }
                    if (ListaThom.Count == 0)
                    {
                        ListaMueve.Insert(0,"n4");
                        ListaThom.Insert(0, null);
                    }
                }
                else if (Aux[i].Equals("or"))
                {
                    for (int j = 0; j < ListaThom.Count; j++)
                    {
                        if (ListaThom[j] == null)
                        {
                            ListaMueve.Add("n1");
                            ListaMueve.Add("n");
                            ListaMueve.Add("n2");
                            ListaMueve.Add("n");
                            ListaMueve.Add("n3");
                            ListaThom.Add("ε");
                            ListaThom.Add(null);
                            ListaThom.Add("ε");
                            ListaThom.Add(null);
                            ListaThom.Add("ε");
                            break;
                        }
                    }
                    if (ListaThom.Count == 0)
                    {
                        ListaMueve.Insert(0,"n1");
                        ListaMueve.Insert(1,"n");
                        ListaMueve.Insert(2,"n2");
                        ListaMueve.Insert(3,"n");
                        ListaMueve.Insert(4,"n3");
                        ListaThom.Insert(0,"epsilon");
                        ListaThom.Insert(1,null);
                        ListaThom.Insert(2,"epsilon");
                        ListaThom.Insert(3,null);
                        ListaThom.Insert(4,"epsilon");
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
            for (int i = 0; i < ListaMueve.Count; i++)
            {
                if (ListaMueve[i].ToString() == "n1")
                {
                    String auxN = (i + 2).ToString();
                    for (int j = i; j < ListaMueve.Count; j++)
                    {
                        if (ListaMueve[j].ToString() == "n2")
                        {
                            auxN = auxN + "," + (j + 2).ToString();
                            ListaMueve[i] = auxN;
                            break;
                        }
                    }
                    } else if (ListaMueve[i].ToString() == "n2")
                {
                    ListaMueve[i] = i + 4;
                } else if (ListaMueve[i].ToString() == "n3")
                {
                    ListaMueve[i] = i + 2;
                }
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
            grafo.Append("}");
            generar_dot(rdot,rpng);
        }

    }
}
