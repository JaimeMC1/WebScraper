namespace WebScraper
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
            btnUpdProfile = new Button();
            btnUpdStats = new Button();
            btnUpdMatch = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // btnUpdProfile
            // 
            btnUpdProfile.Location = new Point(114, 57);
            btnUpdProfile.Name = "btnUpdProfile";
            btnUpdProfile.Size = new Size(120, 29);
            btnUpdProfile.TabIndex = 0;
            btnUpdProfile.Text = "Update Profile";
            btnUpdProfile.UseVisualStyleBackColor = true;
            btnUpdProfile.Click += btnUpdProfile_Click;
            // 
            // btnUpdStats
            // 
            btnUpdStats.Location = new Point(114, 187);
            btnUpdStats.Name = "btnUpdStats";
            btnUpdStats.Size = new Size(120, 29);
            btnUpdStats.TabIndex = 1;
            btnUpdStats.Text = "Update stats";
            btnUpdStats.UseVisualStyleBackColor = true;
            btnUpdStats.Click += btnUpdStats_Click;
            // 
            // btnUpdMatch
            // 
            btnUpdMatch.Location = new Point(114, 124);
            btnUpdMatch.Name = "btnUpdMatch";
            btnUpdMatch.Size = new Size(120, 29);
            btnUpdMatch.TabIndex = 2;
            btnUpdMatch.Text = "Update matchs";
            btnUpdMatch.UseVisualStyleBackColor = true;
            btnUpdMatch.Click += btnUpdMatch_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(113, 13);
            label1.Name = "label1";
            label1.Size = new Size(103, 20);
            label1.TabIndex = 3;
            label1.Text = "Carlos Alzaraz";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(691, 450);
            Controls.Add(label1);
            Controls.Add(btnUpdMatch);
            Controls.Add(btnUpdStats);
            Controls.Add(btnUpdProfile);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnUpdProfile;
        private Button btnUpdStats;
        private Button btnUpdMatch;
        private Label label1;
    }
}