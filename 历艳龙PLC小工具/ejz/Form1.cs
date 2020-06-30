using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ejz
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            calldeng1(textBox1);
            calldeng2(textBox2);
            calldeng3(textBox3);
        }

        public void calldeng1(TextBox textbox)
        {
            textBox4.BackColor = Color.WhiteSmoke;
            textBox5.BackColor = Color.WhiteSmoke;
            textBox6.BackColor = Color.WhiteSmoke;
            textBox7.BackColor = Color.WhiteSmoke;
            textBox8.BackColor = Color.WhiteSmoke;
            textBox9.BackColor = Color.WhiteSmoke;
            textBox10.BackColor = Color.WhiteSmoke;
            textBox11.BackColor = Color.WhiteSmoke;
            if (textbox.Text.Trim() == "")
            {
                return;
            }
            double w1;
            if (!double.TryParse(textbox.Text, out w1))
            {
                MessageBox.Show("输入1不是有效的数字");
                return;
            }
            if (w1 > 255 || w1 < 0)
            {
                MessageBox.Show("输入1值超范围");
                return;
            }
            int zs = Convert.ToInt16(w1);
            string a = DecimalToBinary(zs);
            if (a.Substring(0, 1) == "1")
            {
                textBox11.BackColor = Color.Green;
            }
            else
            {
                textBox11.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(1, 1) == "1")
            {
                textBox10.BackColor = Color.Green;
            }
            else
            {
                textBox10.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(2, 1) == "1")
            {
                textBox9.BackColor = Color.Green;
            }
            else
            {
                textBox9.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(3, 1) == "1")
            {
                textBox8.BackColor = Color.Green;
            }
            else
            {
                textBox8.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(4, 1) == "1")
            {
                textBox7.BackColor = Color.Green;
            }
            else
            {
                textBox7.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(5, 1) == "1")
            {
                textBox6.BackColor = Color.Green;
            }
            else
            {
                textBox6.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(6, 1) == "1")
            {
                textBox5.BackColor = Color.Green;
            }
            else
            {
                textBox5.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(7, 1) == "1")
            {
                textBox4.BackColor = Color.Green;
            }
            else
            {
                textBox4.BackColor = Color.WhiteSmoke;
            }

        }


        public void calldeng2(TextBox textbox)
        {
            textBox12.BackColor = Color.WhiteSmoke;
            textBox13.BackColor = Color.WhiteSmoke;
            textBox14.BackColor = Color.WhiteSmoke;
            textBox15.BackColor = Color.WhiteSmoke;
            textBox16.BackColor = Color.WhiteSmoke;
            textBox17.BackColor = Color.WhiteSmoke;
            textBox18.BackColor = Color.WhiteSmoke;
            textBox19.BackColor = Color.WhiteSmoke;
            if (textbox.Text.Trim() == "")
            {
                return;
            }
            double w1;
            if (!double.TryParse(textbox.Text, out w1))
            {
                MessageBox.Show("输入2不是有效的数字");
                return;
            }
            if (w1 > 255 || w1 < 0)
            {
                MessageBox.Show("输入2值超范围");
                return;
            }
            int zs = Convert.ToInt16(w1);
            string a = DecimalToBinary(zs);
            if (a.Substring(0, 1) == "1")
            {
                textBox19.BackColor = Color.Green;
            }
            else
            {
                textBox19.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(1, 1) == "1")
            {
                textBox18.BackColor = Color.Green;
            }
            else
            {
                textBox18.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(2, 1) == "1")
            {
                textBox17.BackColor = Color.Green;
            }
            else
            {
                textBox17.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(3, 1) == "1")
            {
                textBox16.BackColor = Color.Green;
            }
            else
            {
                textBox16.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(4, 1) == "1")
            {
                textBox15.BackColor = Color.Green;
            }
            else
            {
                textBox15.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(5, 1) == "1")
            {
                textBox14.BackColor = Color.Green;
            }
            else
            {
                textBox14.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(6, 1) == "1")
            {
                textBox13.BackColor = Color.Green;
            }
            else
            {
                textBox13.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(7, 1) == "1")
            {
                textBox12.BackColor = Color.Green;
            }
            else
            {
                textBox12.BackColor = Color.WhiteSmoke;
            }

        }

        public void calldeng3(TextBox textbox)
        {
            textBox20.BackColor = Color.WhiteSmoke;
            textBox21.BackColor = Color.WhiteSmoke;
            textBox22.BackColor = Color.WhiteSmoke;
            textBox23.BackColor = Color.WhiteSmoke;
            textBox24.BackColor = Color.WhiteSmoke;
            textBox25.BackColor = Color.WhiteSmoke;
            textBox26.BackColor = Color.WhiteSmoke;
            textBox27.BackColor = Color.WhiteSmoke;
            if (textbox.Text.Trim() == "")
            {
                return;
            }
            double w1;
            if (!double.TryParse(textbox.Text, out w1))
            {
                MessageBox.Show("输入3不是有效的数字");
                return;
            }
            if (w1 > 255 || w1 < 0)
            {
                MessageBox.Show("输入3值超范围");
                return;
            }
            int zs = Convert.ToInt16(w1);
            string a = DecimalToBinary(zs);
            if (a.Substring(0, 1) == "1")
            {
                textBox27.BackColor = Color.Green;
            }
            else
            {
                textBox27.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(1, 1) == "1")
            {
                textBox26.BackColor = Color.Green;
            }
            else
            {
                textBox26.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(2, 1) == "1")
            {
                textBox25.BackColor = Color.Green;
            }
            else
            {
                textBox25.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(3, 1) == "1")
            {
                textBox24.BackColor = Color.Green;
            }
            else
            {
                textBox24.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(4, 1) == "1")
            {
                textBox23.BackColor = Color.Green;
            }
            else
            {
                textBox23.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(5, 1) == "1")
            {
                textBox22.BackColor = Color.Green;
            }
            else
            {
                textBox22.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(6, 1) == "1")
            {
                textBox21.BackColor = Color.Green;
            }
            else
            {
                textBox21.BackColor = Color.WhiteSmoke;
            }

            if (a.Substring(7, 1) == "1")
            {
                textBox20.BackColor = Color.Green;
            }
            else
            {
                textBox20.BackColor = Color.WhiteSmoke;
            }

        }

        public string DecimalToBinary(int decimalNum)
        {
            string binaryNum = Convert.ToString(decimalNum, 2);
            if (binaryNum.Length < 8)
            {
                int b = binaryNum.Length;
                for (int i = 0; i < 8 - b; i++)
                {
                    binaryNum = '0' + binaryNum;
                }
            }
            return binaryNum;
        }
    }
}
