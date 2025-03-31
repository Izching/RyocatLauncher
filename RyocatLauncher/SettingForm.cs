using System.Diagnostics;

namespace RyocatLauncher;

public partial class SettingForm : MetroFramework.Forms.MetroForm
{

    // 메모리 설정 관련 변수
    private const int MinMemory = 1024; // 최소 메모리 1GB
    private const int MaxMemory = 16384; // 최대 메모리 16GB

    public int SelectedMemory { get; private set; } = 4096; // 기본값 4GB

    public SettingForm(int initialMemoryMb)
    {
        SelectedMemory = initialMemoryMb;
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
        string licenseText = @"
MIT License

Copyright (c) 2020 권세인(AlphaBs)
Copyright (c) 2023 CmlLib
Copyright (c) 2025 Izching

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
";
        MessageBox.Show(licenseText, "LICENSE");
    }

    private void trackBar1_Scroll(object sender, EventArgs e)
    {
        SelectedMemory = trackBar1.Value * 1024;
        label1.Text = $"{SelectedMemory} MB";
    }

    private void button1_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
