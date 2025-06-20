namespace RyocatLauncher
{
    partial class AccountForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountForm));
            label1 = new Label();
            label2 = new Label();
            flAccounts = new FlowLayoutPanel();
            lbNoAccountInfo = new Label();
            btnNewAccount = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(71, 40);
            label1.Name = "label1";
            label1.Size = new Size(283, 32);
            label1.TabIndex = 0;
            label1.Text = "YUMC Ryocat Launcher";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 84);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 1;
            label2.Text = "계정 : ";
            // 
            // flAccounts
            // 
            flAccounts.Location = new Point(12, 102);
            flAccounts.Name = "flAccounts";
            flAccounts.Size = new Size(400, 297);
            flAccounts.TabIndex = 2;
            // 
            // lbNoAccountInfo
            // 
            lbNoAccountInfo.AutoSize = true;
            lbNoAccountInfo.Location = new Point(69, 235);
            lbNoAccountInfo.Name = "lbNoAccountInfo";
            lbNoAccountInfo.Size = new Size(287, 30);
            lbNoAccountInfo.TabIndex = 5;
            lbNoAccountInfo.Text = "'새 계정 등록' 을 눌러 계정을 추가하세요\r\n로그인 화면을 불러오는 데 시간이 걸릴 수 있습니다";
            lbNoAccountInfo.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnNewAccount
            // 
            btnNewAccount.Location = new Point(12, 405);
            btnNewAccount.Name = "btnNewAccount";
            btnNewAccount.Size = new Size(400, 65);
            btnNewAccount.TabIndex = 3;
            btnNewAccount.Text = "새 계정 등록";
            btnNewAccount.UseVisualStyleBackColor = true;
            btnNewAccount.Click += btnNewAccount_Click;
            // 
            // AccountForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(424, 481);
            Controls.Add(lbNoAccountInfo);
            Controls.Add(btnNewAccount);
            Controls.Add(flAccounts);
            Controls.Add(label2);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AccountForm";
            FormClosing += AccountForm_FormClosing;
            Load += AccountForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private FlowLayoutPanel flAccounts;
        private Button btnNewAccount;
        private Label lbNoAccountInfo;
    }
}