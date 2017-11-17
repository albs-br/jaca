using CSCompiler.Entities;
using CSCompiler.Entities.Compiler;
using CSCompiler.Entities.CS.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSCompiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void TxtCsSourceCode_TextChanged(object sender, EventArgs e)
        {
            //var textbox = (TextBox)sender;

            try
            {
                var tokens = Compiler.ConvertSourceToTokens(TxtCsSourceCode.Text);

                var csProgram = Compiler.ConvertTokensToCommands(tokens);

                var machineCodeProgram = Compiler.ConvertCommandsToMachineCode(csProgram);

                //var machineCodeProgram = Compiler.Compile(TxtCsSourceCode.Text);




                TxtTokens.Text = "";
                foreach (var token in tokens)
                {
                    TxtTokens.Text += token.TokenToString();
                }

                TxtAssemblyCommands.Text = "";
                foreach (var command in csProgram.Commands)
                {
                    foreach (string assemblyCommand in command.AssemblyCommands)
                    {
                        TxtAssemblyCommands.Text += assemblyCommand + Environment.NewLine;
                    }
                }

                TxtOutputMachineCode.Text = machineCodeProgram.GetBytesAsString(Constants.BASE_ADDR_PROGRAM, 256);




                LblMsgCsSourceCode.Text = "Valid";
                LblMsgCsSourceCode.ForeColor = Color.DarkGreen;
            }
            catch (Exception ex)
            {
                TxtTokens.Text = "";
                TxtAssemblyCommands.Text = "";
                TxtOutputMachineCode.Text = "";




                LblMsgCsSourceCode.Text = ex.Message;
                LblMsgCsSourceCode.ForeColor = Color.Red;
            }
        }
    }
}
