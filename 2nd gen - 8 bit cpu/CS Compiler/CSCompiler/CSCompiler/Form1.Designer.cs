namespace CSCompiler
{
    partial class Form1
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
            this.SuspendLayout();
            // 
            // TxtCsSourceCode
            // 
            this.TxtCsSourceCode.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtCsSourceCode.Location = new System.Drawing.Point(12, 113);
            this.TxtCsSourceCode.Multiline = true;
            this.TxtCsSourceCode.Name = "TxtCsSourceCode";
            this.TxtCsSourceCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtCsSourceCode.Size = new System.Drawing.Size(435, 477);
            this.TxtCsSourceCode.TabIndex = 0;
            this.TxtCsSourceCode.TextChanged += new System.EventHandler(this.TxtCsSourceCode_TextChanged);
            // 
            // LblMsgCsSourceCode
            // 
            this.LblMsgCsSourceCode.AutoSize = true;
            this.LblMsgCsSourceCode.Location = new System.Drawing.Point(12, 593);
            this.LblMsgCsSourceCode.Name = "LblMsgCsSourceCode";
            this.LblMsgCsSourceCode.Size = new System.Drawing.Size(0, 13);
            this.LblMsgCsSourceCode.TabIndex = 1;
            // 
            // TxtOutputMachineCode
            // 
            this.TxtOutputMachineCode.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtOutputMachineCode.Location = new System.Drawing.Point(1014, 113);
            this.TxtOutputMachineCode.Multiline = true;
            this.TxtOutputMachineCode.Name = "TxtOutputMachineCode";
            this.TxtOutputMachineCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtOutputMachineCode.Size = new System.Drawing.Size(87, 477);
            this.TxtOutputMachineCode.TabIndex = 2;
            // 
            // TxtTokens
            // 
            this.TxtTokens.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtTokens.Location = new System.Drawing.Point(453, 113);
            this.TxtTokens.Multiline = true;
            this.TxtTokens.Name = "TxtTokens";
            this.TxtTokens.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtTokens.Size = new System.Drawing.Size(249, 477);
            this.TxtTokens.TabIndex = 3;
            // 
            // TxtAssemblyCommands
            // 
            this.TxtAssemblyCommands.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtAssemblyCommands.Location = new System.Drawing.Point(708, 113);
            this.TxtAssemblyCommands.Multiline = true;
            this.TxtAssemblyCommands.Name = "TxtAssemblyCommands";
            this.TxtAssemblyCommands.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtAssemblyCommands.Size = new System.Drawing.Size(300, 477);
            this.TxtAssemblyCommands.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 630);
            this.Controls.Add(this.TxtAssemblyCommands);
            this.Controls.Add(this.TxtTokens);
            this.Controls.Add(this.TxtOutputMachineCode);
            this.Controls.Add(this.LblMsgCsSourceCode);
            this.Controls.Add(this.TxtCsSourceCode);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TxtCsSourceCode;
        private System.Windows.Forms.Label LblMsgCsSourceCode;
        private System.Windows.Forms.TextBox TxtOutputMachineCode;
        private System.Windows.Forms.TextBox TxtTokens;
        private System.Windows.Forms.TextBox TxtAssemblyCommands;
    }
}

