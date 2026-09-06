using RSBot.Core;

namespace RSBot.Controller;

internal static class AppService
{
    public static ControllerService Controller { get; private set; }

    public static void Initialize()
    {
        Controller = new ControllerService();
        Controller.Initialize();
        Log.Debug("[Controller] Plugin initialized.");
    }
}
