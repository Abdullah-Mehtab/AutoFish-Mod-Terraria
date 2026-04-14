using TerrariaModder.Core;
using TerrariaModder.Core.Config;
using TerrariaModder.Core.Logging;
using HarmonyLib;

namespace AutoFisher
{
    public class AutoFisherConfig : ModConfig
    {
        public override int Version => 1;

        [Label("Master Switch")]
        [Description("Enables or disables all automated fishing logic globally.")]
        public bool Enabled { get; set; } = true;
    }

    public class AutoFisherMod : IMod, IModLifecycle
    {
        public string Id => "autofisher";
        public string Name => "Auto Fisher";
        public string Version => "1.1.0";

        private ILogger _log;
        private AutoFisherConfig _config;

        public void Initialize(ModContext context)
        {
            _log = context.Logger;
            _config = context.GetConfig<AutoFisherConfig>();

            ModState.IsEnabled = _config.Enabled;

            context.RegisterKeybind("toggle", "Toggle Auto Fisher", "Enable/Disable autofisher", "F5", OnTogglePressed);

            new Harmony(Id).PatchAll();

            _log.Info($"{Name} v{Version} initialized!");
        }

        private void OnTogglePressed()
        {
            ModState.IsEnabled = !ModState.IsEnabled;
            _config.Enabled = ModState.IsEnabled;
            _config.Save();
            _log.Info($"Auto Fisher is now: {(ModState.IsEnabled ? "ENABLED" : "DISABLED")}");
        }

        public void OnConfigChanged()
        {
            _log.Info("Config changed, reloading values...");
            if (_config != null)
                ModState.IsEnabled = _config.Enabled;
        }

        public void OnContentReady(ModContext context) { }
        public void OnWorldLoad() { }
        public void OnWorldUnload() { }

        public void Unload()
        {
            _log.Info($"{Name} unloaded!");
        }
    }
}
