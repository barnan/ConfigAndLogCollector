namespace ListNetworkComputers
{
    partial class frmMain
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
            this.cmbNetworkComputers = new System.Windows.Forms.ComboBox();
            this.lblNetworkComputers = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbNetworkComputers
            // 
            this.cmbNetworkComputers.FormattingEnabled = true;
            this.cmbNetworkComputers.Location = new System.Drawing.Point(25, 46);
            this.cmbNetworkComputers.Name = "cmbNetworkComputers";
            this.cmbNetworkComputers.Size = new System.Drawing.Size(213, 21);
            this.cmbNetworkComputers.TabIndex = 0;
            // 
            // lblNetworkComputers
            // 
            this.lblNetworkComputers.AutoSize = true;
            this.lblNetworkComputers.Location = new System.Drawing.Point(22, 30);
            this.lblNetworkComputers.Name = "lblNetworkComputers";
            this.lblNetworkComputers.Size = new System.Drawing.Size(146, 13);
            this.lblNetworkComputers.TabIndex = 1;
            this.lblNetworkComputers.Text = "Available Network Computers";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 109);
            this.Controls.Add(this.lblNetworkComputers);
            this.Controls.Add(this.cmbNetworkComputers);
            this.Name = "frmMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbNetworkComputers;
        private System.Windows.Forms.Label lblNetworkComputers;
    }
}

