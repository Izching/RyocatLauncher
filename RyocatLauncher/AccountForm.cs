using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Sessions;
using XboxAuthNet.OAuth;

namespace RyocatLauncher;

public partial class AccountForm : MetroFramework.Forms.MetroForm
{
    JELoginHandler _loginHandler;

    public AccountForm()
    {
        _loginHandler = JELoginHandlerBuilder.BuildDefault();
        InitializeComponent();
    }

    private bool exitOnClose = true;
    public bool AutoLogin { get; set; } = true;

    private async void AccountForm_Load(object sender, EventArgs e)
    {
        this.Enabled = false;
        listAccounts();
        if (AutoLogin)
            await tryAutoLogin();
        this.Enabled = true;
    }

    private void listAccounts()
    {
        flAccounts.Controls.Clear();
        var accounts = _loginHandler.AccountManager.GetAccounts();
        foreach (var account in accounts)
        {
            if (account is not JEGameAccount jeGameAccount)
                continue;

            var control = new AccountControl(jeGameAccount);
            control.LoginClicked += Control_LoginClicked;
            control.RemoveClicked += Control_RemoveClicked;
            flAccounts.Controls.Add(control);
        }

        lbNoAccountInfo.Visible = (flAccounts.Controls.Count == 0);
    }

    private async Task tryAutoLogin()
    {
        try
        {
            var result = await _loginHandler.AuthenticateSilently();
            showLauncherForm(result);
        }
        catch (JEAuthException)
        {
            MessageBox.Show("로그인이 만료되었거나 유효하지 않습니다.\n다시 로그인해주세요.");
        }
        catch (MicrosoftOAuthException)
        {

        }
    }

    private async void btnNewAccount_Click(object sender, EventArgs e)
    {
        this.Enabled = false;
        try
        {
            var result = await _loginHandler.AuthenticateInteractively();
            showLauncherForm(result);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }

        this.Enabled = true;
    }

    private async void Control_LoginClicked(object? sender, EventArgs e)
    {
        if (sender is not AccountControl control)
            return;
        this.Enabled = false;
        try
        {
            var selectedAccount = control.Account ?? throw new InvalidOperationException();
            var result = await _loginHandler.Authenticate(selectedAccount);
            showLauncherForm(result);
        }
        catch (JEAuthException)
        {
            MessageBox.Show("로그인 정보가 만료되었거나 유효하지 않습니다.\n계정을 다시 등록해주세요.");

            // 선택된 계정 제거
            var selectedAccount = ((AccountControl)sender!).Account;
            await _loginHandler.Signout(selectedAccount!);

            // UI 새로고침
            listAccounts();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        this.Enabled = true;
    }

    private void Control_RemoveClicked(object? sender, EventArgs e)
    {
        if (sender is not AccountControl control)
            return;

        this.Enabled = false;
        try
        {
            var selectedAccount = control.Account ?? throw new InvalidOperationException();
            _loginHandler.Signout(selectedAccount);
            listAccounts();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        this.Enabled = true;
    }

    private void showLauncherForm(MSession session)
    {
        exitOnClose = false;

        var launcherForm = new LauncherForm(session);
        launcherForm.Show();

        this.BeginInvoke((Action)(() => this.Close()));
    }

    private void AccountForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (exitOnClose)
            Environment.Exit(0);
    }
}