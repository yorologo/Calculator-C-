using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        bool point;
        int openParenthesis;
        List<Button> btnNumberList;
        List<Button> btnOperatorList;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            point = false;
            openParenthesis = 0;

            btnNumberList = new List<Button>
            {
                button1,
                button2,
                button3,
                button4,
                button5,
                button6,
                button7,
                button8,
                button9
            };
            btnOperatorList = new List<Button>
            {
                button0,
                buttonMultiplicacion,
                buttonResta,
                buttonSuma,
                buttonDivision,
            };

            foreach (Button btn in btnNumberList)
                btn.Click += new EventHandler(NumberButton_Click);
            foreach (Button btn in btnOperatorList)
                btn.Click += new EventHandler(OperatorButton_Click);
        }

        private void NumberButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "?")
                textBox1.Text = "";
            if (textBox1.Text != "")
            {
                if (textBox1.Text.Last() == '√' || textBox1.Text.Last() == '^')
                {
                    ++openParenthesis;
                    textBox1.Text += '(';
                }
            }
            textBox1.Text += (sender as Button).Text;
        }

        private void OperatorButton_Click(object sender, EventArgs e)
        {

            point = false;
            if (textBox1.Text != "")
            {
                if (LastCharIsNum() || textBox1.Text.Last() == '.' || textBox1.Text.Last() == ')')
                    textBox1.Text += (sender as Button).Text;
                else
                {
                    string s = "";
                    for (int i = 0; i < textBox1.Text.Length - 1; i++)
                        s += textBox1.Text[i];
                    textBox1.Text = s + (sender as Button).Text;
                }
            }
        }

        private void buttonBorrar_Click(object sender, EventArgs e)
        {
            point = false;
            if (textBox1.Text != "")
            {
                string s = "";
                if (textBox1.Text.Last() == 'R')
                    for (int i = 0; i < textBox1.Text.Length - 3; i++)
                    {
                        Console.WriteLine("DEL " + i + ": " + textBox1.Text[i]);
                        if (textBox1.Text[i] == '.') point = true;
                        else if (!(textBox1.Text[i] <= '9' && textBox1.Text[i] >= '0')) point = false;
                        s += textBox1.Text[i];
                    }
                else
                    for (int i = 0; i < textBox1.Text.Length - 1; i++)
                    {
                        Console.WriteLine("DEL " + i + ": " + textBox1.Text[i]);
                        if (textBox1.Text[i] == '.') point = true;
                        else if (!(textBox1.Text[i] <= '9' && textBox1.Text[i] >= '0')) point = false;
                        s += textBox1.Text[i];
                    }
                textBox1.Text = s;
            }
        }


        private void buttonPunto_Click(object sender, EventArgs e)
        {
            if (!point)
            {
                if (!LastCharIsNum())
                {
                    if (textBox1.Text.Last() == ')')
                        textBox1.Text += '×';
                    textBox1.Text += '0';
                }
                textBox1.Text += (sender as Button).Text;
                point = true;
            }
        }

        private void buttonSigno_Click(object sender, EventArgs e)
        {
            string numero = "", operacion = "";
            if (textBox1.Text != "")
            {
                foreach (char c in textBox1.Text)
                {
                    numero += c;
                    if (!((c <= '9' && c >= '0') || c == '.'))
                    {
                        operacion += numero;
                        numero = "";
                    }
                }
                if (operacion != null)
                    if (operacion.Length > 1 && operacion.Last() == '-')
                    {
                        if (operacion[operacion.Length - 2] == '(')
                        {
                            --openParenthesis;
                            textBox1.Text = "";
                            for (int i = 0; i < operacion.Length - 2; i++)
                                textBox1.Text += operacion[i];
                            textBox1.Text += numero;
                            return;
                        }
                    }
            }
            ++openParenthesis;
            textBox1.Text = operacion + "(-" + numero;
        }

        private void buttonPotencia_Click(object sender, EventArgs e)
        {
            if (LastCharIsNum())
            {
                ++openParenthesis;
                textBox1.Text += "^(";
            }
        }

        private void buttonParentesis_Click(object sender, EventArgs e)
        {
            point = false;
            if (textBox1.Text != "")
            {
                if (LastCharIsNum() || textBox1.Text.Last() == '.' || textBox1.Text.Last() == ')')
                {
                    if (openParenthesis > 0)
                    {
                        --openParenthesis;
                        textBox1.Text += ')';
                        return;
                    }
                    textBox1.Text += '×';
                }
            }
            ++openParenthesis;
            textBox1.Text += '(';
        }

        private void buttonRaiz_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "?")
                textBox1.Text = "";
            if (LastCharIsNum())
                textBox1.Text += '×';
            ++openParenthesis;
            textBox1.Text += "SQR(";
        }

        private void buttonIgual_Click(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = textBox1.Text + new String(')', openParenthesis);
                openParenthesis = 0;
                textBox1.Text = resolve(0).Item2;
            }
            catch (Exception)
            {
                textBox1.Text = textBox2.Text = "";
                textBox1.Text = "?";
            }
        }

        private bool LastCharIsNum()
        {
            if (textBox1.Text == "")
                return false;
            return textBox1.Text.Last() <= '9' && textBox1.Text.Last() >= '0';
        }

        private (int, string) resolve(int posicion)
        {
            DataTable dt = new DataTable();
            string operacion = "", numero = "";
            var aux = (0, "");
            for (int i = posicion; i < textBox1.Text.Length; i++)
            {
                Console.WriteLine(i + ": " + operacion + numero);

                switch (textBox1.Text[i])
                {
                    case '(':
                        aux = resolve(i + 1);
                        i = aux.Item1;
                        operacion += aux.Item2;
                        break;
                    case ')':
                        return (i, "" + dt.Compute(i + numero, null));
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '.':
                        numero += textBox1.Text[i];
                        break;
                    case '+':
                    case '-':
                        operacion += numero;
                        numero = "";
                        operacion += textBox1.Text[i];
                        break;
                    case '×':
                        operacion += numero;
                        numero = "";
                        operacion += '*';
                        break;
                    case '÷':
                        operacion += numero;
                        numero = "";
                        operacion += '/';
                        break;
                    case '^':
                        aux = resolve(i + 2);
                        i = aux.Item1;
                        operacion += Math.Pow(Convert.ToDouble(numero), Convert.ToDouble(aux.Item2));
                        numero = "";
                        break;
                    case 'S':
                        aux = resolve(i + 4);
                        i = aux.Item1;
                        operacion += Math.Sqrt(Convert.ToDouble(aux.Item2));
                        numero = "";
                        break;
                }
            }
            Console.WriteLine(textBox1.Text.Length + ": " + operacion + numero);
            return (textBox1.Text.Length, "" + dt.Compute(operacion + numero, null));
        }
    }
}