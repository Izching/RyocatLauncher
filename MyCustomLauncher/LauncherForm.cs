using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Version;
using CmlLib.Utils;
using CmlLib.Core.Installer.FabricMC;
using System.Net;
using System.IO.Compression;
using System.Security.Cryptography;

namespace MyCustomLauncher;

public partial class LauncherForm : MetroFramework.Forms.MetroForm
{
    private readonly MSession _session;
    private readonly CMLauncher _launcher;

    private bool exitOnClose = true;
    private const string ModpackUrl = "http://mc.yu.ac.kr/ryoket/ryoket.zip";
    private int selectedMemoryMb = 4096;

    public LauncherForm(MSession session)
    {
        _session = session;

        // 사용자 AppData/Roaming 경로 가져오기
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        // MyLauncher 폴더 경로 설정
        string myLauncherPath = Path.Combine(appDataPath, "MyLauncher");


        // 폴더가 없으면 생성하기
        if (!Directory.Exists(myLauncherPath))
        {
            Directory.CreateDirectory(myLauncherPath);
        }

        // MinecraftPath 객체를 MyLauncher 폴더로 설정
        var customPath = new MinecraftPath(myLauncherPath);

        _launcher = new CMLauncher(customPath);

        InitializeComponent();
    }

    private void LauncherForm_Load(object sender, EventArgs e)
    {
        showAccountControl();
        //await listVersions();
        //await listFabricVersions();
    }

    private async Task listVersions()
    {
        var version = await _launcher.GetVersionAsync("1.21.1");

        cbVersion.Items.Clear();

        cbVersion.Text = "1.21.1";
    }



    private async Task listFabricVersions()
    {
        var fabricVersionLoader = new FabricVersionLoader();
        var fabricVersions = await fabricVersionLoader.GetVersionMetadatasAsync();
        var targetVersion = fabricVersions.FirstOrDefault(v => v.Name == "fabric-loader-0.16.10-1.21.1");

        cbFabricVersions.Items.Clear();
        cbFabricVersions.Text = "fabric-loader-0.16.10-1.21.1";

        ////fabric 버전 확인용
        //var fabricVersionLoader = new FabricVersionLoader();
        //var fabricVersions = await fabricVersionLoader.GetVersionMetadatasAsync();

        //cbFabricVersions.Items.Clear();
        //foreach (var v in fabricVersions)
        //{
        //    cbFabricVersions.Items.Add(v.Name);
        //}

        //if (fabricVersions.Count() > 0)
        //{
        //    cbFabricVersions.SelectedIndex = 0;
        //}
    }

    private void showAccountControl()
    {
        var control = new AccountControl(_session);
        pAccountHolder.Controls.Clear();
        pAccountHolder.Controls.Add(control);
    }

    private async void btnStart_Click(object sender, EventArgs e)
    {
        this.Enabled = false;
        const string FabricVersionName = "fabric-loader-0.16.10-1.21.1"; // 고정된 Fabric 버전 이름
        var path = _launcher.MinecraftPath;

        try
        {

            string basePath = _launcher.MinecraftPath.BasePath;
            var fabricVersionPath = Path.Combine(path.Versions, FabricVersionName);
            var fabricInstalled = Directory.Exists(fabricVersionPath);

            var fabricVersionLoader = new FabricVersionLoader();
            var fabricVersions = await fabricVersionLoader.GetVersionMetadatasAsync();
            var fabric = fabricVersions.GetVersionMetadata(FabricVersionName);

            await fabric.SaveAsync(path);
            fabricInstalled = true;

            var fabricJarPath = Path.Combine(path.Versions, "fabric-loader-0.16.10-1.21.1.jar");

            await DownloadAndUpdateFiles(basePath);


            var process = await _launcher.CreateProcessAsync(FabricVersionName, new MLaunchOption
            {
                Session = _session,
                ServerIp = "mc.yu.ac.kr",
                ServerPort = 25568,
                MaximumRamMb = selectedMemoryMb // 메모리 설정
            });

            var logForm = new LogForm();
            logForm.Show();

            var processUtil = new ProcessUtil(process);
            processUtil.OutputReceived += (s, e) => logForm.AppendLog(e);
            processUtil.StartWithEvents();

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        this.Enabled = true;
    }

    private void _launcher_ProgressChanged(object? sender, System.ComponentModel.ProgressChangedEventArgs e)
    {
        pbProgress.Maximum = 100;
        pbProgress.Value = e.ProgressPercentage;
    }


    private void btnSetting_Click(object sender, EventArgs e)
    {
        var form = new SettingForm();
        form.FormClosingRequired += (s, e) =>
        {
            exitOnClose = false;
            this.Close();
        };
        form.ShowDialog();
        selectedMemoryMb = form.SelectedMemory;
    }

    private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (exitOnClose)
            Environment.Exit(0);
    }

    private void pAccountHolder_Paint(object sender, PaintEventArgs e)
    {

    }

    private void cbVersion_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void pbFiles_Click(object sender, EventArgs e)
    {

    }

    private async Task DownloadAndUpdateFiles(string basePath)
    {
        string downloadedZipPath = Path.Combine(basePath, "temp.zip");
        string hashFilePath = Path.Combine(basePath, "ziphash.txt");

        string serverHashUrl = "http://mc.yu.ac.kr/ryoket/serverziphash.txt"; // 서버에 저장된 해시 파일 URL

        using (var client = new WebClient())
        {
            try
            {
                // 서버의 해시값 다운로드
                string serverHash = await client.DownloadStringTaskAsync(serverHashUrl);
                serverHash = serverHash.Trim(); // 줄바꿈이나 공백 제거

                // 기존 해시 값 읽기 (없으면 null)
                string? previousHash = File.Exists(hashFilePath) ? File.ReadAllText(hashFilePath).Trim() : null;

                if (serverHash != previousHash)
                {
                    // 해시 값이 다를 경우 zip 파일 다운로드
                    await client.DownloadFileTaskAsync(ModpackUrl, downloadedZipPath);

                    // 압축 해제
                    ZipFile.ExtractToDirectory(downloadedZipPath, basePath, true);
                    File.Delete(downloadedZipPath);

                    // 새로운 해시 값을 저장
                    File.WriteAllText(hashFilePath, serverHash);

                    MessageBox.Show("모드팩이 업데이트 되었습니다.");
                }
                else
                {
                    MessageBox.Show("모드팩이 최신 상태입니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}");
            }
        }
    }
}
