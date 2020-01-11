﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SourceModdingTool
{
    public class VPKManager
    {
        private Steam sourceSDK;
        public Dictionary<string, VPK> vpks;

        public VPKManager(Steam sourceSDK)
        {
            this.sourceSDK = sourceSDK;
            Reload();
        }

        public void extractFile(string filePath)
        {
            foreach(KeyValuePair<string, VPK> vpk in vpks)
            {
                if(vpk.Value.files.ContainsKey(filePath))
                {
                    vpk.Value.extractFile(filePath);


                    return;
                }
            }
        }

        public List<VPK.File> getAllFiles()
        {
            List<VPK.File> files = new List<VPK.File>();
            foreach(VPK vpk in vpks.Values)
                files.AddRange(vpk.files.Values);
            files = files
                .GroupBy(x => x.path)
                .Select(y => y.First())
                .OrderBy(x => x.path)
                .ToList();

            return files;
        }

        public string getExtractedPath(string filePath)
        {
            foreach(string searchPath in sourceSDK.getModSearchPaths())
                if(File.Exists(searchPath + "\\" + filePath))
                    return searchPath + "\\" + filePath;

            return string.Empty;
        }

        public void Reload()
        {
            vpks = new Dictionary<string, VPK>();

            foreach(string searchPath in sourceSDK.getModMountedPaths())
            {
                if(searchPath.EndsWith(".vpk"))
                    vpks.Add(searchPath, new VPK(searchPath, sourceSDK));
                else
                    vpks.Add(searchPath, new MountedFolder(searchPath, sourceSDK));
            }
        }
    }
}
