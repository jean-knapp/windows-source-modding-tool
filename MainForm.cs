﻿using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using source_modding_tool.SourceSDK;
using source_modding_tool.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace source_modding_tool
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        Instance instance = null;
        Launcher launcher;

        public MainForm() { InitializeComponent(); }

        private void Form_Load(object sender, EventArgs e)
        {
            Text = Application.ProductName;
            string currentGame = Properties.Settings.Default.currentGame;
            string currentMod = Properties.Settings.Default.currentMod;

            launcher = new Launcher();
            updateToolsGames();

            if (repositoryGamesCombo.Items.Contains(currentGame))
                toolsGames.EditValue = currentGame;
            else
                toolsGames.EditValue = "";

            if (repositoryModsCombo.Items.Contains(currentMod))
                toolsMods.EditValue = currentMod;
            else
                toolsMods.EditValue = "";

            UpdateMenus();
        }

        private void menuChoreography_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Faceposer
            if (e.Item == menuChoreographyFaceposer)
            {
                string gamePath = launcher.GetGamesList()[(toolsGames.EditValue.ToString())].installPath;

                string toolPath = gamePath + "\\bin\\hlfaceposer.exe";
                Process.Start(toolPath);
            }
        }

        private void menuFile_ItemClick(object sender, ItemClickEventArgs e)
        {
            // New
            if (e.Item.Name == menuFileNew.Name)
            {
                NewModForm form = new NewModForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string folder = form.modFolder;
                    string title = form.modFolder;
                    string gameName = form.gameName;
                    string gameBranch = form.gameBranch;

                    Game game = launcher.GetGamesList()[gameName];

                    string modName = title + " (" + folder + ")";

                    launcher.SetCurrentGame(game);
                    toolsGames.EditValue = game.name;

                    updateToolsMods();

                    Mod mod = launcher.GetCurrentGame().GetModsList(launcher)[modName];
                    launcher.GetCurrentGame().SetCurrentMod(mod);
                    toolsMods.EditValue = mod.name;
                }
            }

            // Exit
            else if (e.Item.Name == menuFileExit.Name)
            {
                Close();
            }

            // Libraries
            else if (e.Item.Name == menuFileLibraries.Name)
            {
                LibrariesForm form = new LibrariesForm(launcher);
                form.ShowDialog();

                updateToolsGames();
            }
        }

        private void menuLevelDesign_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Run Map
            if (e.Item == menuLevelDesignRunMap)
            {
                RunMapForm form = new RunMapForm(launcher);
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string fileName = form.fileName;
                    if (fileName.EndsWith(".bsp") && File.Exists(fileName))
                    {
                        string mapName = Map.GetMapNameWithoutVersion(Path.GetFileNameWithoutExtension(fileName));
                        File.Copy(fileName, launcher.GetCurrentMod().installPath + "\\maps\\" + mapName + ".bsp", true);

                        if (instance != null)
                        {
                            instance.Command("+map " + mapName);
                        } else
                        {
                            instance = new Instance(launcher, panel1);
                            instance.Start("+map " + mapName);

                            FormBorderStyle = FormBorderStyle.Fixed3D;
                            MaximizeBox = false;
                            modStarted();
                        }
                    }
                }
            }
            // Hammer
            if (e.Item == menuLevelDesignHammer)
            {
                Hammer.RunHammer(launcher.GetCurrentMod());
            }

            // Fog Previewer
            else if (e.Item == menuLevelDesignFogPreviewer)
            {
                FogForm form = new FogForm(launcher);
                form.ShowDialog();
            }

            // Prefabs
            else if (e.Item == menuLevelDesignPrefabs)
            {
                string gamePath = launcher.GetCurrentGame().installPath;
                Process.Start(gamePath + "\\bin\\Prefabs");
            }

            // Mapsrc
            else if (e.Item == menuLevelDesignMapsrc)
            {
                // TODO implement this
            }

            // Crafty
            else if (e.Item == menuLevelDesignCrafty)
            {
                Process.Start("Tools\\Crafty\\Crafty.exe");
            }

            // Terrain generator
            else if (e.Item == menuLevelDesignTerrainGenerator)
            {
                Process.Start("Tools\\TerrainGenerator\\TerrainGenerator.exe");
            }

            // Batch compiler
            else if (e.Item == menuLevelDesignBatchCompiler)
            {
                Process.Start("Tools\\BatchCompiler\\Batch Compiler.exe");
            }
        }

        private void menuMaterials_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Material editor
            if (e.Item == menuMaterialsEditor)
            {
                MaterialEditor form = new MaterialEditor(string.Empty, launcher);
                form.ShowDialog();
            }
        }

        private void menuModding_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Open folder
            if (e.Item == menuModdingOpenFolder)
            {
                launcher.GetCurrentMod().OpenInstallFolder();
            }

            // Clean
            else if (e.Item == menuModdingClean)
            {
                launcher.GetCurrentMod().CleanFolder();
            }

            // Import
            else if (e.Item == menuModdingImport2)
            {
                ModSelectionDialog dialog = new ModSelectionDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string gameName = dialog.game;
                    string modName = dialog.mod;

                    Game game = launcher.GetGamesList()[gameName];
                    Mod mod = launcher.GetModsList(game)[modName];

                    AssetsCopierForm assetsCopierForm = new AssetsCopierForm(game, mod);
                    if (assetsCopierForm.ShowDialog() == DialogResult.OK)
                    {
                    }
                }
            }

            // File explorer
            else if (e.Item == menuModdingFileExplorer)
            {
                FileExplorer form = new FileExplorer(launcher);
                form.ShowDialog();
            }

            // Export
            else if (e.Item == menuModdingExport)
            {
                Game game = launcher.GetGamesList()[toolsGames.EditValue.ToString()];
                Mod mod = launcher.GetModsList(game)[toolsMods.EditValue.ToString()];

                AssetsCopierForm form = new AssetsCopierForm(game, mod);
                form.ShowDialog();
            }

            // Hud Editor
            else if (e.Item == menuModdingHudEditor)
            {
                HudEditorForm form = new HudEditorForm(launcher);
                form.ShowDialog();
            }

            // Delete mod
            else if(e.Item == menuModdingDelete)
            {
                if (XtraMessageBox.Show("Are you sure you want to delete this mod?", "Delete mod", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    launcher.GetCurrentGame().DeleteMod();
                    updateToolsGames();
                    updateToolsMods();
                }
            }
        }

        private void menuModdingRun_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Run
            if (e.Item == menuModdingRun || e.Item == toolsRun || e.Item == toolsRunPopupRun)
            {
                instance = new Instance(launcher, panel1);
                switch(launcher.GetCurrentGame().engine)
                {
                    case Engine.SOURCE:
                        instance.StartFullScreen();
                        break;
                    case Engine.SOURCE2:
                        instance.StartVR();
                        break;
                }
                

                FormBorderStyle = FormBorderStyle.Fixed3D;
                MaximizeBox = false;
                modStarted();
            }

            // Run Windowed
            else if (e.Item == menuModdingRunFullscreen || e.Item == toolsRunPopupRunFullscreen)
            {
                instance = new Instance(launcher, panel1);
                instance.Start();

                modStarted();
            }

            // Ingame tools
            else if (e.Item == menuModdingIngameTools || e.Item == toolsRunPopupIngameTools)
            {
                instance = new Instance(launcher, panel1);
                instance.StartTools();
                instance.modProcess.Exited += new EventHandler(modExited);

                FormBorderStyle = FormBorderStyle.Fixed3D;
                MaximizeBox = false;
                modStarted();
            }
        }

        private void menuModdingSettings_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Game info
            if(e.Item == menuModdingSettingsGameInfo)
            {
                GameinfoForm form = new GameinfoForm(launcher);
                form.ShowDialog();
                updateToolsGames();
                updateToolsMods();
            }

            // Chapters
            else if(e.Item == menuModdingSettingsChapters)
            {
                ChaptersForm form = new ChaptersForm(launcher);
                form.ShowDialog();
            }

            // Menu
            else if(e.Item == menuModdingSettingsMenu)
            {
                GamemenuForm form = new GamemenuForm(launcher);
                form.ShowDialog();
            }

            // Content Monut
            else if (e.Item == menuModdingSettingsContentMount)
            {
                SearchPathsForm form = new SearchPathsForm(launcher);
                form.ShowDialog();
            }
        }

        private void menuModeling_ItemClick(object sender, ItemClickEventArgs e)
        {
            // HLMV
            if(e.Item == menuModelingHLMV)
            {
                string gamePath = launcher.GetGamesList()[toolsGames.EditValue.ToString()].installPath;

                string toolPath = gamePath + "\\bin\\hlmv.exe";
                Process.Start(toolPath);
            }

            // Propper
            else if(e.Item == menuModelingPropper)
            {
                Hammer.RunPropperHammer(launcher.GetCurrentMod());
            }

            // VMF to MDL
            else if(e.Item == menuModelingVMFtoMDL)
            {
                VMFtoMDL form = new VMFtoMDL(launcher);
                form.ShowDialog();
            }

            // Crowbar
            else if(e.Item == menuModelingCrowbar)
            {
                Process.Start("Tools\\Crowbar\\Crowbar.exe");
            }
        }

        private void menuParticles_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Manifest generator
            if(e.Item == menuParticlesManifestGenerator)
            {
                PCF.CreateManifest(launcher);
                XtraMessageBox.Show("Particle manifest generated.");
            }
        }

        private void modStarted()
        {
            instance.modProcess.Exited += new EventHandler(modExited);

            modProcessUpdater.Enabled = true;
            toolsRun.Enabled = false;
            menuModdingRun.Enabled = false;
            menuModdingRunFullscreen.Enabled = false;
            menuModdingIngameTools.Enabled = false;
            menuModdingDelete.Enabled = false;
            toolsStop.Visibility = BarItemVisibility.Always;
        }

        private void modExited(object sender, EventArgs e) {
            if (instance != null)
                instance.modProcess = null;
        }

        private void ModForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(instance != null)
                instance.Stop();
        }

        private void ModForm_ResizeEnd(object sender, EventArgs e)
        {
            if(instance != null)
                instance.Resize();
        }

        private void modProcessUpdater_Tick(object sender, EventArgs e)
        {
            if(instance == null || instance.modProcess == null)
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                modProcessUpdater.Enabled = false;
                MaximizeBox = true;

                toolsRun.Enabled = true;
                menuModdingRun.Enabled = true;
                menuModdingRunFullscreen.Enabled = true;
                menuModdingIngameTools.Enabled = true;
                menuModdingDelete.Enabled = true;
                toolsStop.Visibility = BarItemVisibility.Never;
            }
        }

        private void toolsGames_EditValueChanged(object sender, EventArgs e)
        {
            if(toolsGames.EditValue == null || toolsGames.EditValue.ToString() == string.Empty)
            {
                XtraMessageBox.Show("No Source game was selected. Please, try again.");
                return;
            }

            string gameName = toolsGames.EditValue.ToString();
            Game game = launcher.GetGamesList()[gameName];

            launcher.SetCurrentGame(game);
            updateToolsMods();
            Properties.Settings.Default.currentGame = game.name;
            Properties.Settings.Default.Save();
        }

        private void toolsMods_EditValueChanged(object sender, EventArgs e)
        {
            if (launcher == null)
                return;

            Game currentGame = launcher.GetCurrentGame();

            if (launcher.GetModsList(currentGame).ContainsKey(toolsMods.EditValue.ToString()))
            {
                Mod mod = launcher.GetModsList(currentGame)[toolsMods.EditValue.ToString()];

                launcher.GetCurrentGame().SetCurrentMod(mod);
                Properties.Settings.Default.currentMod = toolsMods.EditValue.ToString();
                Properties.Settings.Default.Save();

                UpdateMenus();
            }
        }

        private void UpdateMenus()
        {
            switch(launcher.GetCurrentGame().engine)
            {
                case Engine.SOURCE:
                    toolsRun.Enabled = (toolsMods.EditValue != null && toolsMods.EditValue.ToString() != string.Empty);
                        toolsRunPopupIngameTools.Enabled = true;
                        toolsRunPopupRun.Enabled = true;
                        toolsRunPopupRunFullscreen.Enabled = true;
                    menuModding.Enabled = (toolsMods.EditValue != null && toolsMods.EditValue.ToString() != string.Empty);
                        menuModdingRunFullscreen.Enabled = true;
                        menuModdingIngameTools.Enabled = true;
                        menuModdingClean.Enabled = true;
                        menuModdingImport2.Enabled = true;
                        menuModdingSettings.Enabled = true;
                        menuModdingHudEditor.Enabled = true;
                        menuModdingFileExplorer.Enabled = true;
                        menuModdingExport.Enabled = true;
                    menuLevelDesign.Enabled = (toolsMods.EditValue != null && toolsMods.EditValue.ToString() != string.Empty);
                    menuModeling.Enabled = (toolsMods.EditValue != null && toolsMods.EditValue.ToString() != string.Empty);
                    menuMaterials.Enabled = (toolsMods.EditValue != null && toolsMods.EditValue.ToString() != string.Empty);
                    menuParticles.Enabled = (toolsMods.EditValue != null && toolsMods.EditValue.ToString() != string.Empty);
                    menuChoreographyFaceposer.Enabled = (toolsMods.EditValue != null && toolsMods.EditValue.ToString() != string.Empty);

                    break;
                case Engine.SOURCE2:
                    toolsRun.Enabled = (toolsMods.EditValue != null && toolsMods.EditValue.ToString() != string.Empty);
                        toolsRunPopupIngameTools.Enabled = false;
                        toolsRunPopupRun.Enabled = true;
                        toolsRunPopupRunFullscreen.Enabled = true;
                    menuModding.Enabled = (toolsMods.EditValue != null && toolsMods.EditValue.ToString() != string.Empty);
                        menuModdingRunFullscreen.Enabled = true;
                        menuModdingIngameTools.Enabled = false;
                        menuModdingClean.Enabled = false;
                        menuModdingImport2.Enabled = false;
                        menuModdingSettings.Enabled = false;
                        menuModdingHudEditor.Enabled = false;
                        menuModdingFileExplorer.Enabled = false;
                        menuModdingExport.Enabled = false;


                    menuLevelDesign.Enabled = false;
                    menuModeling.Enabled = false;
                    menuMaterials.Enabled = false;
                    menuParticles.Enabled = false;
                    menuChoreographyFaceposer.Enabled = false;

                    break;
                case Engine.GOLDSRC:
                    toolsRun.Enabled = false;
                    menuModding.Enabled = false;
                    menuLevelDesign.Enabled = false;
                    menuModeling.Enabled = false;
                    menuMaterials.Enabled = false;
                    menuParticles.Enabled = false;
                    menuChoreographyFaceposer.Enabled = false;

                    break;
            }

        }

        private void toolsStop_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (instance.modProcess != null)
            {
                instance.Stop();
                instance = null;
            }
        }

        private void updateToolsGames()
        {
            if (launcher == null)
                return;

            string currentGame = (toolsGames.EditValue != null ? toolsGames.EditValue.ToString() : string.Empty);
            repositoryGamesCombo.Items.Clear();
            Dictionary<string, Game> gamesList = launcher.GetGamesList();

            if (gamesList.Count > 0)
                foreach (KeyValuePair<string, Game> item in gamesList)
                    repositoryGamesCombo.Items.Add(item.Key);

            if(repositoryGamesCombo.Items.Count > 0 && repositoryGamesCombo.Items.Contains(currentGame))
                toolsGames.EditValue = currentGame;
            else if(repositoryGamesCombo.Items.Count > 0)
                toolsGames.EditValue = repositoryGamesCombo.Items[0];
            else
            {
                toolsGames.EditValue = string.Empty;
            }
        }

        private void updateToolsMods()
        {
            string currentMod = (toolsMods.EditValue != null ? toolsMods.EditValue.ToString() : string.Empty);
            repositoryModsCombo.Items.Clear();

            Game currentGame = (toolsGames.EditValue != null ? launcher.GetGamesList()[toolsGames.EditValue.ToString()] : null);
            if(currentGame == null)
                return;

            Dictionary<string, Mod> modsList = launcher.GetModsList(currentGame);
            if (modsList.Count > 0)
                foreach (KeyValuePair<string, Mod> item in modsList)
                    repositoryModsCombo.Items.Add(item.Key);

            if (repositoryModsCombo.Items.Count > 0 && repositoryModsCombo.Items.Contains(currentMod))
                toolsMods.EditValue = currentMod;
            else if(repositoryModsCombo.Items.Count > 0)
                toolsMods.EditValue = repositoryModsCombo.Items[0];
            else
            {
                toolsMods.EditValue = string.Empty;
            }
        }

        private void tools_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.Item.Tag)
            {
                case "reattach":
                    if (instance != null)
                        instance.AttachProcessTo(instance.modProcess, panel1);
                    break;
            }
        }
    }
}