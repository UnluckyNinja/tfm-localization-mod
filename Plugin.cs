using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace LocalizationMod
{
  [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
  [BepInProcess("tfm.exe")]
  public class Plugin : BaseUnityPlugin
  {
    internal static ManualLogSource Log;

    internal static ConfigEntry<bool> enableExtracting;
    internal static ConfigEntry<string> languagesToExtract;

    private void Awake()
    {
      Plugin.Log = base.Logger;

      var pluginFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

      // setup config
      var config = new ConfigFile(Path.Combine(pluginFolder, "config.cfg"), true);

      enableExtracting = config.Bind("General", "enableExtracting", false, "will extract strings and translations to ./extracted folder on plugin loaded");

      string defaultLangs = "en,tr,zh,fr,de,es,kr,pl,br,ru";
      languagesToExtract = config.Bind("General", "languagesToExtract", defaultLangs, "Specify languages you want to extract, separated by comma without space");

      // Do patches
      Logger.LogInfo($"Patching Code ...");
      Harmony.CreateAndPatchAll(typeof(PatchBuildingFix));
      if (enableExtracting.Value){
        Harmony.CreateAndPatchAll(typeof(PatchExtracting));
      }
      Harmony.CreateAndPatchAll(typeof(PatchImport));
      Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    public static bool fileExistsInPluginFolder(
      string path,
      string filenameWithExtension
    ){

      var pluginFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      var targetFile = Path.Combine(pluginFolder, path, filenameWithExtension);

      return File.Exists(targetFile);
    }

    public static string readFileInPluginFolder(
      string path,
      string filenameWithExtension
    ){

      var pluginFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      var targetFile = Path.Combine(pluginFolder, path, filenameWithExtension);

      Plugin.Log.LogDebug($"Loading {targetFile}");

      string text = File.ReadAllText(targetFile);
      return text;
    }
    
    public static void writeFileInPluginFolder(
      string path,
      string filenameWithExtension,
      string content
    ){

      var pluginFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      var outputFile = Path.Combine(pluginFolder, path, filenameWithExtension);

      Plugin.Log.LogDebug($"Writing {outputFile}/{filenameWithExtension}");

      // with this no need to create file if file doesn't exist
      File.WriteAllText(outputFile, content);
    }
  }
}
