using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bit34.Unity.Localization.Models.Impl;
using Bit34.Unity.Localization.Models.VO;
using HarmonyLib;
using Newtonsoft.Json;
using Tfm.Game.Decision.Models;
using Tfm.Game.Services;
using Tfm.Game.Utilities;
using Tfm.GameCore.Activity.Models;
using Tfm.GameCore.Activity.Services;
using Tfm.GameCore.CharacterPreset.Models;
using Tfm.GameCore.CharacterPreset.Services;
using Tfm.GameCore.Decision.Services;
using Tfm.GameCore.EntityTech.Models;
using Tfm.GameCore.EntityTech.Services;
using Tfm.GameCore.Item.Models;
using Tfm.GameCore.Item.Services;
using Tfm.GameCore.Loot.Models;
using Tfm.GameCore.Loot.Services;
using Tfm.GameCore.MapObject.Models;
using Tfm.GameCore.Skill.Models;
using Tfm.GameCore.Skill.Services;
using Tfm.GameCore.Trait.Models;
using Tfm.GameCore.Trait.Services;
using Tfm.Main.Localization.Constants;
using Tfm.Main.Localization.Utilities;

namespace LocalizationMod
{

  /**
   * Append loading local translation files to original method
   */
  public class PatchImport
  {

    [HarmonyPatch(typeof(LocalizationDataUtilities), "LoadAllUI")]
    [HarmonyPostfix]
    static void patchLoadingUITranslation(
      GameModResourceService ____gameModResourceService,
      LocalizationModel ____localizationModel
    )
    {
      foreach (string language in Languages.Translations)
      {
        DoLoadUI(0, "UI", ".json", language, ____gameModResourceService, ____localizationModel);
      }
    }

    public static void DoLoadUI(
      int modId,
      string filename,
      string extension,
      string language,
      GameModResourceService _gameModResourceService,
      LocalizationModel _localizationModel
    )
    {
      var langsFolder = Path.Combine("langs", "" + modId);
      var sourceFilename = $"{filename}_{language}{extension}";

      if (!Plugin.fileExistsInPluginFolder(langsFolder, sourceFilename))
      {
        return;
      }

      Plugin.Log.LogDebug($"Importing {langsFolder}\\{sourceFilename}");

      string text = Plugin.readFileInPluginFolder(langsFolder, sourceFilename);
      var UI_Strings = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);

      // reflection to get private array
      var propertyInfo = typeof(LocalizationModel).GetProperty("_languages", BindingFlags.Instance | BindingFlags.NonPublic);
      List<LocalizedContentVO> localModels = (List<LocalizedContentVO>)propertyInfo.GetValue(_localizationModel);

      var localizedContentVO = localModels[_localizationModel.GetLanguageIndex(language)];
      foreach (var item in UI_Strings)
      {
        localizedContentVO.Strings[item.Key] = item.Value;
      }
      Plugin.Log.LogInfo($"Translation file loaded: {Path.Combine("langs", ""+modId, sourceFilename)}.");
    }

    [HarmonyPatch(typeof(LocalizationImporterHandlerForActivity), "LoadContent")]
    [HarmonyPostfix]
    static void patchActivity(
      int modId,
      string language,
      LocalizationImporterHandlerForActivity __instance,
      ActivityDefinitionsModel ____activityDefinitionsModel
    )
    {
      var model = ____activityDefinitionsModel;
      var type = __instance.GetType();

      var filename = $"{__instance.FILE_NAME}_{language}{__instance.FILE_EXTENSION}";
      ActivityDefinitionLocalizationFile defs;
      if (!load<ActivityDefinitionLocalizationFile>(modId, language, filename, out defs))
      {
        return;
      }

      foreach (var definitionLocalizationFileItem in defs.definitions)
      {
        long num = GameModHelpers.EncodeDefinitionId(modId, definitionLocalizationFileItem.id);
        if (model.HasDefinition(num))
        {
          var ReadItemFromFile = type.GetMethod("ReadItemFromFile", BindingFlags.Instance | BindingFlags.NonPublic);
          ReadItemFromFile.Invoke(__instance, new object[] { model.GetDefinition(num), definitionLocalizationFileItem, language });
        }
      }
      Plugin.Log.LogInfo($"Translation file loaded: {Path.Combine("langs", "" + modId, filename)}.");
    }

