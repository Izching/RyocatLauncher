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
            label1 = new Label();
            cbVersion = new ComboBox();
            pbFiles = new ProgressBar();
            pbProgress = new ProgressBar();
            lbProgress = new Label();
            btnSetting = new Button();
            btnStart = new Button();
            pAccountHolder = new Panel();
            cbFabricVersions = new ComboBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(35, 169);
            label1.Name = "label1";
            label1.Size = new Size(84, 15);
            label1.TabIndex = 0;
            label1.Text = "Select version:";
            // 
            // cbVersion
            // 
            cbVersion.FormattingEnabled = true;
            cbVersion.Location = new Point(35, 187);
            cbVersion.Name = "cbVersion";
            cbVersion.Size = new Size(353, 23);
            cbVersion.TabIndex = 1;
            cbVersion.Text = "cbVersion";
            cbVersion.SelectedIndexChanged += cbVersion_SelectedIndexChanged;
            // 
            // pbFiles
            // 
            pbFiles.Location = new Point(35, 329);
            pbFiles.Name = "pbFiles";
            pbFiles.Size = new Size(353, 23);
            pbFiles.TabIndex = 3;
            pbFiles.Click += pbFiles_Click;
            // 
            // pbProgress
            // 
            pbProgress.Location = new Point(35, 358);
            pbProgress.Name = "pbProgress";
            pbProgress.Size = new Size(353, 23);
            pbProgress.TabIndex = 4;
            // 
            // lbProgress
            // 
            lbProgress.AutoSize = true;
            lbProgress.Location = new Point(35, 311);
            lbProgress.Name = "lbProgress";
            lbProgress.Size = new Size(39, 15);
            lbProgress.TabIndex = 5;
            lbProgress.Text = "label2";
            // 
            // btnSetting
            // 
            btnSetting.Location = new Point(313, 158);
            btnSetting.Name = "btnSetting";
            btnSetting.Size = new Size(75, 23);
            btnSetting.TabIndex = 6;
            btnSetting.Text = "Setting";
            btnSetting.UseVisualStyleBackColor = true;
            btnSetting.Click += btnSetting_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(35, 255);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(353, 53);
            btnStart.TabIndex = 7;
            btnStart.Text = "Game Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // pAccountHolder
            // 
            pAccountHolder.Location = new Point(10, 52);
            pAccountHolder.Name = "pAccountHolder";
            pAccountHolder.Size = new Size(400, 100);
            pAccountHolder.TabIndex = 8;
            pAccountHolder.Paint += pAccountHolder_Paint;
            // 
            // cbFabricVersions
            // 
            cbFabricVersions.FormattingEnabled = true;
            cbFabricVersions.Location = new Point(35, 216);
            cbFabricVersions.Name = "cbFabricVersions";
            cbFabricVersions.Size = new Size(353, 23);
            cbFabricVersions.TabIndex = 9;
            // 
            // LauncherForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(422, 409);
            Controls.Add(cbFabricVersions);
            Controls.Add(pAccountHolder);
            Controls.Add(btnStart);
            Controls.Add(btnSetting);
            Controls.Add(lbProgress);
            Controls.Add(pbProgress);
            Controls.Add(pbFiles);
            Controls.Add(cbVersion);
            Controls.Add(label1);
            Name = "LauncherForm";
            Text = "YUMC 서버 접속기";
            FormClosing += LauncherForm_FormClosing;
            Load += LauncherForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cbVersion;
        private ProgressBar pbFiles;
        private ProgressBar pbProgress;
        private Label lbProgress;
        private Button btnSetting;
        private Button btnStart;
        private Panel pAccountHolder;
        private ComboBox cbFabricVersions;
    }
}