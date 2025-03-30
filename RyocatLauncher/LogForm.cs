namespace RyocatLauncher;

public partial class LogForm : Form
{
    public LogForm()
    {
        InitializeComponent();
    }

    bool showLog = false;

    public void AppendLog(string message)
    {
        if (!showLog) return;
        this.Invoke(() =>
        {
            if (!showLog) return;
            richTextBox1.AppendText(message);
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();
        });
    }

    private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        e.Cancel = true;
        this.Hide();
    }

    private void LogForm_Shown(object sender, EventArgs e)
    {
        showLog = true;
    }

}
