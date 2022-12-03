using System;
using System.IO;
using System.Windows.Forms;

namespace PatternFileMover
{
    public partial class Form1 : Form
    {
        private FolderBrowserDialog sourceFolderBrowserDialog = new FolderBrowserDialog();
        private string sourceDirectory;

        public Form1()
        {
            InitializeComponent();

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView1.ColumnCount = 1;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns[0].Name = "Dateiname";
            dataGridView1.Columns[0].Width = 773;
        }

        private void quellverzeichnisAuswählenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = sourceFolderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                // clear grid
                dataGridView1.Rows.Clear();

                this.sourceDirectory = sourceFolderBrowserDialog.SelectedPath;

                string[] files = Directory.GetFiles(this.sourceDirectory, "*.pdf");
                foreach (string file in files)
                {
                    dataGridView1.Rows.Add(file);
                }

                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }
    }
}
