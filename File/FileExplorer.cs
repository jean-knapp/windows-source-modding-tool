﻿using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using source_modding_tool.Sound;
using SourceSDK;
using SourceSDK.Packages;
using SourceSDK.Packages.UnpackedPackage;
using SourceSDK.Packages.VPKPackage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace source_modding_tool.Modding
{
    public partial class FileExplorer : DevExpress.XtraEditors.XtraForm
    {
        public string CurrentDirectory = null;
        public string RootDirectory = string.Empty;
        public bool MultiSelect = false;
        public string Filter = "";

        string keywords = string.Empty;
        Stack<string> nextDirectories = new Stack<string>();
        Stack<string> previousDirectories = new Stack<string>();

        public PackageManager packageManager;
        List<PackageDirectory> directories;

        Launcher launcher;
        private Mode mode = Mode.BROWSE;

        public PackageFile[] Selection { get; internal set; }

        public enum Mode
        {
            BROWSE = 0,
            OPEN = 1,
            SAVE = 2
        };

        public FileExplorer(Launcher launcher)
        {
            InitializeComponent();
            this.launcher = launcher;
        }

        public FileExplorer(Launcher launcher, Mode mode) : this(launcher)
        {
            this.mode = mode;
            switch(mode)
            {
                case Mode.OPEN:
                    {
                        openFileDialogPanel.Visible = true;
                        statusBar.Visible = false;
                        okButton.Text = "Open";
                    }
                    break;
                case Mode.SAVE:
                    {
                        openFileDialogPanel.Visible = true;
                        statusBar.Visible = false;
                        okButton.Text = "Save";
                    }
                    break;
                default:
                    {

                    }
                    break;
            }
        }

        private void NewFileExplorer_Load(object sender, EventArgs e)
        {
            if (CurrentDirectory == null)
                this.CurrentDirectory = RootDirectory;

            if (packageManager == null)
                packageManager = new PackageManager(launcher, RootDirectory);

            UpdateDirectoryTree();
            UpdateFileTree(RootDirectory);
        }

        private void UpdateDirectoryTree()
        {
            directories = packageManager.Directories;

            directoryTree.BeginUnboundLoad();

            directoryTree.Nodes.Clear();

            Dictionary<string, TreeListNode> folders = new Dictionary<string, TreeListNode>();

            foreach (PackageDirectory directory in directories)
            {
                string[] pathArray = null;

                // Root
                if (directory.Path == " ")
                    directory.Path = "";

                if (!folders.ContainsKey(directory.Path)) {
                    AddDirectoryToTree(folders, directory.Path, RootDirectory);
                }
            }

            directoryTree.EndUnboundLoad();

            directoryTree.ExpandToLevel(0);
        }

        private void AddDirectoryToTree(Dictionary<string, TreeListNode> folders, string directoryPath, string rootPath)
        {
            if (!folders.ContainsKey(directoryPath))
            {
                // Root
                if (directoryPath == rootPath)
                {
                    TreeListNode node = directoryTree.AppendNode(new object[] { (rootPath != "" ? Path.GetFileName(rootPath) : "root") }, null);
                    node.Tag = directoryPath;
                    node.StateImageIndex = 0;
                    folders.Add(directoryPath, node);
                }

                else if(directoryPath != "")
                {
                    string parentPath = Path.GetDirectoryName(directoryPath).Replace("\\", "/");

                    if (!folders.ContainsKey(parentPath))
                    {
                        AddDirectoryToTree(folders, parentPath, rootPath);
                    }

                    TreeListNode node = directoryTree.AppendNode(new object[] { Path.GetFileName(directoryPath) }, folders[parentPath]);
                    node.StateImageIndex = 0;
                    node.Tag = directoryPath;
                    folders.Add(directoryPath, node);
                }

            }
        }

        private void UpdateFileTree(string directoryPath)
        {
            CurrentDirectory = directoryPath;
            textDirectory.EditValue = CurrentDirectory;
            buttonUp.Enabled = (CurrentDirectory != string.Empty);
            buttonBack.Enabled = (previousDirectories.Count > 0);
            buttonForward.Enabled = (nextDirectories.Count > 0);

            textSearch.EditValue = "";

            List<PackageFile> files = new List<PackageFile>();

            foreach (PackageDirectory directory in directories.Where(d => d.Path == directoryPath))
                files.AddRange(directory.Entries);

            files = files.OrderBy(f => f.Filename).ToList();

            if (Filter != string.Empty)
            {
                List<string> types = Filter.Split('|').ToList();

                for (int i = 0; i < types.Count; i++)
                    types.RemoveAt(i);

                List<PackageFile> filteredFiles = new List<PackageFile>();
                foreach(PackageFile packageFile in files)
                {
                    foreach(string type in types)
                    {
                        string[] filters = type.Split('*');

                        string str = (packageFile.Filename + "." + packageFile.Extension);

                        if ((packageFile.Filename + "." + packageFile.Extension).StartsWith(filters.First()) && (packageFile.Filename + "." + packageFile.Extension).EndsWith(filters.Last())) {
                            filteredFiles.Add(packageFile);
                            break;
                        }
                    }
                }
                files = filteredFiles;
            }

            fileTree.BeginUnboundLoad();
            fileTree.Nodes.Clear();

            foreach (string directory in directories.Select(d => d.Path).Where(d => d.StartsWith(directoryPath.Length > 0 ? directoryPath + "/" : "") && d.Remove(0, (directoryPath.Length > 0 ? directoryPath + "/" : "").Length).Split('/').Length == 1).Distinct())
            {
                if (directory == directoryPath)
                    continue;

                string folderName = directory.Remove(0, (directoryPath.Length > 0 ? directoryPath + "/" : "").Length);
                TreeListNode node = fileTree.AppendNode(new object[] { folderName, "", "" }, null);
                node.Tag = directory;
                node.StateImageIndex = 0;
            }

            foreach (PackageFile file in files.GroupBy(f => new { f.Filename, f.Extension }).Select(g => g.First()))
            {
                TreeListNode node = fileTree.AppendNode(new object[] { file.Filename, file.Extension, file.Directory.ParentArchive.Name }, null);
                node.Tag = file;
                if (file.Filename.StartsWith("soundscapes_") && file.Filename != "soundscapes_manifest" && file.Extension == ".txt")
                {
                    node.StateImageIndex = 8;
                }
                else
                {
                    switch (file.Extension)
                    {
                        case "vmt":
                            node.StateImageIndex = 5;
                            break;
                        case "vtf":
                            node.StateImageIndex = 7;
                            break;
                        case "vmf":
                            if (file.Path.StartsWith("modelsrc"))
                            {
                                node.StateImageIndex = 3;
                            }
                            else
                            {
                                node.StateImageIndex = 6;
                            }

                            break;
                        case "vmx":
                            node.StateImageIndex = 6;
                            break;
                        case "bsp":
                            node.StateImageIndex = 2;
                            break;
                        case "wav":
                            node.StateImageIndex = 8;
                            break;
                        default:
                            node.StateImageIndex = 1;
                            break;
                    }
                }
                
            }

            fileTree.EndUnboundLoad();
        }

        private void SearchFileTree(string searchString)
        {
            textDirectory.EditValue = "Searching in " + CurrentDirectory;
            buttonUp.Enabled = (CurrentDirectory != string.Empty);
            buttonBack.Enabled = (previousDirectories.Count > 0);
            buttonForward.Enabled = (nextDirectories.Count > 0);

            searchString = searchString.ToLower();

            List<PackageFile> files = new List<PackageFile>();

            foreach (PackageDirectory directory in directories.Where(d => d.Path.ToLower() == CurrentDirectory.ToLower() || d.Path.ToLower().StartsWith((CurrentDirectory != "" ? CurrentDirectory + "/" : "").ToLower())))
                files.AddRange(directory.Entries);

            files = files.OrderBy(f => f.Filename).ToList();

            fileTree.BeginUnboundLoad();
            fileTree.Nodes.Clear();

            foreach (PackageFile file in files.GroupBy(f => f.Filename).Select(g => g.First()))
            {
                // Search pattern
                if (!(file.Directory.Path + "/" + file.Filename).ToLower().Contains(searchString))
                    continue;

                TreeListNode node = fileTree.AppendNode(new object[] { file.Directory.Path + "/" + file.Filename, file.Extension, file.Directory.ParentArchive.Name }, null);
                node.Tag = file;
                node.StateImageIndex = 1;
            }

            fileTree.EndUnboundLoad();
        }

        private void fileTree_DoubleClick(object sender, EventArgs e)
        {
            TreeList tree = sender as TreeList;
            TreeListHitInfo hi = tree.CalcHitInfo(tree.PointToClient(Control.MousePosition));

            if (hi.Node != null)
            {
                if (hi.Node.Tag is string)
                {
                    // It's a folder
                    string tag = hi.Node.Tag.ToString();
                    if (tag != CurrentDirectory)
                    {
                        previousDirectories.Push(CurrentDirectory);
                        nextDirectories.Clear();
                    }

                    UpdateFileTree(tag);
                }
                else if (hi.Node.Tag is PackageFile && mode == Mode.BROWSE)
                {
                    // It's a file;
                    OpenFile(hi.Node.Tag as PackageFile);
                } else
                {
                    // Unknown listing type.
                }
            }
        }

        private void OpenFile(PackageFile packageFile)
        {
            if (packageFile.Extension == "vmt")
            {
                // It's a material file.
                MaterialEditor materialEditor = new MaterialEditor(launcher, packageFile);
                materialEditor.ShowDialog();
            }
            else if(packageFile.Extension == "vmf" || packageFile.Extension == "vmx")
            {
                if (packageFile.Path.StartsWith("modelsrc"))
                {
                    // It's a map file.
                    Hammer.RunPropperHammer(launcher.GetCurrentMod(), packageFile);
                } else
                {
                    // It's a map file.
                    Hammer.RunHammer(launcher, null, null, packageFile);
                }

            }
            else if (packageFile.Filename.StartsWith("soundscapes_") && packageFile.Filename != "soundscapes_manifest" && packageFile.Extension == "txt")
            {
                // It's a soundscape.
                SoundscapeEditor soundscapeEditor = new SoundscapeEditor(launcher, packageFile);
                soundscapeEditor.ShowDialog();
            }
            else if(new string[] { "txt", "gi" }.Contains(packageFile.Extension))
            {
                // TODO temp method. This will only work for unpacked files.
                if (packageFile is UnpackedFile)
                    Process.Start("NOTEPAD", packageFile.Directory.ParentArchive.ArchivePath + "\\" + packageFile.Path + "\\" + packageFile.Filename + "." + packageFile.Extension);
            }
            else if(packageFile.Extension == "wav")
            {
                using (Stream s = new MemoryStream(packageFile.Data))
                {
                    System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(s);
                    myPlayer.Play();
                }
            }
        }

        private void directoryTree_Click(object sender, EventArgs e)
        {
            TreeList tree = sender as TreeList;
            TreeListHitInfo hi = tree.CalcHitInfo(tree.PointToClient(Control.MousePosition));
            if (hi.Node != null)
            {
                // It's a folder
                string tag = hi.Node.Tag.ToString();
                if (tag != CurrentDirectory)
                {
                    previousDirectories.Push(CurrentDirectory);
                    nextDirectories.Clear();
                }

                UpdateFileTree(tag);
            }
        }

        private void repositoryTextSearch_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void textSearch_EditValueChanged(object sender, EventArgs e)
        {
            string searchString = textSearch.EditValue.ToString();
            if (searchString != "")
                SearchFileTree(searchString);
            else
                UpdateFileTree(CurrentDirectory);
        }

        private void fileTree_SelectionChanged(object sender, EventArgs e)
        {
            List<PackageFile> result = new List<PackageFile>();
            foreach(TreeListNode node in fileTree.Selection)
            {
                if (node.Tag is PackageFile)
                {
                    result.Add(node.Tag as PackageFile);
                }
            }

            Selection = result.ToArray();
            okButton.Enabled = (Selection.Length > 0);

            if (result.Count == 1 && result[0].Extension == "wav")
            {
                previewButton.Enabled = true;
            } else
            {
                previewButton.Enabled = false;
            }

            fileNameEdit.EditValue = string.Join(", ", Selection.Select(p => p.Filename).ToArray());
        }

        private void navigation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item == buttonBack && previousDirectories.Count > 0)
            {
                nextDirectories.Push(CurrentDirectory);
                UpdateFileTree(previousDirectories.Pop());
            }
            if (e.Item == buttonForward && nextDirectories.Count > 0)
            {
                previousDirectories.Push(CurrentDirectory);
                UpdateFileTree(nextDirectories.Pop());
            }
            if (e.Item == buttonUp && CurrentDirectory != string.Empty)
            {
                previousDirectories.Push(CurrentDirectory);
                nextDirectories.Clear();

                if (CurrentDirectory.Contains("/"))
                    CurrentDirectory = CurrentDirectory.Substring(0, CurrentDirectory.LastIndexOf("/"));

                if (CurrentDirectory.Contains("/"))
                    CurrentDirectory = CurrentDirectory.Substring(0, CurrentDirectory.LastIndexOf("/") + 1);
                else
                    CurrentDirectory = string.Empty;

                UpdateFileTree(CurrentDirectory);
            }
        }

        private void previewButton_Click(object sender, EventArgs e)
        {
            PackageFile file = Selection[0];

            if (file.Extension == "wav")
            {
                using (Stream s = new MemoryStream(file.Data))
                {
                    System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(s);
                    myPlayer.Play();
                }
            }
        }
    }
}
