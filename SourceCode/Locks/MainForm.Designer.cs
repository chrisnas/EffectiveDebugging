namespace Locks
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
            this.btnDeadlock = new System.Windows.Forms.Button();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnDeadlock
            // 
            this.btnDeadlock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeadlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.btnDeadlock.Location = new System.Drawing.Point(13, 13);
            this.btnDeadlock.Name = "btnDeadlock";
            this.btnDeadlock.Size = new System.Drawing.Size(391, 48);
            this.btnDeadlock.TabIndex = 0;
            this.btnDeadlock.Text = "Start dead(good)lock";
            this.btnDeadlock.UseVisualStyleBackColor = true;
            this.btnDeadlock.Click += new System.EventHandler(this.btnDeadlock_Click);
            // 
            // tbResult
            // 
            this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResult.Enabled = false;
            this.tbResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.tbResult.Location = new System.Drawing.Point(13, 68);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.Size = new System.Drawing.Size(391, 53);
            this.tbResult.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 133);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.btnDeadlock);
            this.Name = "MainForm";
            this.Text = "Locks";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDeadlock;
        private System.Windows.Forms.TextBox tbResult;
    }
}

