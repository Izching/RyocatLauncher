namespace MyCustomLauncher
{
    partial class LauncherForm
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
            pbFiles = new ProgressBar();
            pbProgress = new ProgressBar();
            lbProgress = new Label();
            btnSetting = new Button();
            btnStart = new Button();
            pAccountHolder = new Panel();
            btnOpenLog = new Button();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // pbFiles
            // 
            pbFiles.Location = new Point(35, 307);
            pbFiles.Name = "pbFiles";
            pbFiles.Size = new Size(353, 23);
            pbFiles.TabIndex = 3;
            pbFiles.Click += pbFiles_Click;
            // 
            // pbProgress
            // 
            pbProgress.Location = new Point(35, 336);
            pbProgress.Name = "pbProgress";
            pbProgress.Size = new Size(353, 23);
            pbProgress.TabIndex = 4;
            // 
            // lbProgress
            // 
            lbProgress.AutoSize = true;
            lbProgress.Location = new Point(35, 289);
            lbProgress.Name = "lbProgress";
            lbProgress.Size = new Size(0, 15);
            lbProgress.TabIndex = 5;
            // 
            // btnSetting
            // 
            btnSetting.Location = new Point(313, 198);
            btnSetting.Name = "btnSetting";
            btnSetting.Size = new Size(75, 23);
            btnSetting.TabIndex = 6;
            btnSetting.Text = "설정";
            btnSetting.UseVisualStyleBackColor = true;
            btnSetting.Click += btnSetting_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(35, 233);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(353, 53);
            btnStart.TabIndex = 7;
            btnStart.Text = "게임 시작";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // pAccountHolder
            // 
            pAccountHolder.Location = new Point(10, 73);
            pAccountHolder.Name = "pAccountHolder";
            pAccountHolder.Size = new Size(400, 100);
            pAccountHolder.TabIndex = 8;
            pAccountHolder.Paint += pAccountHolder_Paint;
            // 
            // btnOpenLog
            // 
            btnOpenLog.Location = new Point(35, 198);
            btnOpenLog.Name = "btnOpenLog";
            btnOpenLog.Size = new Size(75, 23);
            btnOpenLog.TabIndex = 9;
            btnOpenLog.Text = "로그 열기";
            btnOpenLog.UseVisualStyleBackColor = true;
            btnOpenLog.Click += btnOpenLog_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 24F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(23, 21);
            label1.Name = "label1";
            label1.Size = new Size(113, 45);
            label1.TabIndex = 10;
            label1.Text = "YUMC";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(128, 33);
            label2.Name = "label2";
            label2.Size = new Size(178, 30);
            label2.TabIndex = 11;
            label2.Text = "Ryocat Launcher";
            // 
            // LauncherForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(422, 386);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnOpenLog);
            Controls.Add(pAccountHolder);
            Controls.Add(btnStart);
            Controls.Add(btnSetting);
            Controls.Add(lbProgress);
            Controls.Add(pbProgress);
            Controls.Add(pbFiles);
            Name = "LauncherForm";
            FormClosing += LauncherForm_FormClosing;
            Load += LauncherForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ProgressBar pbFiles;
        private ProgressBar pbProgress;
        private Label lbProgress;
        private Button btnSetting;
        private Button btnStart;
        private Panel pAccountHolder;
        private Button btnOpenLog;
        private Label label1;
        private Label label2;
    }
}