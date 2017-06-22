using Assembler.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assembler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var lines = textBoxAssembly.Lines;
            textBoxBytes.Clear();
            var converter = new Converter();
            byte[] program;

            foreach (var line in lines)
            {
                try
                {
                    var instruction = converter.ConvertLine(line);

                    textBoxBytes.Text += instruction[0].ToString("X").PadLeft(2);
                    textBoxBytes.Text += " " + instruction[1].ToString("X").PadLeft(2);
                    textBoxBytes.Text += " " + instruction[2].ToString("X").PadLeft(2);

                    //program.Concat(instruction);
                }
                catch (NotCommandLineException)
                {
                    // do nothing
                    textBoxBytes.Text += "not command line";
                }
                catch (Exception)
                {
                    //textBoxBytes.Text += "exception";
                }
                finally
                {
                    textBoxBytes.Text += Environment.NewLine;
                }
            }
        }


    }
}
