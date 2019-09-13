namespace Memory
{
    partial class MirrorWindow
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
            this.tbMirror = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbMirror
            // 
            this.tbMirror.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMirror.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.tbMirror.Location = new System.Drawing.Point(13, 13);
            this.tbMirror.Name = "tbMirror";
            this.tbMirror.Size = new System.Drawing.Size(382, 34);
            this.tbMirror.TabIndex = 0;
            // 
            // MirrorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 61);
            this.Controls.Add(this.tbMirror);
            this.Name = "MirrorWindow";
            this.Text = "MirrorWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbMirror;
    }
}