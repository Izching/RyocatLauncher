using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Utils;
using System.Net;
using System.IO.Compression;
using System.Diagnostics;

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

        selectedMemoryMb = LoadSelectedMemory();

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
        lbProgress.Refresh();
    }
    private void LauncherForm_Load(object sender, EventArgs e)
    {
        showAccountControl();

        pbProgress.Maximum = 100; //Progress 최대 진행도 100으로 설정
        //await listVersions();
        //await listFabricVersions();
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
        string versionPath = Path.Combine(_launcher.MinecraftPath.Versions, FabricVersionName);
        var path = _launcher.MinecraftPath;
        string basePath = _launcher.MinecraftPath.BasePath;
        string FabricZipPath = Path.Combine(versionPath, "fabric.zip");

        try
        {
            //버전 폴더가 없으면 수동 다운로드
            if (!Directory.Exists(versionPath) ||
                !File.Exists(Path.Combine(versionPath, $"{FabricVersionName}.json")) ||
                !File.Exists(Path.Combine(versionPath, $"{FabricVersionName}.jar")))
            {
                Directory.CreateDirectory(versionPath);

                //fabric zip 파일 직접 다운로드
                using (var client = new WebClient())
                {
                    try
                    {
                        SetProgress(0, "[Fabric] 다운로드 중...");
                        string zipUrl = $"https://meta.fabricmc.net/v2/versions/loader/1.21.1/0.16.10/profile/zip";
                        await client.DownloadFileTaskAsync(zipUrl, FabricZipPath);
                        ZipFile.ExtractToDirectory(FabricZipPath, _launcher.MinecraftPath.Versions, true);
                        File.Delete(FabricZipPath);
                        SetProgress(100, "[Fabric] 다운로드 완료");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"패브릭 설치 실패: {ex.Message}");
                    }
                }
            }
            else
            {
                SetProgress(100, "[Fabric] 이미 설치됨");
            }

            await DownloadAndUpdateFiles(basePath);

            SetProgress(100, "[Minecraft] 실행 대기 중...");

            var process = await _launcher.CreateProcessAsync(FabricVersionName, new MLaunchOption
            {
                Session = _session,
                ServerIp = "mc.yu.ac.kr", //서버 주소
                ServerPort = 25568, //포트
                MaximumRamMb = selectedMemoryMb // 메모리 설정
            });

            SetProgress(100, "게임 실행 중... 잠시만 기다려 주세요");

            logForm = new LogForm();
            logForm.Hide(); //초기설정으로 로그 숨김

            var processUtil = new ProcessUtil(process);
            processUtil.OutputReceived += (s, e) => logForm.AppendLog(e);
            processUtil.StartWithEvents();

        }
        catch (Exception ex)
        {
            MessageBox.Show($"게임 실행 실패: {ex.Message}");
        }
        await ShowLoadingMessage(); //1분 대기
        this.Enabled = true;
    }

    //1분 대기하면서 Progress 메세지를 계속 갱신
    private async Task ShowLoadingMessage()
    {
        int dotCount = 0;
        for (int i = 0; i < 60; i++)
        {
            dotCount = (i % 5) + 1;
            SetProgress(100, $"게임 실행 중... 잠시만 기다려 주세요{new string('.', dotCount)}");
            await Task.Delay(1000);
        }
    }

    private void btnSetting_Click(object sender, EventArgs e)
    {
        var form = new SettingForm(selectedMemoryMb);
        form.FormClosingRequired += (s, e) =>
        {
            exitOnClose = false;
            this.Close();
        };
        form.ShowDialog();
        selectedMemoryMb = form.SelectedMemory;
        SaveSelectedMemory();
    }

    private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (exitOnClose)
            Environment.Exit(0);
    }

    //모드팩 다운, 업데이트
    private async Task DownloadAndUpdateFiles(string basePath)
    {
        string downloadedZipPath = Path.Combine(basePath, "temp.zip");
        string hashFilePath = Path.Combine(basePath, "ziphash.txt");

        string serverHashUrl = "http://mc.yu.ac.kr/ryoket/serverziphash.txt"; // 서버에 저장된 해시 파일 URL

        using (var client = new WebClient())
        {

            try
            {
                ResetLbProgress();
                SetProgress(0, $"서버 버전 확인 중...");
                // 서버의 해시값 다운로드
                string serverHash = await client.DownloadStringTaskAsync(serverHashUrl);
                serverHash = serverHash.Trim(); // 줄바꿈이나 공백 제거

                // 기존 해시 값 읽기 (없으면 null)
                string? previousHash = File.Exists(hashFilePath) ? File.ReadAllText(hashFilePath).Trim() : null;

                if (previousHash == null)
                {
                    client.DownloadProgressChanged += (s, e) =>
                    {
                        SetProgress(e.ProgressPercentage, $"[Modpack] 다운로드 중... {e.ProgressPercentage}%");
                        pbFiles.Maximum = 100;
                        pbFiles.Value = e.ProgressPercentage;
                    };
                    // 해시 값이 다를 경우 zip 파일 다운로드
                    await client.DownloadFileTaskAsync(ModpackUrl, downloadedZipPath);

                    // 압축 해제
                    SetProgress(0, $"[Modpack] 압축 해제 중...");
                    BackupAndExtractFile(downloadedZipPath, basePath);
                    File.Delete(downloadedZipPath);
                    SetProgress(100, $"[Modpack] 압축 해제 완료");

                    // 새로운 해시 값을 저장
                    File.WriteAllText(hashFilePath, serverHash);

                    MessageBox.Show("모드팩 업데이트가 완료되었습니다.");
                    return;
                }

                if (serverHash != previousHash)
                {
                    this.Enabled = false;

                    DialogResult deleteConfirm = MessageBox.Show("" +
                        "서버가 업데이트되었습니다.\n" +
                        "안정적인 이용을 위해 런처 파일을 초기화하고 최신 버전으로\n" +
                        "업데이트합니다.\n" +
                        "(서버 플레이 기록에는 영향을 주지 않습니다)\n" +
                        "반드시 게임을 완전히 종료하고 진행해 주세요.\n" +
                        "지금 초기화를 진행하시겠습니까? \n", "", MessageBoxButtons.YesNo);

                    if (deleteConfirm == DialogResult.Yes)
                    {
                        try
                        {
                            DeleteAllUpdate();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"초기화 실패: {ex.Message}");
                        }
                    } 
                    else
                    {
                        Application.Exit();
                    }

                    this.Enabled = true;
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show($"모드팩 설치 실패: {ex.Message}");
            }
        }
    }

    //옵션(단축키,언어)파일 백업 후 압축해제
    private void BackupAndExtractFile(string downloadedZipPath, string basePath)
    {
        // 백업 대상 설정
        string fileToBackup = "options.txt";

        string backupPath = Path.Combine(basePath, "backup");
        Directory.CreateDirectory(backupPath);

        string filePath = Path.Combine(basePath, fileToBackup);
        string destPath = Path.Combine(backupPath, fileToBackup);

        if(File.Exists(filePath))
        {
            File.Copy(filePath, destPath, true);
        }

        ZipFile.ExtractToDirectory(downloadedZipPath, basePath, true);

        if(File.Exists(destPath))
        {
            File.Copy(destPath, filePath, true);
        }

        Directory.Delete(backupPath, true);

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

    private void btnDeleteAll_Click(object sender, EventArgs e)
    {
        this.Enabled = false;

        DialogResult deleteConfirm = MessageBox.Show("Ryocat Launcher로 실행한 마인크래프트 파일과 메모리 설정 정보가\n" +
            "초기화됩니다. \n" +
            "게임에 심각한 문제가 발생했을 때 또는 서버가 열려있으나 지속적으로\n" +
            "접속에 실패하는 경우에만 사용을 권장합니다.\n" +
            "반드시 마인크래프트를 완전히 종료하고 초기화를 진행해 주세요.\n" +
            "런처 초기화를 진행하시겠습니까? \n", "", MessageBoxButtons.YesNo);


        if (deleteConfirm == DialogResult.Yes)
        {
            try
            {
                DeleteAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"초기화 실패: {ex.Message}");
            }

        }

        this.Enabled = true;
    }

    private void DeleteAll()
    {
        string launcherPath = _launcher.MinecraftPath.BasePath;

        if (!Directory.Exists(launcherPath))
        {
            MessageBox.Show("파일이 존재하지 않습니다.");
            return;
        }

        string[] files = Directory.GetFiles(launcherPath, "*", SearchOption.AllDirectories);
        string[] directories = Directory.GetDirectories(launcherPath, "*", SearchOption.AllDirectories);
        int total = files.Length + directories.Length;
        int deleted = 0;
        int failed = 0;

        pbFiles.Maximum = total;
        pbFiles.Value = 0;
        ResetLbProgress();
        SetProgress(0, $"초기화 중...");


        for (int i = 0; i < files.Length; i++)
        {
            string file = files[i];

            try
            {
                File.Delete(file);
                deleted++;
            }
            catch
            {
                failed++;
            }
            //진행도 계산 및 출력
            pbFiles.Value = deleted;
            int percent = (int)((deleted / (float)total) * 100);
            SetProgress(percent, $"초기화 중... {percent}%");
        }

        //폴더들 삭제
        for (int i = directories.Length - 1; i >= 0; i--)
        {
            string dir = directories[i];

            try
            {
                Directory.Delete(dir, true);
                deleted++;
            }
            catch
            {
                failed++;
            }

            //진행도 계산 및 출력
            pbFiles.Value = deleted;
            int percent = (int)((deleted / (float)total) * 100);
            SetProgress(percent, $"초기화 중... {percent}%");
        }

        if (failed > 0)
        {
            SetProgress(100, $"초기화 실패");
            MessageBox.Show($"일부 파일 삭제 실패\n총 실패 파일 수: {failed}\n" +
                "초기화를 재시도하거나 수동으로 폴더를 삭제해 주세요\n" +
                $"경로명: {launcherPath}");
        }
        else
        {
            SetProgress(100, $"초기화 성공");
            MessageBox.Show("초기화가 완료되었습니다. 런처를 재시작합니다.");
            Process.Start(Application.ExecutablePath);
            Application.Exit();
        }
    }

    private void DeleteAllUpdate()
    {
        string launcherPath = _launcher.MinecraftPath.BasePath;

        if (!Directory.Exists(launcherPath))
        {
            MessageBox.Show("파일이 존재하지 않습니다.");
            return;
        }

        string[] files = Directory.GetFiles(launcherPath, "*", SearchOption.AllDirectories);
        string[] directories = Directory.GetDirectories(launcherPath, "*", SearchOption.AllDirectories);
        int total = files.Length + directories.Length;
        int deleted = 0;
        int failed = 0;

        pbFiles.Maximum = total;
        pbFiles.Value = 0;
        ResetLbProgress();
        SetProgress(0, $"초기화 중...");


        HashSet<string> excludedFiles = new HashSet<string>
        {
            Path.Combine(launcherPath, "options.txt"),
            Path.Combine(launcherPath, "memory.txt")
        };

        string xaeroFolderPath = Path.Combine(launcherPath, "xaero");
        string localFolderPath = Path.Combine(launcherPath, "local");

        for (int i = 0; i < files.Length; i++)
        {
            string file = files[i];

            if (excludedFiles.Contains(file) || file.StartsWith(xaeroFolderPath + Path.DirectorySeparatorChar) || file.StartsWith(localFolderPath + Path.DirectorySeparatorChar))
            {
                continue;
            }
            try
            {
                File.Delete(file);
                deleted++;
            }
            catch
            {
                failed++;
            }
            //진행도 계산 및 출력
            pbFiles.Value = deleted;
            int percent = (int)((deleted / (float)total) * 100);
            SetProgress(percent, $"초기화 중... {percent}%");
        }

        //폴더들 삭제
        for (int i = directories.Length - 1; i >= 0; i--)
        {
            string dir = directories[i];

            if (dir.StartsWith(xaeroFolderPath + Path.DirectorySeparatorChar) || dir.StartsWith(xaeroFolderPath) || dir.StartsWith(localFolderPath + Path.DirectorySeparatorChar) || dir.StartsWith(localFolderPath))
            {
                continue;
            }
            try
            {
                Directory.Delete(dir, true);
                deleted++;
            }
            catch
            {
                failed++;
            }

            //진행도 계산 및 출력
            pbFiles.Value = deleted;
            int percent = (int)((deleted / (float)total) * 100);
            SetProgress(percent, $"초기화 중... {percent}%");
        }

        if (failed > 0)
        {
            SetProgress(100, $"초기화 실패");
            MessageBox.Show($"일부 파일 삭제 실패\n총 실패 파일 수: {failed}\n" +
                "초기화를 재시도하거나 수동으로 폴더를 삭제해 주세요\n" +
                $"경로명: {launcherPath}");
            Application.Exit();
        }
        else
        {
            SetProgress(100, $"초기화 성공");
            MessageBox.Show("초기화가 완료되었습니다. 런처를 재시작합니다.");
            Process.Start(Application.ExecutablePath);
            Application.Exit();
        }
    }

    //메모리 설정값 저장
    private void SaveSelectedMemory()
    {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "RyocatLauncher", "memory.txt");
        File.WriteAllText(path, selectedMemoryMb.ToString());
    }

    //메모리 불러오기
    private int LoadSelectedMemory()
    {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "RyocatLauncher", "memory.txt");
        if (File.Exists(path))
        {
            string content = File.ReadAllText(path);
            if (int.TryParse(content, out int memory))
            {
                return memory;
            }
        }
        return 4096;
    }

    private void ResetLbProgress()
    {
        using (Graphics g = lbProgress.CreateGraphics())
        {
            g.Clear(lbProgress.BackColor);
        }
    }
}
