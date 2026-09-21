# RSBot — Claude Code Guide

RSBot is an automation bot for **Silkroad Online**, written in C# (.NET, WinForms). It has a modular architecture: one main application, multiple botbases, and multiple plugins, all wired together by a shared core library.

## Build

Use `.\build.ps1` (PowerShell). Never use `dotnet build`.

```powershell
# Debug (default) — builds and launches RSBot.exe
.\build.ps1

# Clean debug build, don't start the app
.\build.ps1 -Clean -DoNotStart

# Release build
.\build.ps1 -Configuration Release
```

If execution policy blocks the script: `powershell.exe -ExecutionPolicy Bypass .\build.ps1`

Build output goes to `.\Build\`. Errors are in `.\build.log` (build.ps1 already tails the last 100 lines). If a build fails, check `.\build.log` — do not ask the user to fix it unless you cannot resolve the error yourself.

MSBuild path (resolved automatically by `build.ps1` via `vswhere.exe`):
`C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe`

Flags: `/p:Configuration=Debug /p:Platform=x86` (the project is **x86 only**).

### Build output structure

| Path | Contents |
|---|---|
| `.\Build\RSBot.exe` | Main executable |
| `.\Build\Data\` | Botbases, plugins, scripts, dependencies |
| `.\Build\User\` | Per-user configs, autologin, profiles |
| `.\Build\User\Logs\` | Per-character log files (ISO-dated) |

## File editing rules

- **Never edit** `.sln`, `.csproj`, `.Designer.cs`, or other auto-generated files without explicit user confirmation.
- Use `dotnet restore` / `dotnet add` for NuGet package management only — not for building.
- The `.\SDUI` directory is a git submodule (UI library). `build.ps1` will auto-initialize it if missing.

## Project structure

```
RSBot/
├── Application/RSBot/          # Main WinForms application (entry point)
├── Botbases/
│   ├── RSBot.Alchemy/          # In-game alchemy automation
│   ├── RSBot.Lure/             # Monster luring
│   ├── RSBot.Trade/            # Trading automation
│   └── RSBot.Training/         # Default botbase — training area & combat
├── Library/
│   ├── RSBot.Core/             # Core library (shared by everything)
│   ├── RSBot.FileSystem/       # File system helpers
│   ├── RSBot.Loader.Library/   # C++ x86 loader
│   └── RSBot.NavMeshApi/       # Navigation mesh
├── Plugins/
│   ├── RSBot.Chat/
│   ├── RSBot.CommandCenter/
│   ├── RSBot.General/
│   ├── RSBot.Inventory/
│   ├── RSBot.Items/
│   ├── RSBot.Log/
│   ├── RSBot.Map/
│   ├── RSBot.Party/
│   ├── RSBot.Protection/
│   ├── RSBot.Quest/
│   ├── RSBot.Scripts/          # Script runner plugin (town loops etc.)
│   ├── RSBot.ServerInfo/
│   ├── RSBot.Skills/
│   └── RSBot.Statistics/
├── SDUI/                       # UI component library (git submodule)
├── Dependencies/
│   ├── Languages/              # Translation files (.rsl)
│   └── Scripts/                # Town-loop scripts
└── docs/                       # GitHub Pages documentation
```

## Core architecture

### Extension system (`Library/RSBot.Core/Plugins/`)

Everything plugs in via two interfaces:

- **`IBotbase`** — implements `Tick()`, `Start()`, `Stop()`, exposes an `Area`. The active botbase drives the main bot loop.
- **`IPlugin`** — implements `OnLoadCharacter()`, declares `DisplayAsTab`, `Index`, `RequireIngame`. Plugins show up as tabs in the UI.

Both extend `IExtension` (shared metadata). Loaded at runtime by `ExtensionManager` / `PluginRepository`.

### Bot loop (`Library/RSBot.Core/Bot.cs`)

`Bot` holds the active `IBotbase` and runs it on a background `Task`. Key members:

- `Bot.Running` (volatile bool)
- `Bot.TokenSource` (cancellation)
- `Bot.SetBotbase(IBotbase)` — fires `OnSetBotbase` event
- `Bot.Start()` / `Bot.Stop()`

### Event system (`Library/RSBot.Core/Event/EventManager.cs`)

Named string events, fire-and-forget. Common pattern throughout the codebase:

```csharp
EventManager.SubscribeEvent("OnSetBotbase", new Action<IBotbase>(OnSetBotbase));
EventManager.FireEvent("OnSetBotbase", botBase);
```

### Network layer (`Library/RSBot.Core/Network/`)

Packet handlers live under `Network/Handler/Agent/` organized by domain:

`Action`, `Alchemy`, `Character`, `CharacterSelection`, `Cos`, `Entity`, `Exchange`, `Game`, `Inventory`, `Job`, `Logout`, `Party`, `Quest`, `Skill`, `StorageBox`, `Teleport`

### Components (`Library/RSBot.Core/Components/`)

| Component | Purpose |
|---|---|
| `AlchemyManager` | Alchemy state |
| `ClientManager` / `ClientlessManager` | Client vs clientless mode |
| `CommandManager` | In-game command dispatching |
| `LanguageManager` | i18n |
| `PickupManager` | Auto-pickup logic |
| `PlayerFollowService` | Follow behavior |
| `ProfileManager` | User profiles |
| `ScriptManager` | Script execution engine |
| `ShoppingManager` | NPC shopping |
| `SkillManager` | Skill usage |
| `SpawnManager` | Spawn tracking |

### Scripting (`Library/RSBot.Core/Components/Scripting/`)

Script commands implement `IScriptCommand`. Built-in commands:

`Attack`, `Buy`, `Dismount`, `LuckDraw`, `Move`, `QuestAccept`, `QuestComplete`, `Repair`, `SkillCast`, `Store`, `Supplies`, `Teleport`, `Wait`

### Key domain objects (`Library/RSBot.Core/Objects/`)

`Player`, `Position`, `Area`, `Movement`, `Skills`, `CharacterInventory`, `State`, `Action`, `Teleportation`, `NpcTalk`, plus enums for every game state (`BattleState`, `LifeState`, `MotionState`, `JobType`, `MonsterRarity`, etc.).

## Common patterns

- UI is **WinForms**, all views are in `Views/` subdirectories.
- Config keys are stored as string constants and read via `GlobalConfig` / profile-specific config.
- Always check `Kernel.Bot.Running` / game state guards before triggering actions in tick loops.
- Packet responses follow the naming convention `<Domain><Action>Response.cs` (e.g., `ActionCommandStateResponse.cs`).
