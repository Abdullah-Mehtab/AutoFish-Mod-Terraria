using TerrariaModder.Core;
using TerrariaModder.Core.Logging;
using HarmonyLib;

namespace AutoFisher
{
    // Главный класс мода
    public class AutoFisherMod : IMod
    {
        public void OnWorldLoad()
        {
        }

        public void OnWorldUnload()
        {
        }
        public string Id => "autofisher";
        public string Name => "Auto Fisher";
        public string Version => "1.0.0";

        private ILogger _log;
        private ModContext _context;

        public void Initialize(ModContext context)
        {
            _log = context.Logger;
            _context = context;
            LoadConfigValues();

            context.RegisterKeybind("toggle", "Toggle Auto Fisher", "Enable/Disable autofisher", "F5", OnTogglePressed);

            new Harmony(Id).PatchAll();

            _log.Info($"{Name} v{Version} initialized!");
        }

        private void OnTogglePressed()
        {
            ModState.IsEnabled = !ModState.IsEnabled;
            _log.Info($"Auto Fisher is now: {(ModState.IsEnabled ? "ENABLED" : "DISABLED")}");

            _context.Config.Set("enabled", ModState.IsEnabled);
            _context.Config.Save();
        }

        public void OnConfigChanged()
        {
            _log.Info("Config changed, reloading values...");
            LoadConfigValues();
        }

        private void LoadConfigValues()
        {
            if (_context?.Config == null) return;
            ModState.IsEnabled = _context.Config.Get("enabled", true);
            _log.Debug($"Config loaded: enabled={ModState.IsEnabled}");
        }
        public void Unload()
        {
            _log.Info($"{Name} unloaded!");
        }
    }
}