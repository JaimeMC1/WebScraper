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
            BtnUpdProfile = new Button();
            btnUpdStats = new Button();
            btnUpdMatch = new Button();
            label2 = new Label();
            DevStatsTxt = new Label();
            SuspendLayout();
            // 
            // BtnUpdProfile
            // 
            BtnUpdProfile.Location = new Point(26, 55);
            BtnUpdProfile.Name = "BtnUpdProfile";
            BtnUpdProfile.Size = new Size(120, 45);
            BtnUpdProfile.TabIndex = 0;
            BtnUpdProfile.Text = "Update Profile";
            BtnUpdProfile.UseVisualStyleBackColor = true;
            BtnUpdProfile.Click += btnUpdProfile_Click;
            // 
            // btnUpdStats
            // 
            btnUpdStats.Location = new Point(26, 157);
            btnUpdStats.Name = "btnUpdStats";
            btnUpdStats.Size = new Size(120, 45);
            btnUpdStats.TabIndex = 1;
            btnUpdStats.Text = "Update stats";
            btnUpdStats.UseVisualStyleBackColor = true;
            btnUpdStats.Click += btnUpdStats_Click;
            // 
            // btnUpdMatch
            // 
            btnUpdMatch.Location = new Point(26, 106);
            btnUpdMatch.Name = "btnUpdMatch";
            btnUpdMatch.Size = new Size(120, 45);
            btnUpdMatch.TabIndex = 2;
            btnUpdMatch.Text = "Update 10";
            btnUpdMatch.UseVisualStyleBackColor = true;
            btnUpdMatch.Click += btnUpdMatch_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(289, 23);
            label2.Name = "label2";
            label2.Size = new Size(99, 20);
            label2.TabIndex = 4;
            label2.Text = "Develop stats";
            // 
            // DevStatsTxt
            // 
            DevStatsTxt.AutoSize = true;
            DevStatsTxt.Location = new Point(289, 55);
            DevStatsTxt.Name = "DevStatsTxt";
            DevStatsTxt.Size = new Size(50, 20);
            DevStatsTxt.TabIndex = 5;
            DevStatsTxt.Text = "label3";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(691, 450);
            Controls.Add(DevStatsTxt);
            Controls.Add(label2);
            Controls.Add(btnUpdMatch);
            Controls.Add(btnUpdStats);
            Controls.Add(BtnUpdProfile);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnUpdProfile;
        private Button btnUpdStats;
        private Button btnUpdMatch;
        private Label label2;
        private Label DevStatsTxt;
    }
}