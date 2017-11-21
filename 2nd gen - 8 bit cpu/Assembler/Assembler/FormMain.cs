using Assembler.Entities;
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
            try
            {
                textBoxBytes.Clear();
                textBoxLabels.Clear();

                var machineCodeProgram = Converter.ResolveLabels(textBoxAssembly.Text);
                Converter.ConvertSource(machineCodeProgram);

                textBoxBytes.Text = machineCodeProgram.BytesAsText;

                foreach (var label in machineCodeProgram.Labels)
                {
                    textBoxLabels.Text +=
                        string.Format("{0}  {1:x4}", label.Key.PadRight(10), label.Value) +
                        Environment.NewLine;
                }

                LblStatus.ForeColor = Color.DarkGreen;
                LblStatus.Text = "Valid";
            }
            catch (Exception ex)
            {
                LblStatus.ForeColor = Color.Red;
                LblStatus.Text = "Invalid. " + ex.Message;
            }
        }

        private void textBoxBytes_TextChanged(object sender, EventArgs e)
        {
            //
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //
        }

        private void textBoxLabels_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
