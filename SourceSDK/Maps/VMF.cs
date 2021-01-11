﻿using SourceSDK.Materials;
using SourceSDK.Models;
using SourceSDK.Particles;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SourceSDK.Maps
{
    public class VMF
    {
        /// <summary>
        /// Returns a list of the relative paths of the assets used by this asset, including the bsp
        /// </summary>
        /// <param name="fullPath">Full path to the asset</param>
        /// <param name="game">The base game name (i.e. Source SDK Base 2013 Singleplayer)</param>
        /// <param name="mod">The mod and folder name, in the following format: Mod Title (mod_folder)</param>
        /// <param name="launcher">An instance of the Source SDK lib</param>
        /// <returns></returns>
        public static List<string> GetAssets(string fullPath, Game game, Mod mod, Launcher launcher)
        {
            if (string.IsNullOrEmpty(fullPath) ||
                game == null ||
                mod == null ||
                launcher == null)
                return null;

            List<string> assets = new List<string>();

            KeyValue map = KeyValue.readChunkfile(fullPath, false);

            // Add maps assets
            string mapName = Path.GetFileNameWithoutExtension(fullPath).ToLower();
            assets.Add("maps/" + mapName + ".bsp");

            // Add material assets
            List<KeyValue> materials = map.findChildrenByKey("material");
            foreach (KeyValue kv in materials)
            {
                string value = "materials/" + kv.getValue().ToLower() + ".vmt";
                if (!assets.Contains(value))
                {
                    assets.Add(value);
                    assets.AddRange(VMT.GetAssets(value, game, mod, launcher));
                }
            }

            // Add model and sprite assets
            List<KeyValue> models = map.findChildrenByKey("model");
            foreach (KeyValue kv in models)
            {
                string value = kv.getValue().ToLower();
                if (!assets.Contains(value))
                {
                    assets.Add(kv.getValue().ToLower());
                    assets.AddRange(MDL.GetAssets(value, game, mod, launcher));
                }
            }

            // Add sound assets
            List<KeyValue> sounds = map.findChildrenByKey("message");
            foreach (KeyValue kv in sounds)
            {
                string value = kv.getValue().ToLower();
                if ((value.EndsWith(".wav") || value.EndsWith(".mp3")) && !assets.Contains(value))
                {
                    string v = kv.getValue().ToLower();
                    if (v.StartsWith("#"))
                        v = v.Substring(1);
                    assets.Add("sound/" + v);
                }
            }

            // Add particle assets
            List<KeyValue> effectsKVs = map.findChildrenByKey("effect_name");
            List<string> effects = new List<string>();
            List<string> pcfFiles = PCF.GetAllFiles(launcher);
            foreach (KeyValue kv in effectsKVs)
                effects.Add(kv.getValue());
            effects = effects.Distinct().ToList();
            foreach (string pcf in pcfFiles)
                if (PCF.ContainsEffect(effects, pcf))
                {
                    string asset = pcf.Substring(pcf.IndexOf("\\particles\\") + 1).Replace("\\", "/");
                    assets.Add(asset);
                    assets.AddRange(PCF.GetAssets(asset, game, mod, launcher));
                }

            // Add skybox
            KeyValue skybox = map.findChildByKey("skyname");
            if (skybox != null)
            {
                string value = skybox.getValue().ToLower();

                string[] parts = new string[] { "up", "dn", "lf", "rt", "ft", "bk" };
                foreach (string part in parts)
                {
                    assets.Add("materials/skybox/" + value + part + ".vmt");
                    VMT.GetAssets("materials/skybox/" + value + part + ".vmt", game, mod, launcher);
                }
            }

            assets = assets.Distinct().ToList();

            return assets;
        }
    }
}