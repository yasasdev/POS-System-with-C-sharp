namespace PARAGON_MOTORS
{
    partial class DisplayUserForm
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
            this.pictureboxHOME = new System.Windows.Forms.PictureBox();
            this.pictureboxADMIN = new System.Windows.Forms.PictureBox();
            this.pictureboxHOTLINE = new System.Windows.Forms.PictureBox();
            this.lblCLOSE = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.labelDateTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureboxHOME)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureboxADMIN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureboxHOTLINE)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureboxHOME
            // 
            this.pictureboxHOME.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureboxHOME.Image = global::PARAGON_MOTORS.Properties.Resources._254967;
            this.pictureboxHOME.Location = new System.Drawing.Point(941, 12);
            this.pictureboxHOME.Name = "pictureboxHOME";
            this.pictureboxHOME.Size = new System.Drawing.Size(53, 49);
            this.pictureboxHOME.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureboxHOME.TabIndex = 33;
            this.pictureboxHOME.TabStop = false;
            this.pictureboxHOME.Click += new System.EventHandler(this.pictureboxHOME_Click);
            // 
            // pictureboxADMIN
            // 
            this.pictureboxADMIN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureboxADMIN.Image = global::PARAGON_MOTORS.Properties.Resources.abc;
            this.pictureboxADMIN.Location = new System.Drawing.Point(868, 12);
            this.pictureboxADMIN.Name = "pictureboxADMIN";
            this.pictureboxADMIN.Size = new System.Drawing.Size(53, 49);
            this.pictureboxADMIN.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureboxADMIN.TabIndex = 31;
            this.pictureboxADMIN.TabStop = false;
            // 
            // pictureboxHOTLINE
            // 
            this.pictureboxHOTLINE.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureboxHOTLINE.Image = global::PARAGON_MOTORS.Properties.Resources._483947;
            this.pictureboxHOTLINE.Location = new System.Drawing.Point(1010, 13);
            this.pictureboxHOTLINE.Name = "pictureboxHOTLINE";
            this.pictureboxHOTLINE.Size = new System.Drawing.Size(53, 49);
            this.pictureboxHOTLINE.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureboxHOTLINE.TabIndex = 32;
            this.pictureboxHOTLINE.TabStop = false;
            // 
            // lblCLOSE
            // 
            this.lblCLOSE.AutoSize = true;
            this.lblCLOSE.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblCLOSE.Font = new System.Drawing.Font("Algerian", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCLOSE.ForeColor = System.Drawing.Color.Black;
            this.lblCLOSE.Location = new System.Drawing.Point(1102, 9);
            this.lblCLOSE.Name = "lblCLOSE";
            this.lblCLOSE.Size = new System.Drawing.Size(26, 24);
            this.lblCLOSE.TabIndex = 28;
            this.lblCLOSE.Text = "X";
            this.lblCLOSE.Click += new System.EventHandler(this.lblCLOSE_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Location = new System.Drawing.Point(10, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1116, 5);
            this.panel1.TabIndex = 34;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 100);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1106, 337);
            this.listView1.TabIndex = 35;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // labelDateTime
            // 
            this.labelDateTime.AutoSize = true;
            this.labelDateTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelDateTime.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDateTime.ForeColor = System.Drawing.Color.Black;
            this.labelDateTime.Location = new System.Drawing.Point(13, 40);
            this.labelDateTime.Name = "labelDateTime";
            this.labelDateTime.Size = new System.Drawing.Size(62, 16);
            this.labelDateTime.TabIndex = 37;
            this.labelDateTime.Text = "Date&Time";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label1.Font = new System.Drawing.Font("Elephant", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 21);
            this.label1.TabIndex = 36;
            this.label1.Text = "SAKURA MOBILE";
            // 
            // DisplayUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1138, 449);
            this.Controls.Add(this.labelDateTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureboxHOME);
            this.Controls.Add(this.pictureboxADMIN);
            this.Controls.Add(this.pictureboxHOTLINE);
            this.Controls.Add(this.lblCLOSE);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DisplayUserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DisplayUserForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DisplayUserForm_FormClosing);
            this.Load += new System.EventHandler(this.DisplayUserForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureboxHOME)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureboxADMIN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureboxHOTLINE)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureboxHOME;
        private System.Windows.Forms.PictureBox pictureboxADMIN;
        private System.Windows.Forms.PictureBox pictureboxHOTLINE;
        private System.Windows.Forms.Label lblCLOSE;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label labelDateTime;
        private System.Windows.Forms.Label label1;
    }
}