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
    public partial class FormMain : Form
    {
        public FormMain()
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
                    var text = String.Format("{0:x2} {1:x2} {2:x2}",
                        instruction[0],
                        instruction[1],
                        instruction[2]
                        );
                    textBoxBytes.Text += text;

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

        private void textBoxBytes_TextChanged(object sender, EventArgs e)
        {
            //
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
