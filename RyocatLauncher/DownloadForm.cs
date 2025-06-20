
namespace RyocatLauncher
{
    public partial class DownloadForm : Form
    {

        private bool downloadCompleted = false;
        private bool downloadCanceled = false;
        public DownloadForm()
        {
            InitializeComponent();
        }

        public void SetProgress(int percent)
        {
            if (downloadCanceled)
            {
                label1.Text = "다운로드 중단됨";
                return;
            }
            progressBar1.Value = percent;
            label1.Text = $"다운로드 중... {percent}%";
        }

        public void SetDownloadCompleted()
        {
            downloadCompleted = true;
            this.Close();
        }

        //private void DownloadForm_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    if (!downloadCompleted)
        //    {
        //        MessageBox.Show("다운로드가 중단되었습니다.");
        //        Environment.Exit(0);
        //    }
        //}
        private void DownloadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!downloadCompleted)
            {
                downloadCanceled = true;
                MessageBox.Show("다운로드가 중단되었습니다.");
                Environment.Exit(0);
            }
        }
    }
}
