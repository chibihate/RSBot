using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using CommandLine;
using CommandLine.Text;
using RSBot.Core;
using RSBot.Core.Components;
using RSBot.Views;

namespace RSBot;

internal static class Program
{
    public static string AssemblyTitle = Assembly
        .GetExecutingAssembly()
        .GetCustomAttribute<AssemblyProductAttribute>()
        ?.Product;

    public static string AssemblyVersion =
        $"v{Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version}";

    public static string AssemblyDescription = Assembly
        .GetExecutingAssembly()
        .GetCustomAttribute<AssemblyDescriptionAttribute>()
        ?.Description;

    private static readonly string CrashLogPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "crash.log");

    private static void WriteCrashLog(Exception ex)
    {
        try
        {
            var entry = new StringBuilder();
            entry.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {AssemblyTitle} {AssemblyVersion}");
            entry.AppendLine($"Type   : {ex?.GetType().FullName}");
            entry.AppendLine($"Message: {ex?.Message}");
            entry.AppendLine($"Source : {ex?.Source}");
            entry.AppendLine("Stack  :");
            entry.AppendLine(ex?.StackTrace);
            if (ex?.InnerException != null)
            {
                entry.AppendLine("Inner  :");
                entry.AppendLine($"  {ex.InnerException.GetType().FullName}: {ex.InnerException.Message}");
                entry.AppendLine(ex.InnerException.StackTrace);
            }
            entry.AppendLine(new string('-', 80));

            File.AppendAllText(CrashLogPath, entry.ToString(), Encoding.UTF8);
        }
        catch { }
    }

    public class CommandLineOptions
    {
        [Option('c', "character", Required = false, HelpText = "Set the character name to use.")]
        public string Character { get; set; }

        [Option('p', "profile", Required = false, HelpText = "Set the profile name to use.")]
        public string Profile { get; set; }

        [Option("launch-client", Required = false, HelpText = "Start with client")]
        public bool LaunchClient { get; set; }

        [Option("launch-clientless", Required = false, HelpText = "Start clientless")]
        public bool LaunchClientless { get; set; }
    }

    private static void DisplayHelp(ParserResult<CommandLineOptions> result)
    {
        var helpText = HelpText.AutoBuild(
            result,
            h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.AddDashesToOption = true;
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }
        );
        MessageBox.Show(
            helpText,
            AssemblyTitle + " " + AssemblyVersion,
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );
    }

    internal static void WriteLog(string message)
    {
        try
        {
            File.AppendAllText(CrashLogPath,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}",
                Encoding.UTF8);
        }
        catch { }
    }

    [STAThread]
    private static void Main(string[] args)
    {
        WriteLog($"--- Starting {AssemblyTitle} {AssemblyVersion} ---");

        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            WriteCrashLog(e.ExceptionObject as Exception);
        };

        Application.ThreadException += (_, e) =>
        {
            WriteCrashLog(e.Exception);
        };

        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

        try
        {
            var parser = new Parser(with => with.HelpWriter = Console.Out);
            var parserResult = parser.ParseArguments<CommandLineOptions>(args);

            parserResult
                .WithParsed(options =>
                {
                    RunOptions(options);
                })
                .WithNotParsed(errs =>
                {
                    DisplayHelp(parserResult);
                    Environment.Exit(1);
                });

            //CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            // We need "." instead of "," while saving float numbers
            // Also client data is "." based float digit numbers
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

            using Main mainForm = new Main();
            using SplashScreen splashScreen = new(mainForm);

            splashScreen.ShowDialog();

            splashScreen.Dispose();
            Application.Run(mainForm);
        }
        catch (Exception ex)
        {
            WriteCrashLog(ex);
            MessageBox.Show(
                $"An unexpected error occurred. Details saved to:\n{CrashLogPath}\n\n{ex.Message}",
                $"{AssemblyTitle} - Fatal Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    private static void RunOptions(CommandLineOptions options)
    {
        if (options.LaunchClient)
        {
            Kernel.LaunchMode = "client";
            Log.Debug("Launching with client dictated by launch paramaters");
        }
        else if (options.LaunchClientless)
        {
            Kernel.LaunchMode = "clientless";
            Log.Debug("Launching client as clientless dictated by launch paramaters");
        }

        if (!string.IsNullOrEmpty(options.Profile))
        {
            var profile = options.Profile;
            if (ProfileManager.ProfileExists(profile))
                ProfileManager.SetSelectedProfile(profile);
            else
                ProfileManager.Add(profile);

            ProfileManager.IsProfileLoadedByArgs = true;
            Log.Debug($"Selected profile by args: {profile}");
        }

        if (!string.IsNullOrEmpty(options.Character))
        {
            var character = options.Character;
            ProfileManager.SelectedCharacter = character;
            Log.Debug($"Selected character by args: {character}");
        }
    }
}
