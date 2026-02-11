using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChecksumGen
{
    /// <summary>
    /// Simple checksum generator UI form.
    /// Accepts files via dialog or drag/drop, generates checksums and allows saving them to disk.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// List of file paths to generate checksums for.
        /// </summary>
        [Description("List of file paths to generate checksums for.")]
        private List<string> fileList = new List<string>();

        /// <summary>
        /// Generated checksum strings aligned with <see cref="fileList"/> by index.
        /// </summary>
        [Description("List of generated checksums corresponding to the file paths.")]
        private List<string> checksumList = new List<string>();

        /// <summary>
        /// Target save location chosen by the user.
        /// </summary>
        [Description("Target location to save the checksum file.")]
        private string targetLocation = string.Empty;

        /// <summary>
        /// Selected checksum format for generation.
        /// </summary>
        [Description("Selected checksum format for generation.")]
        private CheckSumFormat checksumFormat = CheckSumFormat.MD5;

        /// <summary>
        /// Supported checksum formats. Note: CRC32 and SHA3 variants are not implemented and will be empty strings.
        /// </summary>
        public enum CheckSumFormat
        {
            MD5,
            SHA1,
            SHA256,
            SHA512,
            CRC32,
            SHA3_256,
            SHA3_512
        }

        /// <summary>
        /// Get the recommended file extension for a given checksum format.
        /// </summary>
        /// <param name="format">The checksum format.</param>
        /// <returns>The file extension (without dot) associated with the format.</returns>
        /// <summary>
        /// Get the recommended file extension for a given checksum format.
        /// </summary>
        /// <param name="format">The checksum format.</param>
        /// <returns>The file extension (without dot) associated with the format.</returns>
        public string GetFileExtensionForFormat(CheckSumFormat format)
        {
            return format switch
            {
                CheckSumFormat.MD5 => "md5",
                CheckSumFormat.SHA1 => "sha1",
                CheckSumFormat.SHA256 => "sha256",
                CheckSumFormat.SHA512 => "sha512",
                CheckSumFormat.CRC32 => "sfv",
                CheckSumFormat.SHA3_256 => "sha3-256",
                CheckSumFormat.SHA3_512 => "sha3-512",
                _ => "chk",
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> form and sets up default dialog filters.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            saveFileDialog.Filter = "MD5 (*.md5)|*.md5";
            saveFileDialog.Filter += "|SHA1 (*.sha1)|*.sha1";
            saveFileDialog.Filter += "|SHA256 (*.sha256)|*.sha256";
            saveFileDialog.Filter += "|SHA512 (*.sha512)|*.sha512";
            //saveFileDialog.Filter += "|CRC-32 (*.sfv)|*.sfv";
            //saveFileDialog.Filter += "|SHA3-256 (*.sha3-256)|*.sha3-256";
            //saveFileDialog.Filter += "|SHA3-512 (*.sha3-512)|*.sha3-512";
            saveFileDialog.DefaultExt = "md5";
            saveFileDialog.Title = "Save Checksum File";
        }

        /// <summary>
        /// Handles the ItemDrag event for the file list. Currently reserved for future drag operations.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">ItemDrag event data.</param>
        private void lv_FileList_ItemDrag(object sender, ItemDragEventArgs e)
        {

        }

        /// <summary>
        /// Handles the DragDrop event for the file list and forwards the call to the form drag-drop handler.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">DragEvent data.</param>
        private void lv_FileList_DragDrop(object sender, DragEventArgs e)
        {
            ChecksumGen_DragDrop(sender, e);
        }

        /// <summary>
        /// Handles the DragEnter event for the file list to determine whether file drops are allowed.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">DragEvent data.</param>
        private void lv_FileList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Adds a file path to the internal file list if it is not already present (case-insensitive).
        /// This method is safe to call from background threads and will marshal updates to the UI thread.
        /// </summary>
        /// <param name="path">The full file path to add.</param>
        private void AddPathToList(string path)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddPathToList(path)));
                return;
            }

            // Normalize and avoid duplicates (case-insensitive)
            try
            {
                var full = Path.GetFullPath(path);
                if (fileList.Any(f => string.Equals(f, full, StringComparison.OrdinalIgnoreCase)))
                    return;

                fileList.Add(full);
                UpdateListViewFromFileList();
                UpdateFileCountStatus();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AddPathToList: failed to add '{path}': {ex}");
            }
        }

        /// <summary>
        /// Refreshes the ListView to reflect the current contents of the internal file and checksum lists.
        /// This method will marshal to the UI thread when required.
        /// </summary>
        private void UpdateListViewFromFileList()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateListViewFromFileList));
                return;
            }

            lv_FileList.BeginUpdate();
            try
            {
                lv_FileList.Items.Clear();
                for (int i = 0; i < fileList.Count; i++)
                {
                    var path = fileList[i];
                    var item = new ListViewItem(path);
                    // If a checksum exists for this index, add it as a subitem
                    if (i < checksumList.Count && !string.IsNullOrEmpty(checksumList[i]))
                    {
                        item.SubItems.Add(checksumList[i]);
                    }
                    else
                    {
                        item.SubItems.Add(string.Empty);
                    }

                    lv_FileList.Items.Add(item);
                }
            }
            finally
            {
                lv_FileList.EndUpdate();
            }
        }

        /// <summary>
        /// Updates the status label to show the current number of files in the list.
        /// This method is safe to call from background threads and will marshal updates to the UI thread.
        /// </summary>
        private void UpdateFileCountStatus()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(UpdateFileCountStatus));
                    return;
                }

                if (toolStripStatusLbl != null)
                {
                    // Use fileList.Count which is the source of truth
                    toolStripStatusLbl.Text = $"Files: {fileList.Count}";
                    // Enable Save only when we have at least one file
                    try
                    {
                        if (toolStripButton_Save != null)
                            toolStripButton_Save.Enabled = (fileList.Count > 0);
                    }
                    catch
                    {
                        // ignore if UI element not accessible for any reason
                    }
                }
            }
            catch
            {
                // ignore UI update errors
            }
        }

        /// <summary>
        /// Processes an array of dropped paths, adding files and enumerating directories recursively.
        /// </summary>
        /// <param name="paths">Array of file or directory paths dropped by the user.</param>
        private void HandleDroppedFiles(string[] paths)
        {
            foreach (var p in paths)
            {
                try
                {
                    if (System.IO.File.Exists(p))
                    {
                        AddPathToList(p);
                    }
                    else if (System.IO.Directory.Exists(p))
                    {
                        // enumerate files recursively
                        foreach (var file in System.IO.Directory.EnumerateFiles(p, "*", System.IO.SearchOption.AllDirectories))
                        {
                            AddPathToList(file);
                        }
                    }
                }
                catch
                {
                    // ignore items we can't access
                }
            }
        }

        /// <summary>
        /// Form-level drag-drop handler that accepts file drops and forwards the list to processing.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">DragEvent data.</param>
        private void ChecksumGen_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                HandleDroppedFiles(files);
            }
        }

        /// <summary>
        /// Form-level drag-enter handler to indicate whether the current drag data can be accepted.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">DragEvent data.</param>
        private void ChecksumGen_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Clears the current file and checksum lists, prompting the user if there are existing entries.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        private void toolStripButton_New_Click(object sender, EventArgs e)
        {
            if (fileList.Count > 0)
            {
                var result = MessageBox.Show("Clear the current file list?", "New Checksum List", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    return;
            }
            fileList.Clear();
            checksumList.Clear();
            UpdateListViewFromFileList();
            UpdateFileCountStatus();
        }

        /// <summary>
        /// Launches the save file dialog, determines the selected checksum format, generates checksums and saves them to disk.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        private void toolStripButton_Save_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                targetLocation = saveFileDialog.FileName;
                // Determine format by selected filter index (more reliable than comparing the filter string)
                switch (saveFileDialog.FilterIndex)
                {
                    case 1:
                        checksumFormat = CheckSumFormat.MD5;
                        break;
                    case 2:
                        checksumFormat = CheckSumFormat.SHA1;
                        break;
                    case 3:
                        checksumFormat = CheckSumFormat.SHA256;
                        break;
                    case 4:
                        checksumFormat = CheckSumFormat.SHA512;
                        break;
                    default:
                        checksumFormat = CheckSumFormat.MD5;
                        break;
                }

                GenerateChecksums();
                SaveChecksumsToFile();
            }
        }

        /// <summary>
        /// Computes the checksum for a file using the specified format.
        /// </summary>
        /// <param name="filePath">The full path to the file to hash.</param>
        /// <param name="format">The checksum algorithm to use.</param>
        /// <param name="checksum">Outputs the computed checksum as a lowercase hexadecimal string, or empty on error/unsupported algorithm.</param>
        /// <summary>
        /// Compute the checksum for a file using the specified format. Unsupported algorithms will result in an empty string.
        /// </summary>
        /// <param name="filePath">Full file path.</param>
        /// <param name="format">Algorithm to use.</param>
        /// <param name="checksum">Output checksum string (lowercase hex) or empty on error.</param>
        void GetChecksum(string filePath, CheckSumFormat format, out string checksum)
        {
            checksum = string.Empty;
            try
            {
                using (var stream = File.OpenRead(filePath))
                {
                    switch (format)
                    {
                        case CheckSumFormat.MD5:
                            checksum = ComputeHashHex(stream, MD5.Create());
                            break;
                        case CheckSumFormat.SHA1:
                            checksum = ComputeHashHex(stream, SHA1.Create());
                            break;
                        case CheckSumFormat.SHA256:
                            checksum = ComputeHashHex(stream, SHA256.Create());
                            break;
                        case CheckSumFormat.SHA512:
                            checksum = ComputeHashHex(stream, SHA512.Create());
                            break;
                        case CheckSumFormat.CRC32:
                            // CRC32 is not implemented. Could add a lightweight CRC32 implementation or a NuGet package.
                            Debug.WriteLine("CRC32 not implemented");
                            break;
                        case CheckSumFormat.SHA3_256:
                        case CheckSumFormat.SHA3_512:
                            // SHA3 is not available in System.Security.Cryptography until later runtimes or via third-party libraries.
                            Debug.WriteLine("SHA3 not implemented (consider adding a library for SHA3 support)");
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetChecksum failed for '{filePath}': {ex}");
            }
        }

        /// <summary>
        /// Compute a hex string from a stream using the provided HashAlgorithm instance.
        /// The caller is responsible for disposing the algorithm if needed; this method disposes it.
        /// </summary>
        private static string ComputeHashHex(Stream stream, HashAlgorithm algorithm)
        {
            using (algorithm)
            {
                var hash = algorithm.ComputeHash(stream);
                var sb = new StringBuilder(hash.Length * 2);
                foreach (var b in hash)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        /// <summary>
        /// Generates checksums for all files currently in the file list using the selected checksum format.
        /// The results are stored in the internal checksum list and the UI is refreshed.
        /// </summary>
        void GenerateChecksums()
        {
            checksumList.Clear();
            foreach (var filePath in fileList)
            {
                GetChecksum(filePath, checksumFormat, out string checksum);
                // Always add an entry so checksumList has the same indexing as fileList.
                checksumList.Add(checksum ?? string.Empty);
            }
            // Refresh the list view so any newly-generated checksums appear as subitems
            UpdateListViewFromFileList();
        }

        /// <summary>
        /// Saves the generated checksums to the target file. Each line contains the checksum and the absolute file path.
        /// </summary>
        void SaveChecksumsToFile()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(targetLocation))
                {
                    Debug.WriteLine("SaveChecksumsToFile: targetLocation is empty");
                    return;
                }

                using (var writer = new StreamWriter(targetLocation, false, Encoding.UTF8))
                {
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        var path = fileList[i];
                        var checksum = (i < checksumList.Count) ? checksumList[i] : string.Empty;
                        if (!string.IsNullOrEmpty(checksum))
                        {
                            writer.WriteLine($"{checksum} {Path.GetFullPath(path)}");
                        }
                    }
                }
            }
            catch
            {
                // Handle exceptions (e.g., access denied)
                Debug.WriteLine($"SaveChecksumsToFile: failed to save to '{targetLocation}'");
            }
        }

        /// <summary>
        /// Shows the open file dialog and adds the selected files to the list.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        private void toolStripButton_Add_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    AddPathToList(file);
                }
            }
        }

        /// <summary>
        /// Removes checked items from the file list and corresponding checksums.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        private void toolStripButton_Remove_Click(object sender, EventArgs e)
        {
            if (lv_FileList.CheckedItems.Count > 0)
            {
                foreach (ListViewItem item in lv_FileList.CheckedItems)
                {
                    var path = item.Text;
                    int index = fileList.FindIndex(f => string.Equals(f, path, StringComparison.OrdinalIgnoreCase));
                    if (index >= 0)
                    {
                        fileList.RemoveAt(index);
                        if (index < checksumList.Count)
                        {
                            checksumList.RemoveAt(index);
                        }
                    }
                }
                UpdateListViewFromFileList();
                UpdateFileCountStatus();
            }
            toolStripButton_Remove.Enabled = false;
        }

        /// <summary>
        /// Enables or disables the Remove toolbar button based on whether any ListView items are checked.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">ItemChecked event data.</param>
        private void lv_FileList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (lv_FileList.CheckedItems.Count > 0)
            {
                toolStripButton_Remove.Enabled = true;
            }
            else
            {
                toolStripButton_Remove.Enabled = false;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text += $" (v{Application.ProductVersion})";
        }
    }
}
