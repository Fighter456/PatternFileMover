using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace PatternFileMover
{
    public partial class NameAssociationsForm : Form
    {
        public NameAssociationsForm()
        {
            InitializeComponent();
        }

        private void NameAssociationsForm_Load(object sender, EventArgs e)
        {
            var dataList = new BindingList<NameAssociationsData>(NameAssociations.LoadFromExistingConfigFile());
            var dataSource = new BindingSource(dataList, null);

            dataGridView1.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView1_CellValidating);
            dataGridView1.DataSource = dataSource;
            dataGridView1.AutoResizeColumns();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // equals the column for the target directory
            if (e.ColumnIndex == 1)
            {
                var folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.SelectedPath = dataGridView1.SelectedCells[0].Value.ToString();
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    dataGridView1.SelectedCells[0].Value = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].ErrorText = String.Empty;
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name.Equals("SearchPattern"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = "Dieser Eintrag ist obligatorisch.";
                    e.Cancel = true;
                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["SearchPattern"].Value == null) {
                        continue;
                    }

                    if (
                        !row.Index.Equals(e.RowIndex) &&
                        row.Cells["SearchPattern"].Value.ToString().Equals(e.FormattedValue.ToString())
                        )
                    {
                        dataGridView1.Rows[e.RowIndex].ErrorText = "Dieser Eintrag ist bereits vorhanden.";
                        e.Cancel = true;
                    }
                }
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name.Equals("TargetDirectory"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = "Dieser Eintrag ist obligatorisch.";
                    e.Cancel = true;
                }
                else if (!Directory.Exists(e.FormattedValue.ToString())) {
                    dataGridView1.Rows[e.RowIndex].ErrorText = "Der eingebene Pfad existiert nicht.";
                    e.Cancel = true;
                }
            }
        }
    }
}
