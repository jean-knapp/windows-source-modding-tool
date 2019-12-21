﻿namespace windows_source1ide
{
    partial class NewModForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textTitle = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.textFolder = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.comboGames = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.createButton = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.labelFolderInfo = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.comboBranches = new DevExpress.XtraEditors.ComboBoxEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl6 = new DevExpress.XtraEditors.PanelControl();
            this.textClient = new DevExpress.XtraEditors.TextEdit();
            this.textServer = new DevExpress.XtraEditors.TextEdit();
            this.buttonClient = new DevExpress.XtraEditors.SimpleButton();
            this.buttonServer = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textFolder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboGames.Properties)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBranches.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).BeginInit();
            this.panelControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textServer.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // textTitle
            // 
            this.textTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textTitle.Location = new System.Drawing.Point(68, 3);
            this.textTitle.Name = "textTitle";
            this.textTitle.Size = new System.Drawing.Size(464, 20);
            this.textTitle.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(3, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(41, 13);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Mod title";
            // 
            // textFolder
            // 
            this.textFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.textFolder.Location = new System.Drawing.Point(0, 0);
            this.textFolder.Name = "textFolder";
            this.textFolder.Size = new System.Drawing.Size(464, 20);
            this.textFolder.TabIndex = 2;
            this.textFolder.EditValueChanged += new System.EventHandler(this.textFolder_EditValueChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(3, 29);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(59, 13);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "Folder name";
            // 
            // comboGames
            // 
            this.comboGames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboGames.Location = new System.Drawing.Point(68, 73);
            this.comboGames.Name = "comboGames";
            this.comboGames.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboGames.Properties.ReadOnly = true;
            this.comboGames.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboGames.Size = new System.Drawing.Size(464, 20);
            this.comboGames.TabIndex = 4;
            this.comboGames.TextChanged += new System.EventHandler(this.comboGames_TextChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(3, 73);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(52, 13);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "Base game";
            // 
            // createButton
            // 
            this.createButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.createButton.Enabled = false;
            this.createButton.Location = new System.Drawing.Point(468, 8);
            this.createButton.Margin = new System.Windows.Forms.Padding(8);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 6;
            this.createButton.Text = "OK";
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.Controls.Add(this.panelControl3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textTitle, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelControl2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelControl3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboGames, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelControl4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBranches, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelControl5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelControl6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.panelControl5, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.panelControl6, 1, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 8);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(535, 254);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // panelControl3
            // 
            this.panelControl3.AutoSize = true;
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl3.Controls.Add(this.panelControl4);
            this.panelControl3.Controls.Add(this.textFolder);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(68, 29);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(464, 38);
            this.panelControl3.TabIndex = 8;
            // 
            // panelControl4
            // 
            this.panelControl4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl4.Controls.Add(this.labelFolderInfo);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl4.Location = new System.Drawing.Point(0, 20);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(464, 18);
            this.panelControl4.TabIndex = 7;
            // 
            // labelFolderInfo
            // 
            this.labelFolderInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelFolderInfo.Location = new System.Drawing.Point(401, 0);
            this.labelFolderInfo.Name = "labelFolderInfo";
            this.labelFolderInfo.Size = new System.Drawing.Size(63, 13);
            this.labelFolderInfo.TabIndex = 7;
            this.labelFolderInfo.Text = "Invalid folder";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(3, 99);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(33, 13);
            this.labelControl4.TabIndex = 9;
            this.labelControl4.Text = "Branch";
            // 
            // comboBranches
            // 
            this.comboBranches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBranches.Location = new System.Drawing.Point(68, 99);
            this.comboBranches.Name = "comboBranches";
            this.comboBranches.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBranches.Properties.ReadOnly = true;
            this.comboBranches.Size = new System.Drawing.Size(464, 20);
            this.comboBranches.TabIndex = 10;
            this.comboBranches.TextChanged += new System.EventHandler(this.comboBranches_TextChanged);
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.createButton);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 262);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(551, 39);
            this.panelControl1.TabIndex = 9;
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.tableLayoutPanel1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Padding = new System.Windows.Forms.Padding(8, 8, 8, 0);
            this.panelControl2.Size = new System.Drawing.Size(551, 262);
            this.panelControl2.TabIndex = 10;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(3, 125);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(40, 13);
            this.labelControl5.TabIndex = 11;
            this.labelControl5.Text = "Client dll";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(3, 151);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(45, 13);
            this.labelControl6.TabIndex = 12;
            this.labelControl6.Text = "Server dll";
            // 
            // panelControl5
            // 
            this.panelControl5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelControl5.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl5.Controls.Add(this.textClient);
            this.panelControl5.Controls.Add(this.buttonClient);
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl5.Location = new System.Drawing.Point(68, 125);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(464, 20);
            this.panelControl5.TabIndex = 13;
            // 
            // panelControl6
            // 
            this.panelControl6.AutoSize = true;
            this.panelControl6.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl6.Controls.Add(this.textServer);
            this.panelControl6.Controls.Add(this.buttonServer);
            this.panelControl6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl6.Location = new System.Drawing.Point(68, 151);
            this.panelControl6.Name = "panelControl6";
            this.panelControl6.Size = new System.Drawing.Size(464, 20);
            this.panelControl6.TabIndex = 14;
            // 
            // textClient
            // 
            this.textClient.Dock = System.Windows.Forms.DockStyle.Top;
            this.textClient.Location = new System.Drawing.Point(0, 0);
            this.textClient.Name = "textClient";
            this.textClient.Properties.ReadOnly = true;
            this.textClient.Size = new System.Drawing.Size(389, 20);
            this.textClient.TabIndex = 0;
            // 
            // textServer
            // 
            this.textServer.Dock = System.Windows.Forms.DockStyle.Top;
            this.textServer.Location = new System.Drawing.Point(0, 0);
            this.textServer.Name = "textServer";
            this.textServer.Properties.ReadOnly = true;
            this.textServer.Size = new System.Drawing.Size(389, 20);
            this.textServer.TabIndex = 0;
            // 
            // buttonClient
            // 
            this.buttonClient.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonClient.Enabled = false;
            this.buttonClient.Location = new System.Drawing.Point(389, 0);
            this.buttonClient.Name = "buttonClient";
            this.buttonClient.Size = new System.Drawing.Size(75, 20);
            this.buttonClient.TabIndex = 1;
            this.buttonClient.Text = "Search";
            this.buttonClient.Click += new System.EventHandler(this.buttonClient_Click);
            // 
            // buttonServer
            // 
            this.buttonServer.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonServer.Enabled = false;
            this.buttonServer.Location = new System.Drawing.Point(389, 0);
            this.buttonServer.Name = "buttonServer";
            this.buttonServer.Size = new System.Drawing.Size(75, 20);
            this.buttonServer.TabIndex = 1;
            this.buttonServer.Text = "Search";
            this.buttonServer.Click += new System.EventHandler(this.buttonServer_Click);
            // 
            // NewModForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 301);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "NewModForm";
            this.Text = "New Mod";
            this.Load += new System.EventHandler(this.NewModForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textFolder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboGames.Properties)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panelControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBranches.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).EndInit();
            this.panelControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textClient.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textServer.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit textTitle;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit textFolder;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit comboGames;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton createButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl labelFolderInfo;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit comboBranches;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraEditors.TextEdit textClient;
        private DevExpress.XtraEditors.SimpleButton buttonClient;
        private DevExpress.XtraEditors.PanelControl panelControl6;
        private DevExpress.XtraEditors.TextEdit textServer;
        private DevExpress.XtraEditors.SimpleButton buttonServer;
    }
}