using RSBot.Core;

namespace RSBot.Repetition;

internal static class AppService
{
    public static ControllerService Controller { get; private set; }

    public static void Initialize()
    {
        Controller = new ControllerService();
        Controller.Initialize();
        Log.Debug("[Repetition] Plugin initialized.");
    }
}
