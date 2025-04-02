namespace RyocatLauncher
{
    partial class SettingForm
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
            btnChangeAccount = new Button();
            btnLicense = new Button();
            trackBar1 = new TrackBar();
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            label3 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            // 
            // btnChangeAccount
            // 
            btnChangeAccount.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnChangeAccount.Location = new Point(12, 203);
            btnChangeAccount.Name = "btnChangeAccount";
            btnChangeAccount.Size = new Size(548, 58);
            btnChangeAccount.TabIndex = 2;
            btnChangeAccount.Text = "계정 변경";
            btnChangeAccount.UseVisualStyleBackColor = true;
            btnChangeAccount.Click += btnChangeAccount_Click;
            // 
            // btnLicense
            // 
            btnLicense.Location = new Point(12, 276);
            btnLicense.Name = "btnLicense";
            btnLicense.Size = new Size(75, 23);
            btnLicense.TabIndex = 3;
            btnLicense.Text = "라이센스";
            btnLicense.UseVisualStyleBackColor = true;
            btnLicense.Click += btnLicense_Click;
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(23, 108);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(526, 45);
            trackBar1.TabIndex = 4;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(100, 77);
            label1.Name = "label1";
            label1.Size = new Size(0, 15);
            label1.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 77);
            label2.Name = "label2";
            label2.Size = new Size(78, 15);
            label2.TabIndex = 6;
            label2.Text = "메모리 할당 :";
            // 
            // button1
            // 
            button1.Location = new Point(485, 74);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 7;
            button1.Text = "저장";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(23, 147);
            label3.Name = "label3";
            label3.Size = new Size(53, 15);
            label3.TabIndex = 8;
            label3.Text = "1024MB";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(492, 147);
            label4.Name = "label4";
            label4.Size = new Size(60, 15);
            label4.TabIndex = 9;
            label4.Text = "16384MB";
            // 
            // SettingForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(572, 312);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(trackBar1);
            Controls.Add(btnLicense);
            Controls.Add(btnChangeAccount);
            Name = "SettingForm";
            Text = "설정";
            Load += SettingForm_Load;
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnChangeAccount;
        private Button btnLicense;
        private TrackBar trackBar1;
        private Label label1;
        private Label label2;
        private Button button1;
        private Label label3;
        private Label label4;
    }
}