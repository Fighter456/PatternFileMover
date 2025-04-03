using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace PatternFileMover
{
    public partial class Form1 : Form
    {
        private FolderBrowserDialog sourceFolderBrowserDialog = new FolderBrowserDialog();
        private string sourceDirectory;
        private List<NameAssociationsData_v2> nameAssociations = new List<NameAssociationsData_v2>();
        private int processedFileCount = 0;

        public Form1()
        {
            InitializeComponent();

            dataGridView1.ColumnCount = 1;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns[0].Name = "Dateiname";

            // ensure the existance of the necessary config file
            if (!File.Exists(NameAssociations.configManifestPath))
            {
                // file not found
                // typically at the first usage of the program
                // create an empty config file
                NameAssociations.CreateEmptyConfigFile();
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

        private void quellverzeichnisAuswählenToolStripMenuItem_Click(object sender, EventArgs e)
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
                        "Es wurde ein Verzeichnis ausgewählt, welches keine Dateien beinhaltet. Anderes Verzeichnis auswählen?",
                        "Hinweis: Verzeichnisauswahl",
                        MessageBoxButtons.YesNo
                    );

                    if (result == DialogResult.Yes)
                    {
                        this.quellverzeichnisAuswählenToolStripMenuItem_Click(sender, e);
                    }
                    else
                    {
                        button2.Visible = true;
                    }
                }
            }
        }

        private void zuordnungenBearbeitenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new NameAssociationsForm().ShowDialog();
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs eventArgs)
        {   
            for (int i = 0; i <= dataGridView1.RowCount - 1; i++)
            {
                foreach (NameAssociationsData_v2 data in this.nameAssociations)
                {
                    if (
                        (
                            data.FileExtension == "*.*" &&
                            Path.GetFileNameWithoutExtension(
                                dataGridView1.Rows[i].Cells[0].Value.ToString()
                            ).Contains(data.SearchPattern)
                        ) ||
                        (
                           Path.GetFileNameWithoutExtension(
                               dataGridView1.Rows[i].Cells[0].Value.ToString()
                           ).Contains(data.SearchPattern) &&
                           data.FileExtension == Path.GetExtension(dataGridView1.Rows[i].Cells[0].Value.ToString())
                        )
                    )
                    {
                        if (!Directory.Exists(data.TargetDirectory + Path.DirectorySeparatorChar))
                        {
                            string warningMessage = $"Warning: Target directory '{data.TargetDirectory}' does not exist. Possible broken name association.";

                            // Log the warning (if there's a logging system)
                            Console.WriteLine(warningMessage);

                            // Optionally show a message box in the UI
                            MessageBox.Show(warningMessage, "Missing Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            // Skip and continue processing
                            continue;
                        }


                        if (File.Exists(
                                data.TargetDirectory + 
                                Path.DirectorySeparatorChar + 
                                Path.GetFileName(dataGridView1.Rows[i].Cells[0].Value.ToString())
                            )
                        )
                        {
                            // delete the existing file before it gets replaced with the current
                            // processed file
                            File.Delete(
                                data.TargetDirectory +
                                Path.DirectorySeparatorChar +
                                Path.GetFileName(dataGridView1.Rows[i].Cells[0].Value.ToString())
                            );
                        }

                        File.Move(
                            dataGridView1.Rows[i].Cells[0].Value.ToString(),
                            data.TargetDirectory + Path.DirectorySeparatorChar + Path.GetFileName(dataGridView1.Rows[i].Cells[0].Value.ToString())
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
                    "Es wurden keine Zuordnungen angelegt. Bitte erstellen Sie zuerst mindestens eine Zuordnung. Jetzt Zuordnung anlegen?",
                    "Hinweis: Keine Zuordnung gefunden",
                    MessageBoxButtons.YesNo
                );

                if (result == DialogResult.Yes)
                {
                    this.zuordnungenBearbeitenToolStripMenuItem_Click(new object(), new EventArgs());
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
                    "Die Verarbeitung ist erfolgt. Es wurde(n) {0} Datei(en) gemäß der Zuordnung(en) verschoben. Wollen Sie weitere Dateien verarbeiten?",
                    processedFileCount
                ),
                "Verarbeitung abgeschlossen",
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
    }
}
