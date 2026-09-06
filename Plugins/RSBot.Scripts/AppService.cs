using RSBot.Scripts.Views;

namespace RSBot.Scripts;

internal static class AppService
{
    private static Main _mainView;

    public static AutoScriptsService Bot { get; set; }
    public static AutoScriptsService Script1 { get; set; }
    public static AutoScriptsService Script2 { get; set; }
    public static AutoScriptsService Script3 { get; set; }

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
