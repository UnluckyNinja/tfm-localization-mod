using HarmonyLib;
using Tfm.GameCore.MapObject.Models;
using Tfm.Main.Localization.Utilities;

namespace LocalizationMod
{

  public class PatchBuildingFix
  {
    // patch localizations for construcitons to include addtional properties
    // as a method to provide compatibility for UIs that don't use `displayName`
    [HarmonyPatch(typeof(LocalizationImporterHandlerForConstruction), "ReadItemFromFile")]
    [HarmonyPostfix]
    static void patchReadItemFromFile(ConstructionDefinitionVO constructionDef, ConstructionDefinitionLocalizationFileItem item, string language)
    {
      //   Log.LogDebug($"Patching Name: {item.name}, DisplayName: {item.displayName}");
      if (!string.IsNullOrEmpty(item.name))
      {
        constructionDef.name.SetValue(language, item.name);
      }
      else if (!string.IsNullOrEmpty(item.displayName))
      {
        constructionDef.name.SetValue(language, item.displayName);
      }
    }

    // patch localizations for doodads to include addtional properties
    // as a method to provide compatibility for UIs that don't use `displayName`
    [HarmonyPatch(typeof(LocalizationImporterHandlerForDoodad), "ReadItemFromFile")] // Specify target method with HarmonyPatch attribute
    [HarmonyPostfix]
    static void patchReadItemFromFile(DoodadDefinitionVO doodadDef, DoodadDefinitionLocalizationFileItem item, string language)
    {
      //   Log.LogDebug($"Patching Name: {item.name}, DisplayName: {item.displayName}");
      if (!string.IsNullOrEmpty(item.name))
      {
        doodadDef.name.SetValue(language, item.name);
      }
      else if (!string.IsNullOrEmpty(item.displayName))
      {
        doodadDef.name.SetValue(language, item.displayName);
      }
    }
  }
}
