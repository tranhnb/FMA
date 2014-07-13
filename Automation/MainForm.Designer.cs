namespace Automation
{
    partial class MainForm
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
            this.btnStartDig = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStartDig
            // 
            this.btnStartDig.Location = new System.Drawing.Point(12, 21);
            this.btnStartDig.Name = "btnStartDig";
            this.btnStartDig.Size = new System.Drawing.Size(75, 23);
            this.btnStartDig.TabIndex = 0;
            this.btnStartDig.Text = "Start";
            this.btnStartDig.UseVisualStyleBackColor = true;
            this.btnStartDig.Click += new System.EventHandler(this.btnStartDig_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.btnStartDig);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartDig;
    }
}