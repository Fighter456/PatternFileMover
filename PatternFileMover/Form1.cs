using System;
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
        }

        private void quellverzeichnisAuswählenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = sourceFolderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.sourceDirectory = sourceFolderBrowserDialog.SelectedPath;
            }
        }
    }
}
