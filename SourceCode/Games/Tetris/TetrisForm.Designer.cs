namespace DebugMeow.Games.Tetris
{
    partial class TetrisForm
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
            this.GameArea = new System.Windows.Forms.PictureBox();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LabelScore = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LabelDelay = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GameArea)).BeginInit();
            this.SuspendLayout();
            // 
            // GameArea
            // 
            this.GameArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GameArea.Location = new System.Drawing.Point(12, 12);
            this.GameArea.Name = "GameArea";
            this.GameArea.Size = new System.Drawing.Size(300, 600);
            this.GameArea.TabIndex = 0;
            this.GameArea.TabStop = false;
            this.GameArea.Paint += new System.Windows.Forms.PaintEventHandler(this.GameArea_Paint);
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(324, 12);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(96, 30);
            this.ButtonStart.TabIndex = 1;
            this.ButtonStart.Text = "New game";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(321, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Score:";
            // 
            // LabelScore
            // 
            this.LabelScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelScore.Location = new System.Drawing.Point(388, 57);
            this.LabelScore.Name = "LabelScore";
            this.LabelScore.Size = new System.Drawing.Size(182, 30);
            this.LabelScore.TabIndex = 3;
            this.LabelScore.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(322, 599);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Delay between frames: ";
            // 
            // LabelDelay
            // 
            this.LabelDelay.AutoSize = true;
            this.LabelDelay.Location = new System.Drawing.Point(436, 599);
            this.LabelDelay.Name = "LabelDelay";
            this.LabelDelay.Size = new System.Drawing.Size(0, 13);
            this.LabelDelay.TabIndex = 5;
            // 
            // TetrisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 637);
            this.Controls.Add(this.LabelDelay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LabelScore);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ButtonStart);
            this.Controls.Add(this.GameArea);
            this.Name = "TetrisForm";
            this.Text = "Tetris";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TetrisForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.GameArea)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox GameArea;
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelScore;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LabelDelay;
    }
}