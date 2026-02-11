namespace ChecksumGen
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            lv_FileList = new ListView();
            columnHeader_path = new ColumnHeader();
            columnHeader_checksum = new ColumnHeader();
            toolStrip = new ToolStrip();
            toolStripButton_New = new ToolStripButton();
            toolStripButton_Save = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripButton_Add = new ToolStripButton();
            toolStripButton_Remove = new ToolStripButton();
            statusStrip = new StatusStrip();
            toolStripStatusLbl = new ToolStripStatusLabel();
            toolStripProgressBar = new ToolStripProgressBar();
            saveFileDialog = new SaveFileDialog();
            openFileDialog = new OpenFileDialog();
            toolStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // lv_FileList
            // 
            lv_FileList.AllowDrop = true;
            lv_FileList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lv_FileList.CheckBoxes = true;
            lv_FileList.Columns.AddRange(new ColumnHeader[] { columnHeader_path, columnHeader_checksum });
            lv_FileList.FullRowSelect = true;
            lv_FileList.Location = new Point(0, 28);
            lv_FileList.Name = "lv_FileList";
            lv_FileList.Size = new Size(800, 422);
            lv_FileList.TabIndex = 0;
            lv_FileList.UseCompatibleStateImageBehavior = false;
            lv_FileList.View = View.Details;
            lv_FileList.ItemChecked += lv_FileList_ItemChecked;
            lv_FileList.ItemDrag += lv_FileList_ItemDrag;
            lv_FileList.DragDrop += lv_FileList_DragDrop;
            lv_FileList.DragEnter += lv_FileList_DragEnter;
            // 
            // columnHeader_path
            // 
            columnHeader_path.Text = "Filepath";
            columnHeader_path.Width = 400;
            // 
            // columnHeader_checksum
            // 
            columnHeader_checksum.Text = "Checksum";
            columnHeader_checksum.Width = 320;
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripButton_New, toolStripButton_Save, toolStripSeparator1, toolStripButton_Add, toolStripButton_Remove });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(800, 25);
            toolStrip.TabIndex = 1;
            toolStrip.Text = "toolStrip1";
            // 
            // toolStripButton_New
            // 
            toolStripButton_New.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton_New.Image = Resources.NewFile;
            toolStripButton_New.ImageTransparentColor = Color.Magenta;
            toolStripButton_New.Name = "toolStripButton_New";
            toolStripButton_New.Size = new Size(23, 22);
            toolStripButton_New.Text = "New";
            toolStripButton_New.Click += toolStripButton_New_Click;
            // 
            // toolStripButton_Save
            // 
            toolStripButton_Save.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton_Save.Enabled = false;
            toolStripButton_Save.Image = Resources.Save;
            toolStripButton_Save.ImageTransparentColor = Color.Magenta;
            toolStripButton_Save.Name = "toolStripButton_Save";
            toolStripButton_Save.Size = new Size(23, 22);
            toolStripButton_Save.Text = "Save";
            toolStripButton_Save.Click += toolStripButton_Save_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // toolStripButton_Add
            // 
            toolStripButton_Add.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton_Add.Image = Resources.Add;
            toolStripButton_Add.ImageTransparentColor = Color.Magenta;
            toolStripButton_Add.Name = "toolStripButton_Add";
            toolStripButton_Add.Size = new Size(23, 22);
            toolStripButton_Add.Text = "Add";
            toolStripButton_Add.Click += toolStripButton_Add_Click;
            // 
            // toolStripButton_Remove
            // 
            toolStripButton_Remove.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton_Remove.Enabled = false;
            toolStripButton_Remove.Image = Resources.Delete;
            toolStripButton_Remove.ImageTransparentColor = Color.Magenta;
            toolStripButton_Remove.Name = "toolStripButton_Remove";
            toolStripButton_Remove.Size = new Size(23, 22);
            toolStripButton_Remove.Text = "Remove";
            toolStripButton_Remove.Click += toolStripButton_Remove_Click;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLbl, toolStripProgressBar });
            statusStrip.Location = new Point(0, 428);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(800, 22);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLbl
            // 
            toolStripStatusLbl.Name = "toolStripStatusLbl";
            toolStripStatusLbl.Size = new Size(42, 17);
            toolStripStatusLbl.Text = "Files: 0";
            // 
            // toolStripProgressBar
            // 
            toolStripProgressBar.Name = "toolStripProgressBar";
            toolStripProgressBar.Size = new Size(200, 16);
            // 
            // saveFileDialog
            // 
            saveFileDialog.Filter = "MD5 (*.md5)|*.md5";
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "All Files (*.*)|*.*";
            openFileDialog.Multiselect = true;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(statusStrip);
            Controls.Add(toolStrip);
            Controls.Add(lv_FileList);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "Chicksum Generator (Chirp chirp!)";
            Load += MainForm_Load;
            DragDrop += ChecksumGen_DragDrop;
            DragEnter += ChecksumGen_DragEnter;
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView lv_FileList;
        private ColumnHeader columnHeader_path;
        private ColumnHeader columnHeader_checksum;
        private ToolStrip toolStrip;
        private ToolStripButton toolStripButton_New;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLbl;
        private ToolStripProgressBar toolStripProgressBar;
        private ToolStripButton toolStripButton_Save;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripButton_Add;
        private SaveFileDialog saveFileDialog;
        private ToolStripButton toolStripButton_Remove;
        private OpenFileDialog openFileDialog;
    }
}