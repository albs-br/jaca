using Assembler.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            //UpdateTextboxes();
        }

        private void UpdateTextboxes()
        {
            try
            {
                textBoxBytes.Clear();
                textBoxLabels.Clear();
                textBoxVariables.Clear();

                var machineCodeProgram = Converter.ResolveLabels(textBoxAssembly.Text);
                Converter.ConvertSource(machineCodeProgram);

                textBoxBytes.Text = machineCodeProgram.BytesAsText;

                foreach (var label in machineCodeProgram.Labels)
                {
                    textBoxLabels.Text +=
                        string.Format("{0}  {1:x4}", label.Key.PadRight(10), label.Value) +
                        Environment.NewLine;
                }

                foreach (var variable in machineCodeProgram.Variables)
                {
                    textBoxVariables.Text +=
                        string.Format("{0}  {1:x4}", variable.Key.PadRight(10), variable.Value) +
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog();
        }

        private void BtnCopyMachineCodeToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBoxBytes.Text);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyToClipboard();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            NewFileDialog();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFileDialog();
        }

        #region Menu/Toolbar actions

        private string fileExtensions = "ASM files|*.asm|Text files (*.txt)|*.txt|All files (*.*)|*.*";

        private void NewFileDialog()
        {
            var confirmResult = MessageBox.Show("Do you really want to start a new file?",
                                                 "Confirm New File",
                                                 MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                textBoxAssembly.Text = "";
            }
        }

        private void OpenFileDialog()
        {
            Stream myStream = null;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Open File";
            dialog.Filter = fileExtensions;
            dialog.InitialDirectory = Application.StartupPath;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = dialog.OpenFile()) != null)
                    {
                        using (myStream)
                        using (var reader = new StreamReader(myStream, Encoding.UTF8))
                        {
                            string value = reader.ReadToEnd();

                            textBoxAssembly.Text = value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while reading file from disk. Error message: " + ex.Message);
                }
            }
        }

        private void SaveFileDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Save File";
            dialog.Filter = fileExtensions;
            dialog.FileName = "NewFile.asm";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (var writer = new StreamWriter(dialog.FileName, false, Encoding.UTF8))
                {
                    writer.WriteLine(textBoxAssembly.Text);
                }
            }
        }

        private void CopyToClipboard()
        {
            textBoxAssembly.Copy();
        }

        private void PasteToClipboard()
        {
            textBoxAssembly.Paste();
        }

        private void CutToClipboard()
        {
            textBoxAssembly.Cut();
        }

        #endregion

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog();
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            CopyToClipboard();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            PasteToClipboard();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteToClipboard();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CutToClipboard();
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            CutToClipboard();
        }

        private void BtnUpdateTextboxes_Click(object sender, EventArgs e)
        {
            UpdateTextboxes();
        }

        private void BtnSaveLogisim_Click(object sender, EventArgs e)
        {
            var filePath = @"C:\Users\xdad\Source\Repos\jaca\2nd gen - 8 bit cpu\ASM source files\" + "FileGeneratedByJACAAssembler.txt";

            var fileContent = "v2.0 raw" + Environment.NewLine;

            var source = textBoxBytes.Text.Replace(Environment.NewLine, " ");

            foreach (var b in source.Split(' '))
            {
                if (!string.IsNullOrWhiteSpace(b))
                {
                    fileContent += b + Environment.NewLine;
                }
            }

            File.WriteAllText(filePath, fileContent);
        }

    }
}
