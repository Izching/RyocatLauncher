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
                            $"Ryocat Launcher {serverVersion} ������ ������Ʈ �Ǿ����ϴ�.\n"+
                            "������Ʈ�� ���� ��ġ ������ �ٿ�ε� �Ͻðڽ��ϱ�?\n"+
                            "(������ ��ó ���� ���� ������ �Ұ��� �� �� �ֽ��ϴ�.)","",
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
                    MessageBox.Show($"������Ʈ Ȯ�� ����: {ex.Message}");
                }
            }
        }

        static async void DownloadAndUpdate(string url)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "RyocatLauncher_setup.exe");
            var loadingForm = new DownloadForm();

            int progress = 0;

            // �ٿ�ε� ����
            Task downloadTask = Task.Run(async () =>
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += (s, e) =>
                    {
                        progress = e.ProgressPercentage;

                        // UI �����忡�� SetProgress ȣ��
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
                                MessageBox.Show($"������Ʈ ����: {ex.Message}");
                                loadingForm.Close();
                            });
                        }
                    }
                }

                // �ٿ�ε� ������ ���� �� ��ġ�� ����
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
                    MessageBox.Show($"��ġ�� ���� ����: {ex.Message}");
                }
            });

            loadingForm.ShowDialog(); // �ٿ�ε� ���� ��� (UI ������ ���)
            await downloadTask;
            //try
            //{
            //    using (HttpClient client = new HttpClient())
            //    using (var response = client.GetAsync(url).Result)
            //    using (var fs = new FileStream(tempPath, FileMode.Create))
            //    {
            //        response.Content.CopyToAsync(fs).GetAwaiter().GetResult();
            //    }

            //    // ��ġ�� ����
            //    Process.Start(new ProcessStartInfo
            //    {
            //        FileName = tempPath,
            //        UseShellExecute = true // ������ ���� ��ġ�� ���� �� ����
            //    });

            //    Environment.Exit(0); // ���� ��ó ����
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"������Ʈ ����: {ex.Message}");
            //}
        }
    }
}