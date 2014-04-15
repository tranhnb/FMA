namespace Automation
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
            this.button1 = new System.Windows.Forms.Button();
            this.btnInstallApp = new System.Windows.Forms.Button();
            this.btnTakeScreenShot = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(194, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Access Virtual Machine";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnInstallApp
            // 
            this.btnInstallApp.Location = new System.Drawing.Point(12, 41);
            this.btnInstallApp.Name = "btnInstallApp";
            this.btnInstallApp.Size = new System.Drawing.Size(194, 23);
            this.btnInstallApp.TabIndex = 1;
            this.btnInstallApp.Text = "Install Application";
            this.btnInstallApp.UseVisualStyleBackColor = true;
            this.btnInstallApp.Click += new System.EventHandler(this.btnInstallApp_Click);
            // 
            // btnTakeScreenShot
            // 
            this.btnTakeScreenShot.Location = new System.Drawing.Point(12, 70);
            this.btnTakeScreenShot.Name = "btnTakeScreenShot";
            this.btnTakeScreenShot.Size = new System.Drawing.Size(194, 23);
            this.btnTakeScreenShot.TabIndex = 1;
            this.btnTakeScreenShot.Text = "Take ScreenShot";
            this.btnTakeScreenShot.UseVisualStyleBackColor = true;
            this.btnTakeScreenShot.Click += new System.EventHandler(this.btnTakeScreenShot_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 266);
            this.Controls.Add(this.btnTakeScreenShot);
            this.Controls.Add(this.btnInstallApp);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Testing Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnInstallApp;
        private System.Windows.Forms.Button btnTakeScreenShot;
    }
}

