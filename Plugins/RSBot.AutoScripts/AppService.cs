using RSBot.AutoScripts.Views;

namespace RSBot.AutoScripts;

internal static class AppService
{
    private static Main _mainView;

    public static AutoScriptsService Bot { get; set; }

    public static Main View
    {
        get
        {
            if (_mainView == null || _mainView.Disposing || _mainView.IsDisposed)
                _mainView = new Main();
            return _mainView;
        }
    }
}