    [HarmonyPatch(typeof(LocalizationImporterHandlerForCharacterPreset), "LoadContent")]
    [HarmonyPostfix]
    static void patchCharacterPreset(
      int modId,
      string language,
      LocalizationImporterHandlerForCharacterPreset __instance,
      CharacterPresetDefinitionsModel ____characterPresetDefinitionsModel
    )
    {
      var model = ____characterPresetDefinitionsModel;
      var type = __instance.GetType();

      var filename = $"{__instance.FILE_NAME}_{language}{__instance.FILE_EXTENSION}";
       CharacterPresetLocalizationFile defs;
      if (!load< CharacterPresetLocalizationFile>(modId, language, filename, out defs))
      {
        return;
      }

      foreach (var definitionLocalizationFileItem in defs.definitions)
      {
        long num = GameModHelpers.EncodeDefinitionId(modId, definitionLocalizationFileItem.id);
        if (model.HasDefinition(num))
        {
          var ReadItemFromFile = type.GetMethod("ReadItemFromFile", BindingFlags.Instance | BindingFlags.NonPublic);
          ReadItemFromFile.Invoke(__instance, new object[] { model.GetDefinition(num), definitionLocalizationFileItem, language });
        }
      }
      Plugin.Log.LogInfo($"Translation file loaded: {Path.Combine("langs", "" + modId, filename)}.");
    }

    [HarmonyPatch(typeof(LocalizationImporterHandlerForConstruction), "LoadContent")]
    [HarmonyPostfix]
    static void patchConstruction(
      int modId,
      string language,
      LocalizationImporterHandlerForConstruction __instance,
      ConstructionDefinitionsModel ____constructionDefinitionsModel
    )
    {
      var model = ____constructionDefinitionsModel;
      var type = __instance.GetType();

      var filename = $"{__instance.FILE_NAME}_{language}{__instance.FILE_EXTENSION}";
      ConstructionDefinitionLocalizationFile defs;
      if (!load<ConstructionDefinitionLocalizationFile>(modId, language, filename, out defs))
      {
        return;
      }

      foreach (var definitionLocalizationFileItem in defs.definitions)
      {
        long num = GameModHelpers.EncodeDefinitionId(modId, definitionLocalizationFileItem.id);
        if (model.HasDefinition(num))
        {
          var ReadItemFromFile = type.GetMethod("ReadItemFromFile", BindingFlags.Instance | BindingFlags.NonPublic);
          ReadItemFromFile.Invoke(__instance, new object[] { model.GetDefinition(num), definitionLocalizationFileItem, language });
        }
      }
      Plugin.Log.LogInfo($"Translation file loaded: {Path.Combine("langs", "" + modId, filename)}.");
    }

    [HarmonyPatch(typeof(LocalizationImporterHandlerForDecision), "LoadContent")]
    [HarmonyPostfix]
    static void patchDecision(
      int modId,
      string language,
      LocalizationImporterHandlerForDecision __instance,
      DecisionDefinitionsModel ____decisionDefinitionsModel
    )
    {
      var model = ____decisionDefinitionsModel;
      var type = __instance.GetType();

      var filename = $"{__instance.FILE_NAME}_{language}{__instance.FILE_EXTENSION}";
      DecisionDefinitionLocalizationFile defs;
      if (!load<DecisionDefinitionLocalizationFile>(modId, language, filename, out defs))
      {
        return;
      }

      foreach (var definitionLocalizationFileItem in defs.definitions)
      {
        long num = GameModHelpers.EncodeDefinitionId(modId, definitionLocalizationFileItem.id);
        if (model.HasDefinition(num))
        {
          var ReadItemFromFile = type.GetMethod("ReadItemFromFile", BindingFlags.Instance | BindingFlags.NonPublic);
          ReadItemFromFile.Invoke(__instance, new object[] { model.GetDefinition(num), definitionLocalizationFileItem, language });
        }
      }
      Plugin.Log.LogInfo($"Translation file loaded: {Path.Combine("langs", "" + modId, filename)}.");
    }

