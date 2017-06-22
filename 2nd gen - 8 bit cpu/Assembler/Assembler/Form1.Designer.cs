namespace Assembler
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
            this.textBoxAssembly = new System.Windows.Forms.TextBox();
            this.textBoxBytes = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxAssembly
            // 
            this.textBoxAssembly.Location = new System.Drawing.Point(154, 12);
            this.textBoxAssembly.Multiline = true;
            this.textBoxAssembly.Name = "textBoxAssembly";
            this.textBoxAssembly.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxAssembly.Size = new System.Drawing.Size(504, 369);
            this.textBoxAssembly.TabIndex = 0;
            this.textBoxAssembly.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBoxBytes
            // 
            this.textBoxBytes.Location = new System.Drawing.Point(13, 13);
            this.textBoxBytes.Multiline = true;
            this.textBoxBytes.Name = "textBoxBytes";
            this.textBoxBytes.Size = new System.Drawing.Size(132, 367);
            this.textBoxBytes.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 445);
            this.Controls.Add(this.textBoxBytes);
            this.Controls.Add(this.textBoxAssembly);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxAssembly;
        private System.Windows.Forms.TextBox textBoxBytes;
    }
}

