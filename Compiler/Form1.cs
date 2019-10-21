using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        public char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public char[] letters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
        'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G',
        'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
        public char[] chars = { '=', '+', '-', '/', '*', '!','.' ,';', ':', '{', '}' , ')', '(', '<', '>' };

        public static int[] masiv = new int[50];
        public static int t = 0;
        public int k=0;

        public struct SymbolsStruct
        {
            public string symb;
            public int code;
            public SymbolsStruct(string s, int pos)
            {
                symb = s;
                code = pos;
            }
        }

        public struct IndexStruct
        {
            public string symb;
            public int position;
            public IndexStruct(string s, int pos)
            {
                symb = s;
                position = pos;
            }
        }

        public static SymbolsStruct[] symbolTable = new SymbolsStruct[50];
        public static IndexStruct[] indexTab = new IndexStruct[50];
        public static int tableSize = 0;

        public static void addRecord(string symb, int code)
        {
            IndexStruct indexEntry = new IndexStruct(symb, tableSize);
            SymbolsStruct symbolsEntry = new SymbolsStruct(symb, code);
            int comparison;

            for (int i = 0; i < tableSize + 1; i++)
            {
                comparison = string.CompareOrdinal(indexEntry.symb, indexTab[i].symb);

                if (comparison < 0 || indexTab[i].symb == null)
                {
                    IndexStruct temp = new IndexStruct(indexTab[i].symb, indexTab[i].position);
                    IndexStruct temp2 = new IndexStruct();
                    indexTab[i] = indexEntry;
                    symbolTable[tableSize] = symbolsEntry;
                    tableSize++;

                    for (int j = i + 1; j < tableSize; j++)
                    {
                        temp2.symb = indexTab[j].symb;
                        temp2.position = indexTab[j].position;
                        indexTab[j].symb = temp.symb;
                        indexTab[j].position = temp.position;
                        temp.symb = temp2.symb;
                        temp.position = temp2.position;
                    }
                    break;
                }
            }
        } //addRecord


        public int typecode( int elem)
        {
            int checkcode = 0;
            for (int j = 0; j < tableSize; j++) { 
              if (elem == indexTab[j].position) {  
                    for (int i = 0; i < tableSize; i++) { 
                        if ( indexTab[j].symb == symbolTable[i].symb) {
                            checkcode = symbolTable[i].code;
                            break;
                        }
                    }
                    break;
               }
            }
            return checkcode;
        }

        public string typesymb(int elem)
        {
            string checksymb = " ";
            for (int j = 0; j < tableSize; j++)
            {
                if (elem == indexTab[j].position)
                {
                    checksymb = indexTab[j].symb;
                    break;
                }
            }
            return checksymb;
        }

        // factor
        public void factor(ref int k)
        {
            if ((typecode(masiv[k]) == 1) || (typecode(masiv[k]) == 2))
                {
                
                }
            else if (typesymb(masiv[k]) == "(")
                {
                    addSub(ref k);
                }
            else if (typesymb(masiv[k]) != ")")
                {
                    MessageBox.Show("Wrong syntaxis:" + " " + typesymb(k));
                    Application.Exit();
            }
    }
        // mulDiv
        public void mulDiv(ref int k)
        {
            factor(ref k);
            k++;
            while (typesymb(masiv[k]) == "*" || typesymb(masiv[k]) == "/")
            {
                factor(ref k);
            }
            
           // else Error;
        }

        // addSub
        public void addSub(ref int k)
        {
            mulDiv(ref k);
            while (typesymb(masiv[k]) == "+" || typesymb(masiv[k]) == "-")
            {
                k++;
                mulDiv(ref k);
            }
            
            //else Error;
        }

        // exp
        public void exp(ref int k)
        {
            addSub(ref k);
            if (typesymb(masiv[k]) == ">" || typesymb(masiv[k]) == "<" || typesymb(masiv[k]) == "==" || typesymb(masiv[k]) == ">=" || typesymb(masiv[k]) == "<=")
            {
                k++;// nxt = lexAn();
                addSub(ref k);
            }
            else {
                MessageBox.Show("Wrong syntaxis:" + " " + typesymb(k));
            } 
        }

        // stat
        public void stat(ref int k)
        {
            k++; //nxt??

            if (typecode(masiv[k]) == 1)
            {
                k++; // nxt = lex();
                if (typesymb(masiv[k]) != "=")
                {
                    MessageBox.Show("Wrong syntaxis:" + " " + typesymb(k));
                }

                k++; // nxt = lexAn();
                addSub(ref k);

            }


            if (typesymb(masiv[k]) == "if")
            {
                k++;// nxt = lexAn();
              
                exp(ref k);
                if (typesymb(masiv[k]) != "{") {
                    MessageBox.Show("Wrong syntaxis:" + " " + typesymb(k)); 
                }
                k++;  // nxt = lexAn();
                stat(ref k);
                if (typesymb(masiv[k]) != "}") {
                    MessageBox.Show("Wrong syntaxis:" + " " + typesymb(k)); 
                }
                if (typesymb(masiv[k]) == "else")
                {
                    k++;//nxt = lexAn();
                    if (typesymb(masiv[k]) != "{") {
                        MessageBox.Show("Wrong syntaxis:" + " " + typesymb(k));
                    }
                    k++; // nxt = lexAn();
                    stat(ref k);
                    if (typesymb(masiv[k]) != "}") {
                        MessageBox.Show("Wrong syntaxis:" + " " + typesymb(k)); 
                    }
                }
            }
           // else MessageBox.Show("Wrong syntaxis" + masiv[k]);
        }

        // block()
        public void block(ref int k)
        {
            stat(ref k);
            while (typesymb(masiv[k]) == ";")
            {
               // k++;
                stat(ref k);
            }
        }

        // head()
        public void head(ref int k)
        {
            
            if (typesymb(masiv[k]) != "namespace") {
                MessageBox.Show("Wrong syntaxis:" + " " + typesymb(k));
            }
            k++;
            if (typecode(masiv[k]) != 1) {
                MessageBox.Show("Wrong syntaxis:" + " " + typesymb(k));
            }
            k++;
            if (typesymb(masiv[k]) != "{") {
                MessageBox.Show("Wrong syntaxis:" + " " + typesymb(k));
            }
            block(ref k);
            if (typesymb(masiv[k]) != "}") {
                MessageBox.Show("Wrong syntaxis:" + " " + typesymb(k));
            }
        }

        //returns the index of the symbol in the symbolTable
        public static int findTab(string symbol, int code)
        {
            int bot, top, mid, comparison;
            bot = 0;
            top = tableSize;
            while (bot <= top)
            {
                mid = (bot + top) / 2;
                comparison = string.CompareOrdinal(indexTab[mid].symb, symbol);
                if (comparison > 0)
                    top = mid - 1;
                else if (comparison < 0)
                    bot = mid + 1;
                else
                {
                    return indexTab[mid].position;
                }
            }//while
            addRecord(symbol, code);
            return tableSize - 1;
        }//findTab

        static void printAll()
        {
            int i;
            Console.WriteLine("---Index Table---");
            for (i = 0; i < tableSize; i++)
            {
                Console.WriteLine("Record: Symb: {0}  Pos: {1}", indexTab[i].symb, indexTab[i].position);
            }
            Console.WriteLine();
            Console.WriteLine("---Symbol Table---");
            for (i = 0; i < tableSize; i++)
            {
                Console.WriteLine("Record[{0}]: Symb: {1}  Code: {2}", i, symbolTable[i].symb, symbolTable[i].code);
            }
        }

        /*   static void Main(string[] args)
           {

               symbolTable = new SymbolsStruct[25];
               indexTab = new IndexStruct[25];
               tableSize = 0;
               //indexTab.OrderBy(IndexStruct => IndexStruct.symb);

               //int comparison = string.CompareOrdinal("+", "+");
               //Console.WriteLine("The comparison is: {0}", comparison);

               addRecord("Begin", 100); //служебна дума
               addRecord("End", 101); //служебна дума
               addRecord("A", 1); // идентификатор
               addRecord(":=", 200); //операционен символ
               addRecord("+", 201); //операционен символ
               addRecord("3.14", 2);  //константа
               addRecord("AC32DC", 1); // идентификатор
               addRecord(";", 300);
           }//разделител*/

        private void btnAdd_Click(object sender, EventArgs e)
        {

            textBox2.Text = "";
            string code = textBox1.Text + " ";
            System.Text.StringBuilder bufnum = new System.Text.StringBuilder();
            System.Text.StringBuilder bufword = new System.Text.StringBuilder();
            System.Text.StringBuilder bufop = new System.Text.StringBuilder();
            int flag = 0;
            int w = 0;
            int n = 0;
            int op = 0;

            foreach (char c in code)
            {
                if (letters.Contains(c))
                {
                    w = 1;
                    bufword.Append(c);

                }
                else
                {
                    if (numbers.Contains(c) && w == 1)
                    {
                        bufword.Append(c);
                    }
                    else if (w == 1)
                    {
                        findTab(bufword.ToString(), 1);
                        for (int k = 0; k < tableSize; k++)
                            if (bufword.ToString() == indexTab[k].symb)
                            {
                                masiv[t] = indexTab[k].position;
                                t++;
                            }
                        textBox2.Text = textBox2.Text + bufword + "\r\n";
                        bufword.Clear();
                        w = 0;
                    }
                }
                if (numbers.Contains(c) && w != 1)
                {
                    bufnum.Append(c);
                    n = 1;
                }
                else
                {
                    if (flag == 0 && c.Equals('.'))
                    {
                        bufnum.Append(c);
                        flag = 1;
                    }
                    else if (flag == 1 && c.Equals('.'))
                    {
                        MessageBox.Show("Invalid number!");
                        Application.Exit();

                    }
                    else if (n == 1)
                    {
                        findTab(bufnum.ToString(), 2);
                        for (int k = 0; k < tableSize; k++)
                            if (bufnum.ToString() == indexTab[k].symb)
                            {
                                masiv[t] = indexTab[k].position;
                                t++;
                            }
                        textBox2.Text = textBox2.Text + bufnum + "\r\n";
                        bufnum.Clear();
                        flag = 0;
                        n = 0;
                    }
                }

                if (chars.Contains(c) && flag != 1)
                {
                    bufop.Append(c);
                    op = 1;
                }
                else if (op == 1 && flag != 1)
                {
                    findTab(bufop.ToString(), 3);
                    for (int k = 0; k < tableSize; k++)
                        if (bufop.ToString() == indexTab[k].symb)
                        {
                            masiv[t] = indexTab[k].position;
                            t++;
                        }
                    textBox2.Text = textBox2.Text + bufop + "\r\n";
                    bufop.Clear();
                    op = 0;
                }

                if ((!letters.Contains(c)) && (!numbers.Contains(c)) && (!chars.Contains(c)) && (!char.IsWhiteSpace(c)))
                {
                    MessageBox.Show("Invalid symbol: " + c);
                }
            }

            //syntaxis

            /*for (int i = 0; i < masiv.Length; i++)
            {
                head(i);
            }*/
            head(ref k);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TableLayoutPanel dynamicTableLayoutPanel = new TableLayoutPanel();
            int i;

            textBox6.Text = "";
            textBox4.Text = "---Index Table---\r\n";
            textBox4.Text = textBox4.Text + "Symb:          Pos in SymbTab: \r\n";
            textBox7.Text = "\r\n" + "Pos " + "\r\n";
            for (i = 0; i < tableSize; i++)
            {
                textBox4.Text = textBox4.Text + indexTab[i].symb + "                " + indexTab[i].position + "\r\n";
                for (int k = 0; k < tableSize; k++)
                    if (symbolTable[i].symb == indexTab[k].symb)
                    {
                        textBox7.Text = textBox7.Text + indexTab[k].position + "\r\n";

                    }

            }


            textBox3.Text = "-Sym Tab-\r\n";
            textBox5.Text = "\r\n" + "Code " + "\r\n";
            textBox3.Text = textBox3.Text + "Symb:       \r\n";
            for (i = 0; i < tableSize; i++)
            {
                textBox3.Text = textBox3.Text + symbolTable[i].symb + "\r\n";
                textBox5.Text = textBox5.Text + symbolTable[i].code + "\r\n";

            }

            for (int z = 0; z < t; z++)
                textBox6.Text = textBox6.Text + masiv[z] + " ";
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