    [HarmonyPatch(typeof(LocalizationImporterHandlerForDoodad), "LoadContent")]
    [HarmonyPostfix]
    static void patchDoodad(
      int modId,
      string language,
      LocalizationImporterHandlerForDoodad __instance,
      DoodadDefinitionsModel ____doodadDefinitionsModel
    )
    {
      var model = ____doodadDefinitionsModel;
      var type = __instance.GetType();

      var filename = $"{__instance.FILE_NAME}_{language}{__instance.FILE_EXTENSION}";
      DoodadDefinitionLocalizationFile defs;
      if (!load<DoodadDefinitionLocalizationFile>(modId, language, filename, out defs))
      {
        return;
      }

      foreach (var definitionLocalizationFileItem in defs.definitions)
      {
        long num = GameModHelpers.EncodeDefinitionId(modId, definitionLocalizationFileItem.id);
        if (model.HasDefinition(num))
        {
          var ReadItemFromFile = type.GetMethod("ReadItemFromFile", BindingFlags.Instance | BindingFlags.NonPublic);
          ReadItemFromFile.Invoke(__instance, new object[] { model.GetDefinition(num), definitionLocalizationFileItem, language });
        }
      }
      Plugin.Log.LogInfo($"Translation file loaded: {Path.Combine("langs", "" + modId, filename)}.");
    }

    [HarmonyPatch(typeof(LocalizationImporterHandlerForEntityTech), "LoadContent")]
    [HarmonyPostfix]
    static void patchEntityTech(
      int modId,
      string language,
      LocalizationImporterHandlerForEntityTech __instance,
      EntityTechDefinitionsModel ____entityTechDefinitionsModel
    )
    {
      var model = ____entityTechDefinitionsModel;
      var type = __instance.GetType();

      var filename = $"{__instance.FILE_NAME}_{language}{__instance.FILE_EXTENSION}";
      EntityTechDefinitionLocalizationFile defs;
      if (!load<EntityTechDefinitionLocalizationFile>(modId, language, filename, out defs))
      {
        return;
      }

      foreach (var definitionLocalizationFileItem in defs.definitions)
      {
        long num = GameModHelpers.EncodeDefinitionId(modId, definitionLocalizationFileItem.id);
        if (model.HasDefinition(num))
        {
          var ReadItemFromFile = type.GetMethod("ReadItemFromFile", BindingFlags.Instance | BindingFlags.NonPublic);
          ReadItemFromFile.Invoke(__instance, new object[] { model.GetDefinition(num), definitionLocalizationFileItem, language });
        }
      }
      Plugin.Log.LogInfo($"Translation file loaded: {Path.Combine("langs", "" + modId, filename)}.");
    }

    [HarmonyPatch(typeof(LocalizationImporterHandlerForItem), "LoadContent")]
    [HarmonyPostfix]
    static void patchItem(
      int modId,
      string language,
      LocalizationImporterHandlerForItem __instance,
      ItemDefinitionsModel ____itemDefinitionsModel
    )
    {

      var model = ____itemDefinitionsModel;
      var type = __instance.GetType();

      var filename = $"{__instance.FILE_NAME}_{language}{__instance.FILE_EXTENSION}";
      ItemDefinitionLocalizationFile defs;
      if (!load<ItemDefinitionLocalizationFile>(modId, language, filename, out defs))
      {
        return;
      }

      foreach (var definitionLocalizationFileItem in defs.definitions)
      {
        long num = GameModHelpers.EncodeDefinitionId(modId, definitionLocalizationFileItem.id);
        if (model.HasDefinition(num))
        {
          var ReadItemFromFile = type.GetMethod("ReadItemFromFile", BindingFlags.Instance | BindingFlags.NonPublic);
          ReadItemFromFile.Invoke(__instance, new object[] { model.GetDefinition(num), definitionLocalizationFileItem, language });
        }
      }
      Plugin.Log.LogInfo($"Translation file loaded: {Path.Combine("langs", "" + modId, filename)}.");
    }

