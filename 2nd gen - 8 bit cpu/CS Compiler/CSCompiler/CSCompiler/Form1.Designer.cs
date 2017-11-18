namespace CSCompiler
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TxtCsSourceCode = new System.Windows.Forms.TextBox();
            this.LblMsgCsSourceCode = new System.Windows.Forms.Label();
            this.TxtOutputMachineCode = new System.Windows.Forms.TextBox();
            this.TxtTokens = new System.Windows.Forms.TextBox();
            this.TxtAssemblyCommands = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtVariables = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TxtCsSourceCode
            // 
            this.TxtCsSourceCode.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtCsSourceCode.Location = new System.Drawing.Point(16, 139);
            this.TxtCsSourceCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtCsSourceCode.Multiline = true;
            this.TxtCsSourceCode.Name = "TxtCsSourceCode";
            this.TxtCsSourceCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtCsSourceCode.Size = new System.Drawing.Size(579, 586);
            this.TxtCsSourceCode.TabIndex = 0;
            this.TxtCsSourceCode.TextChanged += new System.EventHandler(this.TxtCsSourceCode_TextChanged);
            // 
            // LblMsgCsSourceCode
            // 
            this.LblMsgCsSourceCode.AutoSize = true;
            this.LblMsgCsSourceCode.Location = new System.Drawing.Point(16, 730);
            this.LblMsgCsSourceCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblMsgCsSourceCode.Name = "LblMsgCsSourceCode";
            this.LblMsgCsSourceCode.Size = new System.Drawing.Size(0, 17);
            this.LblMsgCsSourceCode.TabIndex = 1;
            // 
            // TxtOutputMachineCode
            // 
            this.TxtOutputMachineCode.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtOutputMachineCode.Location = new System.Drawing.Point(1352, 139);
            this.TxtOutputMachineCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtOutputMachineCode.Multiline = true;
            this.TxtOutputMachineCode.Name = "TxtOutputMachineCode";
            this.TxtOutputMachineCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtOutputMachineCode.Size = new System.Drawing.Size(115, 586);
            this.TxtOutputMachineCode.TabIndex = 2;
            // 
            // TxtTokens
            // 
            this.TxtTokens.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtTokens.Location = new System.Drawing.Point(604, 139);
            this.TxtTokens.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtTokens.Multiline = true;
            this.TxtTokens.Name = "TxtTokens";
            this.TxtTokens.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtTokens.Size = new System.Drawing.Size(331, 586);
            this.TxtTokens.TabIndex = 3;
            // 
            // TxtAssemblyCommands
            // 
            this.TxtAssemblyCommands.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtAssemblyCommands.Location = new System.Drawing.Point(944, 139);
            this.TxtAssemblyCommands.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TxtAssemblyCommands.Multiline = true;
            this.TxtAssemblyCommands.Name = "TxtAssemblyCommands";
            this.TxtAssemblyCommands.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtAssemblyCommands.Size = new System.Drawing.Size(399, 586);
            this.TxtAssemblyCommands.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "C# Source Code:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(601, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Tokens:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(941, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Assembly Code:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1349, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Machine Code:";
            // 
            // TxtVariables
            // 
            this.TxtVariables.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtVariables.Location = new System.Drawing.Point(1102, 13);
            this.TxtVariables.Margin = new System.Windows.Forms.Padding(4);
            this.TxtVariables.Multiline = true;
            this.TxtVariables.Name = "TxtVariables";
            this.TxtVariables.ReadOnly = true;
            this.TxtVariables.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtVariables.Size = new System.Drawing.Size(241, 118);
            this.TxtVariables.TabIndex = 9;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 775);
            this.Controls.Add(this.TxtVariables);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TxtAssemblyCommands);
            this.Controls.Add(this.TxtTokens);
            this.Controls.Add(this.TxtOutputMachineCode);
            this.Controls.Add(this.LblMsgCsSourceCode);
            this.Controls.Add(this.TxtCsSourceCode);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormMain";
            this.Text = "C# Compiler for JACA 2 Homebrew CPU v.0.1.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TxtCsSourceCode;
        private System.Windows.Forms.Label LblMsgCsSourceCode;
        private System.Windows.Forms.TextBox TxtOutputMachineCode;
        private System.Windows.Forms.TextBox TxtTokens;
        private System.Windows.Forms.TextBox TxtAssemblyCommands;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtVariables;
    }
}

