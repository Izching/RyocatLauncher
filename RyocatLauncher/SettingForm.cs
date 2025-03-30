using CmlLib.Core;
using MetroFramework.Components;
using System.Diagnostics;

namespace RyocatLauncher;

public partial class SettingForm : MetroFramework.Forms.MetroForm
{

    // 메모리 설정 관련 변수
    private const int MinMemory = 1024; // 최소 메모리 1GB
    private const int MaxMemory = 16384; // 최대 메모리 16GB

    public int SelectedMemory { get; private set; } = 4096; // 기본값 4GB

    public SettingForm()
    {
        InitializeComponent();
    }

    public event EventHandler? FormClosingRequired;

    private void SettingForm_Load(object sender, EventArgs e)
    {
        trackBar1.Minimum = MinMemory / 1024;
        trackBar1.Maximum = MaxMemory / 1024;
        trackBar1.Value = SelectedMemory / 1024;

        label1.Text = $"{SelectedMemory} MB";
    }

    private void btnChangeAccount_Click(object sender, EventArgs e)
    {
        var form = new AccountForm();
        form.AutoLogin = false;
        form.Show();
        this.Close();
        FormClosingRequired?.Invoke(this, e);
    }

    private void btnLicense_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "https://github.com/Izching/RyoketLauncher/blob/main/LICENSE",
            UseShellExecute = true,
        });
    }

    private void trackBar1_Scroll(object sender, EventArgs e)
    {
        SelectedMemory = trackBar1.Value * 1024;
        label1.Text = $"{SelectedMemory} MB";
    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void label2_Click(object sender, EventArgs e)
    {

    }

    private void button1_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
