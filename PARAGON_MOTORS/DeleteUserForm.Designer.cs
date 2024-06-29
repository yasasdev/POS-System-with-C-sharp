namespace PARAGON_MOTORS
{
    partial class DeleteUserForm
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
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblCLOSE = new System.Windows.Forms.Label();
            this.pictureboxHOME = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureboxHOME)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFirstName
            // 
            this.txtFirstName.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.Location = new System.Drawing.Point(95, 97);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(253, 23);
            this.txtFirstName.TabIndex = 192;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(94, 77);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(84, 17);
            this.label18.TabIndex = 191;
            this.label18.Text = "Enter Name";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(92, 40);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(147, 19);
            this.label17.TabIndex = 190;
            this.label17.Text = "Delete User details";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.IndianRed;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(97, 142);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(251, 28);
            this.button1.TabIndex = 193;
            this.button1.Text = "Delete";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblCLOSE
            // 
            this.lblCLOSE.AutoSize = true;
            this.lblCLOSE.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblCLOSE.Font = new System.Drawing.Font("Algerian", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCLOSE.ForeColor = System.Drawing.Color.Black;
            this.lblCLOSE.Location = new System.Drawing.Point(384, 3);
            this.lblCLOSE.Name = "lblCLOSE";
            this.lblCLOSE.Size = new System.Drawing.Size(26, 24);
            this.lblCLOSE.TabIndex = 211;
            this.lblCLOSE.Text = "X";
            this.lblCLOSE.Click += new System.EventHandler(this.lblCLOSE_Click);
            // 
            // pictureboxHOME
            // 
            this.pictureboxHOME.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureboxHOME.Image = global::PARAGON_MOTORS.Properties.Resources._254967;
            this.pictureboxHOME.Location = new System.Drawing.Point(316, 12);
            this.pictureboxHOME.Name = "pictureboxHOME";
            this.pictureboxHOME.Size = new System.Drawing.Size(53, 49);
            this.pictureboxHOME.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureboxHOME.TabIndex = 212;
            this.pictureboxHOME.TabStop = false;
            this.pictureboxHOME.Click += new System.EventHandler(this.pictureboxHOME_Click);
            // 
            // DeleteUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(413, 226);
            this.Controls.Add(this.pictureboxHOME);
            this.Controls.Add(this.lblCLOSE);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DeleteUserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DeleteUserForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeleteUserForm_FormClosing);
            this.Load += new System.EventHandler(this.DeleteUserForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureboxHOME)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblCLOSE;
        private System.Windows.Forms.PictureBox pictureboxHOME;
    }
}