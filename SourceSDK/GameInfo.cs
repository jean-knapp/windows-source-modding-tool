﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceSDK
{
    public class GameInfo
    {
        Launcher launcher;

        KeyValue root;

        public GameInfo(Launcher launcher)
        {
            this.launcher = launcher;

            switch (launcher.GetCurrentGame().engine)
            {
                case Engine.SOURCE:
                    root = KeyValue.readChunkfile(launcher.GetCurrentMod().installPath + "\\gameinfo.txt");
                    break;
                case Engine.SOURCE2:
                    root = KeyValue.readChunkfile(launcher.GetCurrentMod().installPath + "\\gameinfo.gi");
                    break;
                case Engine.GOLDSRC:
                    root = KeyValue.readChunkfile(launcher.GetCurrentMod().installPath + "\\liblist.gam");
                    break;
            }

        }

        public string getValue(string key)
        {
            return root.findChildByKey(key).getValue();
        }

        public KeyValue getRoot()
        {
            return root;
        }
    }
}
