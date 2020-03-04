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
        ArrayList Aux = new ArrayList();
        ArrayList ListaThom = new ArrayList();
        public void Inicio(String filtro)
        {
            Aux.Clear();
            for (int i = 0; i < Form1.ER.Count; i++)
            {
                if (Form1.NameER[i].Equals(filtro))
                {
                    Aux.Add(Form1.ER[i]);
                }
            }
            for (int i = 0; i < Aux.Count; i++)
            {
                if (Aux[i].Equals("and"))
                {
                    for (int j = 0; j < ListaThom.Count; j++)
                    {
                        if (ListaThom[j]==null)
                        {
                            ListaThom.Insert(j, null);
                            break;
                        }
                    }
                    if(ListaThom.Count==0)
                    {
                        ListaThom.Insert(0, null);
                    }
                }
                else if (Aux[i].Equals("or"))
                {
                    for (int j = 0; j < ListaThom.Count; j++)
                    {
                        if (ListaThom[j]==null)
                        {
                            ListaThom.Add("epsilon");
                            ListaThom.Add(null);
                            ListaThom.Add("epsilon");
                            ListaThom.Add(null);
                            ListaThom.Add("epsilon");
                            break;
                        }
                    }
                    if (ListaThom.Count == 0)
                    {
                        ListaThom.Add("epsilon");
                        ListaThom.Add(null);
                        ListaThom.Add("epsilon");
                        ListaThom.Add(null);
                        ListaThom.Add("epsilon");
                    }
                }
                else
                {
                    for (int j = 0; j < ListaThom.Count; j++)
                    {
                        if (ListaThom[j]==null)
                        {
                            ListaThom[j]=Aux[i];
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < ListaThom.Count; i++)
            {
                Console.WriteLine(ListaThom[i]);
            }
            Console.WriteLine("FIN");
        }
    }
}
