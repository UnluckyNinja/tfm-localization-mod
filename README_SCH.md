# TFM LocalizationMod
一个MOD [*TFM: The First Men*](https://store.steampowered.com/app/700820/).  
这个MOD修复了游戏内右下角人物面板中，制造面板的显示问题（无论是否翻译都只显示英文）。
也可以用来提取和注入游戏文本

## 安装
1. 下载并安装 [BepInEx](https://github.com/BepInEx/BepInEx/releases/latest)，根据系统选择x86或者x64，解压到游戏根目录
2. 先运行一次游戏，生成相关目录和文件。
3. 在本项目页面右侧的“「Releases](https://github.com/UnluckyNinja/tfm-localization-mod/releases/latest)”中下载最新的MOD文件并解压，解压后保持目录结构为 "<game_root>/BepInEx/plugins/LocalizationMod/LocalizationMod.dll"
4. 启动游戏即完成安装（翻译文件另行下载）。

## 提取游戏内文本和翻译 (翻译者用)
0. 先按照上面说明安装MOD (如果你还没有这么做的话)。
1. 关闭游戏，打开文件夹 "<game_root>/BepInEx/plugins/LocalizationMod"
2. 再打开 config.cfg, 修改 `languagesToExtract` 为你想要的语言（例如 "en,zh"），并设置 `enableExtracting` 为 `true`。保存并关闭。
3. 启动游戏等待提取完成，游戏会在提取时卡住，请耐心等待。  
  你可以启用 BepInEx 的控制台窗口查看输出信息以防万一出现错误。
4. 提取的文本会被放置在 "<game_root>/BepInEx/plugins/LocalizationMod/extracted" 文件夹下

### 额外：将翻译文本整合进英文文本中，来进一步翻译
请参考 https://observablehq.com/d/868f3531598e4d2e （英文），按照指示进行。
当游戏更新时，你可以据此将最新的英文文本整合进已翻译的文本中。

## 注入翻译文本 (玩家用)
1. 把翻译放入MOD文件夹，结构应为 "<game_root>/BepInEx/plugins/LocalizationMod/langs/\*/\*.json" 
2. 结构与文件名需要与导出时一致，只是顶层文件夹从“extracted”变为“langs”
3. 读取本地翻译时，游戏启动时间可能会长一点。

## Develop locally
0. 设置好 dotNET 开发环境.
1. 克隆本项目.
2. 去游戏目录复制 "tfm_Data/Managed/Assembly-CSharp.dll" 文件到项目的 "lib" 文件夹下
3. 用编辑器或IDE打开项目，终端输入 `> dotnet build` 构建dll。