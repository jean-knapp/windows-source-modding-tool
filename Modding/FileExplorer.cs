﻿using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace source_modding_tool.Tools
{
    public partial class FileExplorer : DevExpress.XtraEditors.XtraForm
    {
        string currentDirectory = string.Empty;
        string filter = string.Empty;
        Stack<string> nextDirectories = new Stack<string>();
        Stack<string> previousDirectories = new Stack<string>();

        List<VPK.File> files = new List<VPK.File>();

        Launcher launcher;
        VPKManager vpkManager;

        public FileExplorer(Launcher launcher)
        {
            InitializeComponent();

            this.launcher = launcher;
            vpkManager = new VPKManager(launcher);
            ListFiles();
            traverseFileTree();
            TraverseDirectory(currentDirectory);
        }

        private void navigation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item == buttonBack && previousDirectories.Count > 0)
            {
                nextDirectories.Push(currentDirectory);
                TraverseDirectory(previousDirectories.Pop());
            }
            if (e.Item == buttonForward && nextDirectories.Count > 0)
            {
                previousDirectories.Push(currentDirectory);
                TraverseDirectory(nextDirectories.Pop());
            }
            if (e.Item == buttonUp && currentDirectory != string.Empty)
            {
                previousDirectories.Push(currentDirectory);
                nextDirectories.Clear();

                if (currentDirectory.Contains("/"))
                    currentDirectory = currentDirectory.Substring(0, currentDirectory.LastIndexOf("/"));

                if (currentDirectory.Contains("/"))
                    currentDirectory = currentDirectory.Substring(0, currentDirectory.LastIndexOf("/") + 1);
                else
                    currentDirectory = string.Empty;

                TraverseDirectory(currentDirectory);
            }
        }

        private void repositoryTextSearch_EditValueChanged(object sender, EventArgs e)
        {
            filter = ((TextEdit)sender).EditValue.ToString();
            if (filter != string.Empty)
                TraverseDirectoryFiltered(currentDirectory);
            else
                TraverseDirectory(currentDirectory);
        }

        private void tree_SelectionChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if(tree.FocusedNode == null || tree.FocusedNode.Tag == null)
                return;

            string directory = tree.FocusedNode.Tag.ToString();

            if(directory != currentDirectory)
            {
                previousDirectories.Push(currentDirectory);
                nextDirectories.Clear();
            }

            TraverseDirectory(directory);
        }

        private void list_SelectionChanged(object sender, EventArgs e) { }

        private void list_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                bool hasFolders = false;
                bool fileExists = true;

                List<string> selectedPaths = GetSelectedPaths();
                foreach (string filePath in selectedPaths)
                {
                    if (filePath.EndsWith("/"))
                        // It's a directory
                        hasFolders = true;
                    else
                    {
                        // It's a file
                        fileExists = fileExists && vpkManager.getExtractedPath(filePath) != string.Empty;
                    }
                }

                EnableAvailableActions();
                filePopMenu.ShowPopup(MousePosition);
            }
        }

        private void list_DoubleClick(object sender, EventArgs e)
        {
            TreeList tree = sender as TreeList;
            TreeListHitInfo hi = tree.CalcHitInfo(tree.PointToClient(Control.MousePosition));
            if (hi.Node != null)
            {
                
                if (hi.Node.StateImageIndex == 0)
                {
                    // It's a folder
                    string tag = hi.Node.Tag.ToString();
                    if (tag != currentDirectory)
                    {
                        previousDirectories.Push(currentDirectory);
                        nextDirectories.Clear();
                    }

                    TraverseDirectory(tag);
                }
                else
                {
                    // It's a file
                    FileAction(Action.OPEN);
                }
            }
        }

        class Action
        {
            public const int OPEN = 0;
            public const int EDIT = 1;
            public const int OPEN_FILE_LOCATION = 2;
            public const int DELETE = 3;
            public const int EXTRACT = 4;
        }

        private void EnableAvailableActions()
        {
            List<string> selectedPaths = GetSelectedPaths();

            filePopOpenButton.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            filePopDeleteButton.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            filePopExtractButton.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            filePopEditButton.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            filePopOpenFileLocationButton.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            if (selectedPaths.Count > 1)
            {
                filePopOpenButton.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                filePopEditButton.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }

            foreach(string file in selectedPaths)
            {
                string extractedPath = vpkManager.getExtractedPath(file);
                if (extractedPath == string.Empty)
                {
                    filePopOpenFileLocationButton.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    filePopDeleteButton.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                } else
                {
                    filePopExtractButton.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }

                if (file.EndsWith("/"))
                {
                    filePopEditButton.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }
        }

        private void FileAction(int action)
        {
            string modPath = launcher.GetCurrentMod().installPath;

            switch (action)
            {
                case Action.OPEN:
                    {

                    }
                    break;
                case Action.EDIT:
                    {
                        foreach (string filePath in GetSelectedPaths())
                        {
                            string extractedPath = vpkManager.getExtractedPath(filePath);

                            if (extractedPath == string.Empty)
                            {
                                vpkManager.extractFile(filePath);
                                extractedPath = modPath + "\\" + filePath;
                            }
                            else if (extractedPath != modPath + "\\" + filePath)
                            {
                                File.Copy(extractedPath, modPath + "\\" + filePath, true);
                            }

                            Process.Start("notepad", extractedPath);
                        }

                        ListFiles(true);
                        TraverseDirectory(currentDirectory);
                    }
                    break;
                case Action.OPEN_FILE_LOCATION:
                    {
                        List<string> paths = GetSelectedPaths();
                        for (int i = 0; i < paths.Count; i++)
                        {
                            paths[i] = vpkManager.getExtractedPath(paths[i]).Replace("/", "\\");
                            if (paths[i].Contains("\\"))
                            {
                                paths[i] = paths[i].Substring(0, paths[i].LastIndexOf("\\") + 1);
                            }
                        }

                        paths = paths.Distinct().ToList();
                        foreach (string path in paths)
                        {
                            Process.Start(path);
                        }
                    }
                    break;
                case Action.DELETE:
                    {
                        List<string> selectedPaths = GetSelectedPaths();
                        foreach (string filePath in selectedPaths)
                        {
                            string fullPath = vpkManager.getExtractedPath(filePath);
                            if (fullPath != String.Empty)
                            {
                                File.Delete(fullPath);
                            }                            
                        }

                        ListFiles(true);
                        TraverseDirectory(currentDirectory);
                    }
                    break;
                case Action.EXTRACT:
                    {
                        //var nodes = list.Selection;
                        //List<string> values = new List<string>();
                        //foreach (TreeListNode node in nodes)
                        //{
                        //    values.Add(node.Tag.ToString());
                        //}
                        List<string> selectedPaths = GetSelectedPaths();
                        foreach (string filePath in selectedPaths)
                        {
                            vpkManager.extractFile(filePath);
                        }

                        Process.Start(modPath);

                        ListFiles(true);
                        TraverseDirectory(currentDirectory);
                    }
                    break;
            }
        }

        private void Popup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item == filePopOpenButton)
                FileAction(Action.OPEN);

            if (e.Item == filePopEditButton)
                FileAction(Action.EDIT);

            if (e.Item == filePopExtractButton)
                FileAction(Action.EXTRACT);

            if (e.Item == filePopDeleteButton)
                FileAction(Action.DELETE);

            if (e.Item == filePopOpenFileLocationButton)
                FileAction(Action.OPEN_FILE_LOCATION);
        }

        private List<string> GetSelectedPaths()
        {
            List<string> selectedPaths = new List<string>();
            foreach (TreeListNode node in list.Selection)
            {
                if (!(node.Tag is VPK.File))
                {
                    // It's a folder
                    selectedPaths.Add(node.Tag.ToString());
                }
                else
                {
                    // It's a file
                    VPK.File file = node.Tag as VPK.File;
                    selectedPaths.Add(file.path);
                }
            }
            return selectedPaths;
        }

        private void ListFiles() { ListFiles(false); }

        private void ListFiles(bool reload)
        {
            if(reload)
                vpkManager.Reload();
            files = vpkManager.getAllFiles();
        }

        private void TraverseDirectory(string directory)
        {
            currentDirectory = directory;
            buttonUp.Enabled = (currentDirectory != string.Empty);
            buttonBack.Enabled = (previousDirectories.Count > 0);
            buttonForward.Enabled = (nextDirectories.Count > 0);

            filter = string.Empty;
            textSearch.EditValue = string.Empty;

            if(directory.Contains("/"))
                repositoryTextSearch.NullValuePrompt = "Search in " +
                    directory.Substring(0, directory.Length - 1).Split('/').Last();
            else
                repositoryTextSearch.NullValuePrompt = "Search";

            textDirectory.EditValue = directory;

            list.BeginUnboundLoad();
            list.Nodes.Clear();

            List<string> usedFiles = new List<string>();

            for(int f = 0; f < files.Count; f++)
            {
                VPK.File file = files[f];
                string path = file.path;

                if(!path.StartsWith(directory))
                    continue;

                path = path.Substring(directory.Length);

                string[] fileSplit = path.Split('/');

                if(fileSplit.Length > 1)
                {
                    // It's a directory
                    if(usedFiles.Contains(fileSplit[0]))
                        continue;

                    TreeListNode node = list.AppendNode(new object[] { fileSplit[0], "Folder" }, null);
                    node.Tag = directory + fileSplit[0] + "/";
                    node.StateImageIndex = 0;
                    usedFiles.Add(fileSplit[0]);
                } else
                {
                    // It's a file
                    TreeListNode node = list.AppendNode(new object[] { fileSplit[0], file.type, file.pack }, null);
                    //node.Tag = directory + path;
                    node.Tag = file;
                    node.StateImageIndex = 1;
                    usedFiles.Add(path);
                }
            }

            list.EndUnboundLoad();
        }

        private void TraverseDirectoryFiltered(string directory)
        {
            buttonUp.Enabled = (currentDirectory != string.Empty);
            buttonBack.Enabled = (previousDirectories.Count > 0);
            buttonForward.Enabled = (nextDirectories.Count > 0);

            textDirectory.EditValue = "Search results for " + filter;

            list.BeginUnboundLoad();
            list.Nodes.Clear();

            List<VPK.File> filtered = files.Where(x => x.path.Contains(filter)).ToList();

            List<string> usedFiles = new List<string>();

            for(int f = 0; f < filtered.Count; f++)
            {
                VPK.File file = filtered[f];
                string path = file.path;

                if(!path.StartsWith(directory))
                    continue;

                string[] fileSplit = path.Split('/');

                string dir = string.Empty;
                for(int j = 0; j < fileSplit.Length; j++)
                {
                    dir = dir + fileSplit[j] + "/";

                    if(!fileSplit[j].Contains(filter))
                        continue;

                    if(j < fileSplit.Length - 1)
                    {
                        // It's a directory
                        if(usedFiles.Contains(dir))
                            continue;

                        TreeListNode node = list.AppendNode(new object[] { path, "Folder" }, null);
                        node.Tag = dir;
                        node.StateImageIndex = 0;
                        usedFiles.Add(dir);
                    } else
                    {
                        // It's a file
                        TreeListNode node = list.AppendNode(new object[] { path, file.type, file.pack }, null);
                        node.Tag = file;
                        node.StateImageIndex = 1;
                        usedFiles.Add(path);
                    }
                }
            }

            list.EndUnboundLoad();
        }

        private void traverseFileTree()
        {
            tree.BeginUnboundLoad();
            tree.Nodes.Clear();

            Stack<TreeListNode> stack = new Stack<TreeListNode>();
            Stack<string> stackString = new Stack<string>();

            stack.Push(tree.AppendNode(new object[] { "root" }, null));
            stack.Peek().Tag = string.Empty;
            stack.Peek().StateImageIndex = 0;

            for(int f = 0; f < files.Count; f++)
            {
                VPK.File file = files[f];

                string[] fileSplit = file.path.Split('/');

                while(stackString.Count >= fileSplit.Length)
                {
                    stackString.Pop();
                    stack.Pop();
                }

                for(int i = stackString.Count - 1; i >= 0; i--)
                {
                    if(stackString.Peek() != fileSplit[i])
                    {
                        stackString.Pop();
                        stack.Pop();
                    } else
                        break;
                }

                for(int i = stack.Count - 1; i < fileSplit.Length - 1; i++)
                {
                    string tag = fileSplit[i] + "/";
                    if(stackString.Count > 0)
                        tag = stack.Peek().Tag.ToString() + tag;

                    stack.Push(tree.AppendNode(new object[] { fileSplit[i] }, stack.Peek()));
                    stack.Peek().Tag = tag;
                    stack.Peek().StateImageIndex = 0;
                    stackString.Push(fileSplit[i]);
                }
            }

            tree.ExpandToLevel(0);

            tree.EndUnboundLoad();
        }
    }
}