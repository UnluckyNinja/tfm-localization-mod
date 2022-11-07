using System.IO;
using HarmonyLib;
using Tfm.Game.Services;
using Tfm.Main.Localization.Constants;
using Tfm.Main.Localization.Utilities;

namespace LocalizationMod
{
  /**
   * Extracts localization files for translator
   */
  public class PatchExtracting
  {

    [HarmonyPatch(typeof(LocalizationDataUtilities), "LoadAllUI")]
    [HarmonyPostfix]
    static void patchExtractingUI(GameModResourceService ____gameModResourceService)
    {
      // check plugin config
      if (!Plugin.enableExtracting.Value)
      {
        Plugin.Log.LogDebug($"Not enabling extracting, skipped.");
        return;
      }

      foreach (string language in Plugin.languagesToExtract.Value.Split(','))
      {
        DoExtract(0, "UI", ".json", language, ____gameModResourceService);
      }

    }

    [HarmonyPatch(typeof(LocalizationDataUtilities), "LoadContentTranslations")]
    [HarmonyPostfix]
    static void patchExtractingContent(
      LocalizationHandlers handler,
      int modId,
      GameModResourceService ____gameModResourceService,
      LocalizationImporterHandler[] ____contentHandlers
    )
    {
      // check plugin config
      if (!Plugin.enableExtracting.Value)
      {
        Plugin.Log.LogDebug($"Not enabling extracting, skipped.");
        return;
      }

      LocalizationImporterHandler importer = ____contentHandlers[(int)handler];

      foreach (string language in Plugin.languagesToExtract.Value.Split(','))
      {
        DoExtract(modId, importer.FILE_NAME, importer.FILE_EXTENSION, language, ____gameModResourceService);
      }

    }

    public static void DoExtract(int modId, string filename, string extension, string language, GameModResourceService _gameModResourceService)
    {
      // prepare filename
      string sourcefile = filename;
      if (language != "en")
      {
        sourcefile = filename + "_" + language;
      }

      // load translation into string
      string text;
      var result = _gameModResourceService.LoadTextFile(modId, "", sourcefile, extension, out text);

      if (!result)
      {
        Plugin.Log.LogWarning($"Doesn't find \"{language}\" translation for {filename}{extension} in modId:{modId}");
        return;
      }

      // writes text
      var extractionFolder = Path.Combine("extracted", ""+modId);
      var outputFilename = $"{filename}_{language}{extension}";
      Plugin.writeFileInPluginFolder(extractionFolder, outputFilename, text);

      Plugin.Log.LogInfo($"File extracted/{outputFilename} extracted");

    }
  }
}