    [HarmonyPatch(typeof(LocalizationImporterHandlerForLoot), "LoadContent")]
    [HarmonyPostfix]
    static void patchLoot(
      int modId,
      string language,
      LocalizationImporterHandlerForLoot __instance,
      LootDefinitionsModel ____lootDefinitionsModel
    )
    {

      var model = ____lootDefinitionsModel;
      var type = __instance.GetType();

      var filename = $"{__instance.FILE_NAME}_{language}{__instance.FILE_EXTENSION}";
      LootDefinitionLocalizationFile defs;
      if (!load<LootDefinitionLocalizationFile>(modId, language, filename, out defs))
      {
        return;
      }

      foreach (var definitionLocalizationFileItem in defs.definitions)
      {
        long num = GameModHelpers.EncodeDefinitionId(modId, definitionLocalizationFileItem.id);
        if (model.HasDefinition(num))
        {
          var ReadItemFromFile = type.GetMethod("ReadItemFromFile", BindingFlags.Instance | BindingFlags.NonPublic);
          ReadItemFromFile.Invoke(__instance, new object[] { model.GetDefinition(num), definitionLocalizationFileItem, language });
        }
      }
      Plugin.Log.LogInfo($"Translation file loaded: {Path.Combine("langs", "" + modId, filename)}.");
    }

    [HarmonyPatch(typeof(LocalizationImporterHandlerForSkill), "LoadContent")]
    [HarmonyPostfix]
    static void patchSkill(
      int modId,
      string language,
      LocalizationImporterHandlerForSkill __instance,
      SkillDefinitionsModel ____skillDefinitionsModel
    )
    {

      var model = ____skillDefinitionsModel;
      var type = __instance.GetType();

      var filename = $"{__instance.FILE_NAME}_{language}{__instance.FILE_EXTENSION}";
      SkillDefinitionLocalizationFile defs;
      if (!load<SkillDefinitionLocalizationFile>(modId, language, filename, out defs))
      {
        return;
      }

      foreach (var definitionLocalizationFileItem in defs.definitions)
      {
        long num = GameModHelpers.EncodeDefinitionId(modId, definitionLocalizationFileItem.id);
        if (model.HasDefinition(num))
        {
          var ReadItemFromFile = type.GetMethod("ReadItemFromFile", BindingFlags.Instance | BindingFlags.NonPublic);
          ReadItemFromFile.Invoke(__instance, new object[] { model.GetDefinition(num), definitionLocalizationFileItem, language });
        }
      }
      Plugin.Log.LogInfo($"Translation file loaded: {Path.Combine("langs", "" + modId, filename)}.");
    }

    [HarmonyPatch(typeof(LocalizationImporterHandlerForTrait), "LoadContent")]
    [HarmonyPostfix]
    static void patchTrait(
      int modId,
      string language,
      LocalizationImporterHandlerForTrait __instance,
      TraitDefinitionsModel ____traitDefinitionsModel
    )
    {
      var model = ____traitDefinitionsModel;
      var type = __instance.GetType();

      var filename = $"{__instance.FILE_NAME}_{language}{__instance.FILE_EXTENSION}";
      TraitDefinitionLocalizationFile defs;
      if (!load<TraitDefinitionLocalizationFile>(modId, language, filename, out defs))
      {
        return;
      }

      foreach (var definitionLocalizationFileItem in defs.definitions)
      {
        long num = GameModHelpers.EncodeDefinitionId(modId, definitionLocalizationFileItem.id);
        if (model.HasDefinition(num))
        {
          var ReadItemFromFile = type.GetMethod("ReadItemFromFile", BindingFlags.Instance | BindingFlags.NonPublic);
          ReadItemFromFile.Invoke(__instance, new object[] { model.GetDefinition(num), definitionLocalizationFileItem, language });
        }
      }
      Plugin.Log.LogInfo($"Translation file loaded: {Path.Combine("langs", "" + modId, filename)}.");
    }

    static bool load<T>(
      int modId,
      string language,
      string filename,
      out T definitionLocalizationFile
    ) where T : class
    {
      var langsFolder = Path.Combine("langs", "" + modId);
      var sourceFile = Path.Combine(langsFolder, filename);
      Plugin.Log.LogDebug($"Checking translation file: {sourceFile}");

      if (!Plugin.fileExistsInPluginFolder(langsFolder, filename))
      {
        Plugin.Log.LogDebug($"Translation file Doesn't exists, skipped.");
        definitionLocalizationFile = null;
        return false;
      }

      var text = Plugin.readFileInPluginFolder(langsFolder, filename);
      definitionLocalizationFile = JsonConvert.DeserializeObject<T>(text);
      return true;
    }
  }
}