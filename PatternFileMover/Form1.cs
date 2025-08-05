using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Resources;
using System.Windows.Forms;

namespace PatternFileMover
{
    public partial class Form1 : Form
    {
        private FolderBrowserDialog sourceFolderBrowserDialog = new FolderBrowserDialog();
        private string sourceDirectory;
        private List<NameAssociationsData_v3> nameAssociations = new List<NameAssociationsData_v3>();
        private int processedFileCount = 0;
        private ResourceManager i18n = new ResourceManager(
            "PatternFileMover.Form1",
            typeof(Form1).Assembly
        );

        public Form1()
        {
            InitializeComponent();

            dataGridView1.ColumnCount = 1;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns[(int)NameAssociationCellIndex.Name].Name = i18n.GetString("filename");

            // ensure the existance of the necessary config file
            if (!File.Exists(NameAssociations.configManifestPath))
            {
                // the manifest file hasn't been found…
                if (File.Exists(NameAssociations.legacyConfigPath))
                {
                    // … but there is a legacy config file
                    // that means that we need to convert a `v1` config file
                    NameAssociations.checkAndUpgradeConfigurationFile(true);
                }
                else 
                {
                    // file not found
                    // typically at the first usage of the program
                    // create an empty config file
                    NameAssociations.CreateEmptyConfigFile();
                }
            }
            else
            {
                NameAssociations.checkAndUpgradeConfigurationFile();
            }

            // background worker
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        private void selectSourceDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            DialogResult result = sourceFolderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                // clear grid
                dataGridView1.Rows.Clear();

                this.sourceDirectory = sourceFolderBrowserDialog.SelectedPath;

                string[] files = Directory.GetFiles(this.sourceDirectory, "*.*");
                foreach (string file in files)
                {
                    dataGridView1.Rows.Add(file);
                }

                if (dataGridView1.Rows.Count > 0)
                {
                    button1.Visible = true;
                    button2.Visible = false;
                }
                else
                {
                    result = MessageBox.Show(
                        i18n.GetString("sourceFolderEmpty"),
                        i18n.GetString("sourceFolderEmpty.Title"),
                        MessageBoxButtons.YesNo
                    );

                    if (result == DialogResult.Yes)
                    {
                        this.selectSourceDirectoryToolStripMenuItem_Click(sender, e);
                    }
                    else
                    {
                        button2.Visible = true;
                    }
                }
            }
        }

        private void editNameAssociationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new NameAssociationsForm().ShowDialog();
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs eventArgs)
        {   
            for (int i = 0; i <= dataGridView1.RowCount - 1; i++)
            {
                foreach (NameAssociationsData_v3 data in this.nameAssociations)
                {
                    if (
                        (
                            data.FileExtension == "*.*" &&
                            Path.GetFileNameWithoutExtension(
                                dataGridView1.Rows[i].Cells[(int)NameAssociationCellIndex.Name].Value.ToString()
                            ).Contains(data.SearchPattern)
                        ) ||
                        (
                           Path.GetFileNameWithoutExtension(
                               dataGridView1.Rows[i].Cells[0].Value.ToString()
                           ).Contains(data.SearchPattern) &&
                           data.FileExtension == Path.GetExtension(dataGridView1.Rows[i].Cells[(int)NameAssociationCellIndex.Name].Value.ToString())
                        )
                    )
                    {
                        if (!Directory.Exists(data.TargetDirectory + Path.DirectorySeparatorChar))
                        {
                            // the target directory does not existing
                            // maybe a broken name association or a network drive is not available
                            // skip this and go on
                            continue;
                        }

                        if (File.Exists(
                                data.TargetDirectory + 
                                Path.DirectorySeparatorChar + 
                                Path.GetFileName(dataGridView1.Rows[i].Cells[(int)NameAssociationCellIndex.Name].Value.ToString())
                            )
                        )
                        {
                            // delete the existing file before it gets replaced with the current
                            // processed file
                            File.Delete(
                                data.TargetDirectory +
                                Path.DirectorySeparatorChar +
                                Path.GetFileName(dataGridView1.Rows[i].Cells[(int)NameAssociationCellIndex.Name].Value.ToString())
                            );
                        }

                        File.Move(
                            dataGridView1.Rows[i].Cells[0].Value.ToString(),
                            data.TargetDirectory + Path.DirectorySeparatorChar + Path.GetFileName(dataGridView1.Rows[i].Cells[(int)NameAssociationCellIndex.Name].Value.ToString())
                        );

                        processedFileCount++;
                    }
                }

                backgroundWorker1.ReportProgress(100 * i / dataGridView1.RowCount);
            }
        }

        void backgroundWorker1_ProgressChanged (object sender, ProgressChangedEventArgs eventArgs)
        {
            progressBar1.Value = eventArgs.ProgressPercentage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // load configuration
            this.nameAssociations = NameAssociations.LoadFromExistingConfigFile();

            if (this.nameAssociations.Count == 0)
            {
                DialogResult result = MessageBox.Show(
                    i18n.GetString("noAssociations"),
                    i18n.GetString("noAssociations.Title"),
                    MessageBoxButtons.YesNo
                );

                if (result == DialogResult.Yes)
                {
                    this.editNameAssociationsToolStripMenuItem_Click(new object(), new EventArgs());
                    return;
                }
                else
                {
                    return;
                }
            }

            progressBar1.Visible = true;
            button1.Visible = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
            
            DialogResult dialogResult = MessageBox.Show(
                string.Format(
                    i18n.GetString("success"),
                    processedFileCount
                ),
                i18n.GetString("success.Title"),
                MessageBoxButtons.YesNo
            );

            if (dialogResult == DialogResult.No)
            {
                Application.Exit();
            }
            else if (dialogResult == DialogResult.Yes)
            {
                dataGridView1.Rows.Clear();

                button2.Visible = true;
                button2.PerformClick();
            }
            else {
                // this should never happen
                throw new InvalidOperationException();
            }
        }

        private void button2_onDragDrop(object sender, DragEventArgs e)
        {
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                var droppedData = ((string[]) e.Data.GetData(DataFormats.FileDrop));

                foreach (var data in droppedData)
                {
                    if (File.GetAttributes(data).HasFlag(FileAttributes.Directory))
                    {
                        // directory
                        string[] files = Directory.GetFiles(data, "*.*");
                        foreach (string file in files)
                        {
                            dataGridView1.Rows.Add(file);
                        }
                    }
                    else
                    {
                        // file
                        dataGridView1.Rows.Add(data);
                    }
                }

                if (dataGridView1.Rows.Count > 0)
                {
                    button1.Visible = true;
                    button2.Visible = false;
                }
            }
        }

        private void button2_onDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                return;
            }

            e.Effect = DragDropEffects.None;
        }
    }
}
