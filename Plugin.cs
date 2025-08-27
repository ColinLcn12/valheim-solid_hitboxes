using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using ServerSync;

namespace SolidHitboxes
{
    [BepInPlugin(pluginId, pluginName, pluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string pluginId = "org.bepinex.plugins.solid-hitboxes";
        private const string pluginName = "Solid Hitboxes";
        private const string pluginVersion = "1.0.6";

        private readonly Harmony _harmony = new Harmony(pluginId);

        ConfigSync configSync = new ConfigSync(pluginId) { DisplayName = pluginName, CurrentVersion = pluginVersion, MinimumRequiredVersion = pluginVersion };
        private static ConfigEntry<bool> LockConfig;
        private static ConfigEntry<bool> EnableFriendlyFire;

        public static bool FFEnabled => EnableFriendlyFire.Value;

        private void Awake()
        {
            LockConfig = config("General", "LockConfig", true, "If on, the configuration is locked and can be changed by server admins only. [Synced with server]");
            EnableFriendlyFire = config("General", "EnableFriendlyFire", false, "Whether or not AI can damage their friends");

            _harmony.PatchAll();
        }

        #region ServerSync

        ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);

        #endregion
    }
}
