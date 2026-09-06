using System.Collections.Generic;
using System.Linq;

namespace RSBot.Core.Components.Scripting.Commands;

internal class QuestAcceptScriptCommand : IScriptCommand
{
    #region Properties

    public string Name => "quest-accept";

    public bool IsBusy { get; private set; }

    public Dictionary<string, string> Arguments => new()
    {
        { "NpcCodename", "The code name of the quest NPC" },
        { "QuestCodename", "The code name of the quest" },
    };

    #endregion Properties

    #region Methods

    public bool Execute(string[] arguments = null)
    {
        if (arguments == null || arguments.Length < Arguments.Count)
        {
            Log.Warn("[Script] Invalid quest-accept command: NPC or quest code name missing.");
            return false;
        }

        var npcCodeName = arguments[0];
        var questCodeName = arguments[1];

        var quest = Game.ReferenceManager.QuestData.Values.FirstOrDefault(q => q.CodeName == questCodeName);
        if (quest == null)
        {
            Log.Warn($"[Script] quest-accept: Quest '{questCodeName}' not found in reference data.");
            return false;
        }

        try
        {
            IsBusy = true;
            Log.Notify($"[Script] Accepting quest [{quest.GetTranslatedName()}]...");
            ShoppingManager.AcceptQuest(npcCodeName, quest.ID);
            return true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void Stop()
    {
        IsBusy = false;
    }

    #endregion Methods
}
