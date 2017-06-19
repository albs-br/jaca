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
            var lines = textBox1.Lines;
            var converter = new Converter();
            byte[] program;

            foreach (var line in lines)
            { 
                // Ignore Comments
                if (line.StartsWith("//"))
                {
                    continue;
                }

                var instruction = converter.ConvertLine(line);

                //program.Concat(instruction);
            }
        }


    }
}
