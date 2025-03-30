using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Version;
using CmlLib.Utils;
using CmlLib.Core.Installer.FabricMC;
using System.Net;
using System.IO.Compression;
using System.Security.Cryptography;
using CmlLib.Core.Downloader;

namespace RyocatLauncher;

public partial class LauncherForm : MetroFramework.Forms.MetroForm
{
    private readonly MSession _session;
    private readonly CMLauncher _launcher;

    private LogForm? logForm;

    private bool exitOnClose = true;
    private const string ModpackUrl = "http://mc.yu.ac.kr/ryoket/ryoket.zip";
    private int selectedMemoryMb = 4096;

    public LauncherForm(MSession session)
    {
        _session = session;

        // 사용자 AppData/Roaming 경로 가져오기
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        // MyLauncher 폴더 경로 설정
        string myLauncherPath = Path.Combine(appDataPath, "RyocatLauncher");


        // 폴더가 없으면 생성하기
        if (!Directory.Exists(myLauncherPath))
        {
            Directory.CreateDirectory(myLauncherPath);
        }

        // MinecraftPath 객체를 MyLauncher 폴더로 설정
        var customPath = new MinecraftPath(myLauncherPath);

        _launcher = new CMLauncher(customPath);
        _launcher.FileChanged += launcher_FileChanged;
        _launcher.ProgressChanged += launcher_ProgressChanged;

        InitializeComponent();
    }

    //마인크래프트 Progress 진행도
    private void launcher_ProgressChanged(object? sender, System.ComponentModel.ProgressChangedEventArgs e)
    {
        SetProgress(e.ProgressPercentage, $"[Minecraft] 다운로드 중... {e.ProgressPercentage}%");
    }

    //마인크래프트 Files 진행도
    private void launcher_FileChanged(CmlLib.Core.Downloader.DownloadFileChangedEventArgs e)
    {
        pbFiles.Maximum = e.TotalFileCount;
        pbFiles.Value = e.ProgressedFileCount;
    }

    private void SetProgress(int percent, string message)
    {
        pbProgress.Value = percent;
        lbProgress.Text = message;
    }
    private void LauncherForm_Load(object sender, EventArgs e)
    {
        showAccountControl();
        //await listVersions();
        //await listFabricVersions();
        pbProgress.Maximum = 100; //Progress 최대 진행도 100으로 설정
    }

    // 마인크래프트 버전 리스트업 매서드
    //private async Task listVersions()
    //{
    //    var version = await _launcher.GetVersionAsync("1.21.1");

    //    cbVersion.Items.Clear();

    //    cbVersion.Text = "1.21.1";
    //}


    // fabric 버전 리스트업 매서드
    //private async Task listFabricVersions()
    //{
    //    var fabricVersionLoader = new FabricVersionLoader();
    //    var fabricVersions = await fabricVersionLoader.GetVersionMetadatasAsync();
    //    var targetVersion = fabricVersions.FirstOrDefault(v => v.Name == "fabric-loader-0.16.10-1.21.1");

    //    cbFabricVersions.Items.Clear();
    //    cbFabricVersions.Text = "fabric-loader-0.16.10-1.21.1";

    //    ////fabric 버전 확인용
    //    //var fabricVersionLoader = new FabricVersionLoader();
    //    //var fabricVersions = await fabricVersionLoader.GetVersionMetadatasAsync();

    //    //cbFabricVersions.Items.Clear();
    //    //foreach (var v in fabricVersions)
    //    //{
    //    //    cbFabricVersions.Items.Add(v.Name);
    //    //}

    //    //if (fabricVersions.Count() > 0)
    //    //{
    //    //    cbFabricVersions.SelectedIndex = 0;
    //    //}
    //}

    private void showAccountControl()
    {
        var control = new AccountControl(_session);
        pAccountHolder.Controls.Clear();
        pAccountHolder.Controls.Add(control);
    }

    private async void btnStart_Click(object sender, EventArgs e)
    {
        this.Enabled = false;
        const string FabricVersionName = "fabric-loader-0.16.10-1.21.1"; // Fabric 버전명
        var path = _launcher.MinecraftPath;

        try
        {

            string basePath = _launcher.MinecraftPath.BasePath;
            var fabricVersionPath = Path.Combine(path.Versions, FabricVersionName);

            var fabricVersionLoader = new FabricVersionLoader();
            var fabricVersions = await fabricVersionLoader.GetVersionMetadatasAsync();
            var fabric = fabricVersions.GetVersionMetadata(FabricVersionName);

            SetProgress(0, "[Fabric] 다운로드 시작...");
            await fabric.SaveAsync(path);
            SetProgress(100, "[Fabric] 다운로드 완료...");

            await DownloadAndUpdateFiles(basePath);


            var process = await _launcher.CreateProcessAsync(FabricVersionName, new MLaunchOption
            {
                Session = _session,
                ServerIp = "mc.yu.ac.kr", //서버 주소
                ServerPort = 25568, //포트
                MaximumRamMb = selectedMemoryMb // 메모리 설정
            });

            SetProgress(100, "게임 실행 중...");
            logForm = new LogForm();
            logForm.Hide(); //초기설정으로 로그 숨김

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

            client.DownloadProgressChanged += (s, e) =>
            {
                SetProgress(e.ProgressPercentage, $"[Modpack] 다운로드 중... {e.ProgressPercentage}%");
                pbFiles.Maximum = 100;
                pbFiles.Value = e.ProgressPercentage;
            };

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
                    SetProgress(0, $"[Modpack] 압축 해제 중...");
                    ZipFile.ExtractToDirectory(downloadedZipPath, basePath, true);
                    File.Delete(downloadedZipPath);
                    SetProgress(100, $"[Modpack] 압축 해제 완료...");

                    // 새로운 해시 값을 저장
                    File.WriteAllText(hashFilePath, serverHash);

                    MessageBox.Show("모드팩이 업데이트 되었습니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}");
            }
        }
    }

    private void btnOpenLog_Click(object sender, EventArgs e)
    {
        if (logForm == null || logForm.IsDisposed)
        {
            MessageBox.Show("로그가 시작되지 않았습니다.");
            return;
        }
        logForm.Show();
    }

}
