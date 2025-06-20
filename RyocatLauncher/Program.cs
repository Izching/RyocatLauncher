using Newtonsoft.Json.Linq;
using RyocatLauncher;
using System.Diagnostics;
using System.Net;

namespace RyocatLauncher
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            System.Net.ServicePointManager.DefaultConnectionLimit = 256;
            ApplicationConfiguration.Initialize();
            CheckForUpdateAsync().GetAwaiter().GetResult();
            var form = new AccountForm();
            form.Show();
            Application.Run();
        }

        static async Task CheckForUpdateAsync()
        {
            string currentVersion = "3.0.0";
            string versionUrl = "http://mc.yu.ac.kr/ryoket/version.json";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(versionUrl);
                    JObject obj = JObject.Parse(json);
                    string? serverVersion = obj["version"]?.ToString();
                    string? downloadUrl = obj["downloadUrl"]?.ToString();

                    if (!currentVersion.Equals(serverVersion))
                    {
                        var result = MessageBox.Show(
                            $"Ryocat Launcher {serverVersion} 버전이 업데이트 되었습니다.\n"+
                            "업데이트를 위한 설치 파일을 다운로드 하시겠습니까?\n"+
                            "(구버전 런처 사용시 서버 접속이 불가능 할 수 있습니다.)","",
                            MessageBoxButtons.YesNo
                        );

                        if (result == DialogResult.Yes)
                        {
                            DownloadAndUpdate(downloadUrl);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"업데이트 확인 실패: {ex.Message}");
                }
            }
        }

        static async void DownloadAndUpdate(string url)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "RyocatLauncher_setup.exe");
            var loadingForm = new DownloadForm();

            int progress = 0;

            // 다운로드 시작
            Task downloadTask = Task.Run(async () =>
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += (s, e) =>
                    {
                        progress = e.ProgressPercentage;

                        // UI 스레드에서 SetProgress 호출
                        if (loadingForm.IsHandleCreated)
                        {
                            loadingForm.Invoke(() =>
                            {
                                loadingForm.SetProgress(progress);
                            });
                        }
                    };

                    try
                    {
                        await client.DownloadFileTaskAsync(url, tempPath);
                    }
                    catch (Exception ex)
                    {
                        if (loadingForm.IsHandleCreated)
                        {
                            loadingForm.Invoke(() =>
                            {
                                MessageBox.Show($"업데이트 실패: {ex.Message}");
                                loadingForm.Close();
                            });
                        }
                    }
                }

                // 다운로드 끝나고 종료 및 설치기 실행
                if (loadingForm.IsHandleCreated)
                {
                    loadingForm.Invoke(() =>
                    {
                        loadingForm.SetDownloadCompleted();
                    });
                }

                await Task.Delay(100);

                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = tempPath,
                        UseShellExecute = true
                    });
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"설치기 실행 실패: {ex.Message}");
                }
            });

            loadingForm.ShowDialog(); // 다운로드 동안 대기 (UI 스레드 사용)
            await downloadTask;
            //try
            //{
            //    using (HttpClient client = new HttpClient())
            //    using (var response = client.GetAsync(url).Result)
            //    using (var fs = new FileStream(tempPath, FileMode.Create))
            //    {
            //        response.Content.CopyToAsync(fs).GetAwaiter().GetResult();
            //    }

            //    // 설치기 실행
            //    Process.Start(new ProcessStartInfo
            //    {
            //        FileName = tempPath,
            //        UseShellExecute = true // 관리자 권한 설치기 실행 시 권장
            //    });

            //    Environment.Exit(0); // 현재 런처 종료
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"업데이트 실패: {ex.Message}");
            //}
        }
    }
}