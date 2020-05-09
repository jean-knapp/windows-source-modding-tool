﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace source_modding_tool
{
    public class Mod
    {
        public Game game;
        public string name;
        public string installPath;

        public Mod(Game game, string name, string installPath)
        {
            this.game = game;
            this.name = name;
            this.installPath = installPath;
        }

        public void OpenInstallFolder()
        {
            Process.Start(installPath);
        }

        public List<string> GetSearchPaths()
        {
            return GetMountedPaths().Where(x => Directory.Exists(x)).ToList();
        }

        public List<string> GetMountedPaths()
        {
            List<string> result = new List<string>();

            string gamePath = game.installPath;
            string modPath = installPath;

            SourceSDK.KeyValue gameInfo = SourceSDK.KeyValue.readChunkfile(modPath + "\\gameinfo.txt");

            SourceSDK.KeyValue searchPaths = gameInfo.findChildByKey("searchpaths");
            foreach (SourceSDK.KeyValue searchPath in searchPaths.getChildren())
            {
                string[] keys = searchPath.getKey().Split('+');

                if (!keys.Contains("game"))
                    continue;

                string value = searchPath.getValue();
                value = value.Replace("/", "\\");
                value = value.Replace("|all_source_engine_paths|", gamePath + "\\");
                value = value.Replace("|gameinfo_path|", modPath + "\\");
                value = value.Replace("\\\\", "\\");
                if (value.EndsWith("/"))
                    value = value.Substring(0, value.Length - 1);

                result.Add(value);
            }

            return result.Distinct().ToList();
        }

        public List<string> GetMountedVPKs() {
            return GetMountedPaths().Where(x => x.EndsWith(".vpk")).ToList();
        }

        public void CleanFolder()
        {
            //string modPath = sourceSDK.GetModPath(toolsGames.EditValue.ToString(), toolsMods.EditValue.ToString());
            string modPath = installPath;

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
            if (Directory.Exists(modPath + "\\cfg"))
                foreach (string file in Directory.GetFiles(modPath + "\\cfg"))
                {
                    if (new FileInfo(file).Name.StartsWith("user_") && new FileInfo(file).Name != "user_keys_default.vcfg")
                        File.Delete(file);
                }
        }

        public List<string> GetFGDs()
        {
            List<string> result = new List<string>();

            result.AddRange(Directory.GetFiles(game.installPath, "*.fgd", SearchOption.AllDirectories));
            result.AddRange(Directory.GetFiles(installPath, "*.fgd", SearchOption.AllDirectories));

            List<string> alreadyIncluded = new List<string>();

            foreach (string fgd in result)
            {
                string line;
                StreamReader file = new StreamReader(fgd);
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Trim().StartsWith("@include"))
                    {
                        line = line.Replace("@include", string.Empty).Replace("\"", string.Empty).Trim();
                        alreadyIncluded.Add(new FileInfo(fgd).Directory.FullName + "\\" + line);
                        alreadyIncluded.Add(game.installPath + "\\bin\\" + line);
                    }
                }
            }

            foreach (string fgd in alreadyIncluded.Distinct().ToList())
            {
                result.Remove(fgd);
            }

            return result;
        }
    }
}
