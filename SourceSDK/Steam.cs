﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceModdingTool
{
    public class Steam
    {
        string currentGame = string.Empty;
        string currentMod = string.Empty;
        Dictionary<string, string> games = new Dictionary<string, string>();
        Dictionary<string, string> mods = new Dictionary<string, string>();
        public List<string> libraries;

        public Steam() { LoadLibraries(); }

        private void CopySlartibartysHammer()
        {
            string gamePath = GetGamePath();
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            if(currentGame == "Source SDK Base 2013 Singleplayer")
            {
                File.Copy(startupPath + "\\Tools\\SlartibartysHammer\\sp\\hammer.exe",
                          gamePath + "\\bin\\hammer.exe",
                          true);
                File.Copy(startupPath + "\\Tools\\SlartibartysHammer\\sp\\hammer_dll.dll",
                          gamePath + "\\bin\\hammer_dll.dll",
                          true);
                File.Copy(startupPath + "\\Tools\\SlartibartysHammer\\sp\\hammer_run_map_launcher.exe",
                          gamePath + "\\bin\\hammer_run_map_launcher.exe",
                          true);
                File.Copy(startupPath + "\\Tools\\SlartibartysHammer\\sp\\vbsp.exe",
                          gamePath + "\\bin\\vbsp.exe",
                          true);
                File.Copy(startupPath + "\\Tools\\SlartibartysHammer\\sp\\vrad.exe", gamePath + "\\bin\\vrad.exe", true);
                File.Copy(startupPath + "\\Tools\\SlartibartysHammer\\sp\\vrad_dll.dll",
                          gamePath + "\\bin\\vrad_dll.dll",
                          true);
                File.Copy(startupPath + "\\Tools\\SlartibartysHammer\\sp\\vvis.exe", gamePath + "\\bin\\vvis.exe", true);
                File.Copy(startupPath + "\\Tools\\SlartibartysHammer\\sp\\vvis_dll.dll",
                          gamePath + "\\bin\\vvis_dll.dll",
                          true);
            }
        }

        private void CreateGameConfig()
        {
            string gamePath = GetGamePath();
            string modPath = GetModPath();

            if(File.Exists(gamePath + "\\bin\\propper.fgd"))
                File.Delete(gamePath + "\\bin\\propper.fgd");

            if(File.Exists(gamePath + "\\bin\\propper.exe"))
                File.Delete(gamePath + "\\bin\\propper.exe");

            List<string> fgds = getFGDs();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\"Configs\"");
            sb.AppendLine("{");
            sb.AppendLine(" \"Games\"");
            sb.AppendLine(" {");
            sb.AppendLine("     \"My game\"");
            sb.AppendLine("     {");
            sb.AppendLine("         \"GameDir\"		\"" + modPath + "\"");
            sb.AppendLine("         \"Hammer\"");
            sb.AppendLine("         {");
            for(int i = 0; i < fgds.Count; i++)
            {
                sb.AppendLine("             \"GameData" + i + "\"		\"" + fgds[i] + "\"");
            }

            sb.AppendLine("             \"DefaultTextureScale\"		\"0.250000\"");
            sb.AppendLine("             \"DefaultLightmapScale\"		\"16\"");
            sb.AppendLine("             \"GameExe\"		\"" + gamePath + "\\hl2.exe\"");
            sb.AppendLine("             \"DefaultSolidEntity\"		\"func_detail\"");
            sb.AppendLine("             \"DefaultPointEntity\"		\"info_player_start\"");
            sb.AppendLine("             \"BSP\"		\"" + gamePath + "\\bin\\vbsp.exe\"");
            sb.AppendLine("             \"Vis\"		\"" + gamePath + "\\bin\\vvis.exe\"");
            sb.AppendLine("             \"Light\"		\"" + gamePath + "\\bin\\vrad.exe\"");
            sb.AppendLine("             \"GameExeDir\"		\"" + gamePath + "\"");
            sb.AppendLine("             \"MapDir\"		\"" + gamePath + "\\sourcesdk_content\\ep2\\mapsrc\"");
            sb.AppendLine("             \"BSPDir\"		\"" + modPath + "\\maps\"");
            sb.AppendLine("             \"CordonTexture\"		\"tools\\toolsskybox\"");
            sb.AppendLine("             \"MaterialExcludeCount\"		\"0\"");
            sb.AppendLine("         }");
            sb.AppendLine("     }");
            sb.AppendLine(" }");
            sb.AppendLine(" \"SDKVersion\"		\"5\"");
            sb.AppendLine("}");

            File.WriteAllText(gamePath + "\\bin\\GameConfig.txt", sb.ToString());
        }

        private void CreatePropperConfig()
        {
            string gamePath = GetGamePath();
            string modPath = GetModPath();

            File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\Tools\\Propper\\propper.fgd",
                      gamePath + "\\bin\\propper.fgd",
                      true);
            File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\Tools\\Propper\\propper.exe",
                      gamePath + "\\bin\\propper.exe",
                      true);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\"Configs\"");
            sb.AppendLine("{");
            sb.AppendLine(" \"Games\"");
            sb.AppendLine(" {");
            sb.AppendLine("     \"My game\"");
            sb.AppendLine("     {");
            sb.AppendLine("         \"GameDir\"		\"" + modPath + "\"");
            sb.AppendLine("         \"Hammer\"");
            sb.AppendLine("         {");
            sb.AppendLine("             \"GameData0\"		\"" + gamePath + "\\bin\\propper.fgd\"");
            sb.AppendLine("             \"DefaultTextureScale\"		\"0.250000\"");
            sb.AppendLine("             \"DefaultLightmapScale\"		\"16\"");
            sb.AppendLine("             \"GameExe\"		\"" + gamePath + "\\hl2.exe\"");
            sb.AppendLine("             \"DefaultSolidEntity\"		\"propper_model\"");
            sb.AppendLine("             \"DefaultPointEntity\"		\"propper_skins\"");
            sb.AppendLine("             \"GameExeDir\"		\"" + gamePath + "\"");
            sb.AppendLine("             \"MapDir\"		\"" + gamePath + "\\sourcesdk_content\\ep2\\mapsrc\"");
            sb.AppendLine("             \"BSPDir\"		\"" + modPath + "\\maps\"");
            sb.AppendLine("             \"CordonTexture\"		\"tools\\toolsskybox\"");
            sb.AppendLine("             \"MaterialExcludeCount\"		\"0\"");
            sb.AppendLine("         }");
            sb.AppendLine("     }");
            sb.AppendLine(" }");
            sb.AppendLine(" \"SDKVersion\"		\"5\"");
            sb.AppendLine("}");

            File.WriteAllText(gamePath + "\\bin\\GameConfig.txt", sb.ToString());
        }

        private Dictionary<string, string> LoadGames()
        {
            games = new Dictionary<string, string>();
            if(libraries.Count > 0)
            {
                foreach(string library in libraries)
                {
                    foreach(String path in Directory.GetDirectories(library + "\\steamapps\\common\\"))
                    {
                        String game = new FileInfo(path).Name;

                        if(File.Exists(library + "\\steamapps\\common\\" + game + "\\bin\\engine.dll"))
                            games.Add(game, library + "\\steamapps\\common\\" + game);
                    }
                }
            }
            return games;
        }

        private void LoadLibraries()
        {
            String steamPath = GetInstallPath();

            libraries = new List<string>();
            libraries.Add(steamPath);

            SourceSDK.KeyValue root = SourceSDK.KeyValue.readChunkfile(steamPath + "\\steamapps\\libraryfolders.vdf");

            foreach(SourceSDK.KeyValue child in root.getChildren())
            {
                string dir = child.getValue().Replace("\\\\", "\\");
                if(Directory.Exists(dir))
                    libraries.Add(dir);
            }
        }

        /// <summary>
        /// Loads all the mods with the same game app id of the specified game
        /// </summary>
        /// <param name="game">The base game name (i.e. Source SDK Base 2013 Singleplayer)</param>
        /// <returns></returns>
        private Dictionary<string, string> LoadMods(String game)
        {
            mods = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(game))
                return mods;
            
            int gameAppId = GetGameAppId(game);
            string gamePath = GetGamePath();

            if (gameAppId == -1 || gamePath == null)
                return mods;

            List<string> paths = GetAllModPaths();
            
            foreach (string path in GetAllBaseGameinfoFolders(currentGame))
                paths.Add(gamePath + "\\" + path);
            foreach(string path in paths)
            {
                SourceSDK.KeyValue gameInfo = SourceSDK.KeyValue.readChunkfile(path + "\\gameinfo.txt");

                string name = gameInfo.getChildByKey("game").getValue() + " (" + new DirectoryInfo(path).Name + ")";
                string modAppId = gameInfo.getChildByKey("filesystem").getChildByKey("steamappid").getValue();

                if(int.Parse(modAppId) == gameAppId || path.Contains(gamePath))
                {
                    while (mods.Keys.Contains(name))
                        name = name + "_";
                    mods.Add(name, path);
                }
            }
            return mods;
        }

        /// <summary>
        /// Returns a list of all the folders with game infos of the specific game
        /// </summary>
        /// <param name="game">The base game name (i.e. Source SDK Base 2013 Singleplayer)</param>
        /// <returns></returns>
        public List<string> GetAllBaseGameinfoFolders(string game)
        {
            List<string> mods = new List<string>();

            if (string.IsNullOrEmpty(game))
                return mods;
            
            string gamePath = GetGamesList()[game];
            foreach(String path in Directory.GetDirectories(gamePath))
            {
                String gameBranch = new FileInfo(path).Name;

                if(File.Exists(gamePath + "\\" + gameBranch + "\\gameinfo.txt"))
                    mods.Add(gameBranch);
            }
            return mods;
        }

        public List<string> GetAllModPaths()
        {
            List<string> mods = new List<string>();
            string library = Steam.GetInstallPath();
            foreach(String path in Directory.GetDirectories(library + "\\steamapps\\sourcemods\\"))
            {
                String game = new FileInfo(path).Name;

                if(File.Exists(library + "\\steamapps\\sourcemods\\" + game + "\\gameinfo.txt"))
                {
                    mods.Add(library + "\\steamapps\\sourcemods\\" + game);
                }
            }
            return mods;
        }

        public List<string> getFGDs()
        {
            List<string> result = new List<string>();

            string gamePath = GetGamePath();
            string modPath = GetModPath();

            result.AddRange(Directory.GetFiles(gamePath, "*.fgd", SearchOption.AllDirectories));
            result.AddRange(Directory.GetFiles(modPath, "*.fgd", SearchOption.AllDirectories));

            List<string> alreadyIncluded = new List<string>();

            foreach(string fgd in result)
            {
                string line;
                StreamReader file = new StreamReader(fgd);
                while((line = file.ReadLine()) != null)
                {
                    if(line.Trim().StartsWith("@include"))
                    {
                        line = line.Replace("@include", string.Empty).Replace("\"", string.Empty).Trim();
                        alreadyIncluded.Add(new FileInfo(fgd).Directory.FullName + "\\" + line);
                        alreadyIncluded.Add(gamePath + "\\bin\\" + line);
                    }
                }
            }

            foreach(string fgd in alreadyIncluded.Distinct().ToList())
            {
                result.Remove(fgd);
            }

            return result;
        }

        public int GetGameAppId(string game)
        {
            if(game == string.Empty)
                return -1;

            if(!games.ContainsKey(game))
                GetGamesList();

            string gamePath = games[game];
            if(File.Exists(gamePath + "\\steam_appid.txt"))
            {
                string steam_appid = File.ReadAllText(gamePath + "\\steam_appid.txt");
                return int.Parse(steam_appid);
            }
            return -1;
        }

        public string GetGamePath() { return GetGamePath(currentGame); }

        public string GetGamePath(string game)
        {
            if(games.ContainsKey(game))
                return games[game];
            else
            {
                LoadGames();
                if(games.ContainsKey(game))
                    return games[game];
                else
                    return null;
            }
        }

        public Dictionary<string, string> GetGamesList()
        {
            LoadLibraries();
            return LoadGames();
        }

        public static string GetInstallPath()
        { return Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Valve\\Steam", "InstallPath", null).ToString(); }

        public List<string> getModMountedPaths() { return getModMountedPaths(currentGame, currentMod); }

        public List<string> getModMountedPaths(string game, string mod)
        {
            List<string> result = new List<string>();

            string gamePath = GetGamePath(game);
            string modPath = GetModPath(game, mod);

            SourceSDK.KeyValue gameInfo = SourceSDK.KeyValue.readChunkfile(modPath + "\\gameinfo.txt");

            SourceSDK.KeyValue searchPaths = gameInfo.findChildByKey("searchpaths");
            foreach(SourceSDK.KeyValue searchPath in searchPaths.getChildren())
            {
                string[] keys = searchPath.getKey().Split('+');

                if(!keys.Contains("game"))
                    continue;

                string value = searchPath.getValue();
                value = value.Replace("/", "\\");
                value = value.Replace("|all_source_engine_paths|", gamePath + "\\");
                value = value.Replace("|gameinfo_path|", modPath + "\\");
                value = value.Replace("\\\\", "\\");
                if(value.EndsWith("/"))
                    value = value.Substring(0, value.Length - 1);

                result.Add(value);
            }

            return result.Distinct().ToList();
        }

        public List<string> getModMountedVPKs() { return getModMountedPaths().Where(x => x.EndsWith(".vpk")).ToList(); }

        public string GetModPath() { return GetModPath(currentGame, currentMod); }

        public string GetModPath(string game, string mod)
        {
            if(game != currentGame || !games.ContainsKey(game))
            {
                if(!games.ContainsKey(game))
                    LoadGames();

                if(games.ContainsKey(game))
                    LoadMods(game);
                else
                    return null;
            }

            if(mod != currentMod || !mods.ContainsKey(mod))
            {
                if(!mods.ContainsKey(mod))
                    LoadMods(game);

                if(!mods.ContainsKey(mod))
                    return null;
            }

            string path = mods[mod];

            if(game != currentGame)
                LoadMods(currentGame);

            return path;
        }

        public List<string> getModSearchPaths() { return getModSearchPaths(currentGame, currentMod); }

        public List<string> getModSearchPaths(string game, string mod)
        { return getModMountedPaths(game, mod).Where(x => Directory.Exists(x)).ToList(); }

        public Dictionary<string, string> GetModsList(string game)
        {
            LoadGames();
            LoadMods(game);
            return mods;
        }

        public void OpenModFolder(string mod)
        {
            string modPath = mods[mod];
            Process.Start(modPath);
        }

        public void RunHammer(string startupPath)
        {
            string gamePath = GetGamePath();
            string modPath = GetModPath();

            CopySlartibartysHammer();

            CreateGameConfig();

            string hammerPath = gamePath + "\\bin\\hammer.exe";
            Debug.Write("Hammer: " + hammerPath);

            Process ffmpeg = new Process();
            ffmpeg.StartInfo.FileName = hammerPath;
            ffmpeg.StartInfo.Arguments = string.Empty;
            ffmpeg.Start();
        }

        public void RunPropperHammer()
        {
            string gamePath = GetGamePath();
            string modPath = GetModPath();

            CopySlartibartysHammer();
            CreatePropperConfig();

            string hammerPath = gamePath + "\\bin\\hammer.exe";
            Debug.Write("Hammer: " + hammerPath);

            Process ffmpeg = new Process();
            ffmpeg.StartInfo.FileName = hammerPath;
            ffmpeg.StartInfo.Arguments = string.Empty;
            ffmpeg.Start();
        }

        public void setCurrentGame(string game)
        {
            currentGame = game;
            LoadMods(game);
        }

        public void setCurrentMod(string mod) { currentMod = mod; }
        public void CleanModFolder()
        {
            //string modPath = sourceSDK.GetModPath(toolsGames.EditValue.ToString(), toolsMods.EditValue.ToString());
            string modPath = GetModPath();

            if (File.Exists(modPath + "\\Gamestate.txt"))
                File.Delete(modPath + "\\Gamestate.txt");
            if (File.Exists(modPath + "\\demoheader.tmp"))
                File.Delete(modPath + "\\demoheader.tmp");
            if (File.Exists(modPath + "\\ep1_gamestats.dat"))
                File.Delete(modPath + "\\ep1_gamestats.dat");
            if (File.Exists(modPath + "\\modelsounds.cache"))
                File.Delete(modPath + "\\modelsounds.cache");
            if (File.Exists(modPath + "\\stats.txt"))
                File.Delete(modPath + "\\stats.txt");
            if (File.Exists(modPath + "\\voice_ban.dt"))
                File.Delete(modPath + "\\voice_ban.dt");
            if (File.Exists(modPath + "\\cfg\\config.cfg"))
                File.Delete(modPath + "\\cfg\\config.cfg");
            if (File.Exists(modPath + "\\cfg\\server_blacklist.txt"))
                File.Delete(modPath + "\\cfg\\server_blacklist.txt");
            if (File.Exists(modPath + "\\sound\\sound.cache"))
                File.Delete(modPath + "\\sound\\sound.cache");
            if (File.Exists(modPath + "\\voice_ban.dt"))
                File.Delete(modPath + "\\voice_ban.dt");
            if (Directory.Exists(modPath + "\\materialsrc"))
                Directory.Delete(modPath + "\\materialsrc", true);
            if (Directory.Exists(modPath + "\\downloadlists"))
                Directory.Delete(modPath + "\\downloadlists", true);
            if (Directory.Exists(modPath + "\\mapsrc"))
                Directory.Delete(modPath + "\\mapsrc", true);
            if (Directory.Exists(modPath + "\\save"))
                Directory.Delete(modPath + "\\save", true);
            if (Directory.Exists(modPath + "\\screenshots"))
                Directory.Delete(modPath + "\\screenshots", true);
        }
        public void deleteMod()
        {
            string modPath = GetModPath();

            if (Directory.Exists(modPath))
                Directory.Delete(modPath, true);
        }
    }
}
