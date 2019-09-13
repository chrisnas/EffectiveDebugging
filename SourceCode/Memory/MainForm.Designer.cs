namespace Memory
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
            this.btnMirror = new System.Windows.Forms.Button();
            this.tbSource = new System.Windows.Forms.TextBox();
            this.btnFullCollect = new System.Windows.Forms.Button();
            this.btnIDisposable = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMirror
            // 
            this.btnMirror.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMirror.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnMirror.Location = new System.Drawing.Point(14, 96);
            this.btnMirror.Name = "btnMirror";
            this.btnMirror.Size = new System.Drawing.Size(400, 43);
            this.btnMirror.TabIndex = 0;
            this.btnMirror.Text = "Mirror";
            this.btnMirror.UseVisualStyleBackColor = true;
            this.btnMirror.Click += new System.EventHandler(this.btnMirror_Click);
            // 
            // tbSource
            // 
            this.tbSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.tbSource.Location = new System.Drawing.Point(14, 145);
            this.tbSource.Name = "tbSource";
            this.tbSource.Size = new System.Drawing.Size(400, 32);
            this.tbSource.TabIndex = 1;
            // 
            // btnFullCollect
            // 
            this.btnFullCollect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFullCollect.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnFullCollect.Location = new System.Drawing.Point(13, 182);
            this.btnFullCollect.Name = "btnFullCollect";
            this.btnFullCollect.Size = new System.Drawing.Size(400, 43);
            this.btnFullCollect.TabIndex = 0;
            this.btnFullCollect.Text = "Full Collect";
            this.btnFullCollect.UseVisualStyleBackColor = true;
            this.btnFullCollect.Click += new System.EventHandler(this.btnFullCollect_Click);
            // 
            // btnIDisposable
            // 
            this.btnIDisposable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIDisposable.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnIDisposable.Location = new System.Drawing.Point(12, 12);
            this.btnIDisposable.Name = "btnIDisposable";
            this.btnIDisposable.Size = new System.Drawing.Size(400, 43);
            this.btnIDisposable.TabIndex = 0;
            this.btnIDisposable.Text = "IDisposable";
            this.btnIDisposable.UseVisualStyleBackColor = true;
            this.btnIDisposable.Click += new System.EventHandler(this.btnIDisposable_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 240);
            this.Controls.Add(this.tbSource);
            this.Controls.Add(this.btnFullCollect);
            this.Controls.Add(this.btnIDisposable);
            this.Controls.Add(this.btnMirror);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.Name = "MainForm";
            this.Text = "Memory";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMirror;
        private System.Windows.Forms.TextBox tbSource;
        private System.Windows.Forms.Button btnFullCollect;
        private System.Windows.Forms.Button btnIDisposable;
    }
}

