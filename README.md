# TFM LocalizationMod
A mod for [*TFM: The First Men*](https://store.steampowered.com/app/700820/).  
This mod provide a workaround fix to translation display issues in characters's crafting panel.  
It can also extracting or importing translations.

## Install
1. Download and install [BepInEx](https://github.com/BepInEx/BepInEx/releases/latest)
2. Run the game once to generate relating folders, close.
3. Download this plugin from releases at right and unzip it to "<game_root>/BepInEx/plugins"
4. Run the game.

## Extract built-in text and translations (for translators)
0. Install and run the game after that (if you haven't).
1. Close the game and navigate to "<game_root>/BepInEx/plugins/LocalizationMod"
2. Open config.cfg, modify `languagesToExtract` to ones you want and set `enableExtracting` to `true`. Save & close.
3. Run the game and waiting for it to finished. The game will be stuck when extracting.  
  You can enable BepInEx's console window for logging information.
4. The extracted files will reside in "<game_root>/BepInEx/plugins/LocalizationMod/extracted"

### Bonus: Merge translated text into English text for further translation 
Check out https://observablehq.com/d/868f3531598e4d2e, and follow instructions.
You can do it when the game updated to merge latest untranslated text.

## Import custom translations files (for players)
1. Put translations in "<game_root>/BepInEx/plugins/LocalizationMod/langs" 
2. Its path should be exactly the same as when it was extracted, but in "langs" folder rather than "extracted".
3. The game will have a little longer loading time as injecting translations from disk.

## Develop locally
0. Setup development environment for dotNET.
1. Clone this project.
2. Go the game's folder, copy "Assembly-CSharp.dll" to "lib" folder in the project folder
3. Choose your favorite editor and IDE, type `> dotnet build` to compile it to DLL.