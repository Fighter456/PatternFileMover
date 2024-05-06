using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
            var dataList = new BindingList<NameAssociationsData>(NameAssociations.LoadFromExistingConfigFile()).OrderBy(x => x.Name).ToList();
            var dataSource = new BindingSource(dataList, null);

            panel1.Controls.Add(dataGridView1);

            dataGridView1.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView1_CellValidating);
            dataGridView1.DataSource = dataSource;

            if (dataGridView1.Rows.Count > 0)
            {
                this.Text = this.Text + " (" + dataGridView1.Rows.Count.ToString() + ")";
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex.ToString().Equals("-1"))
            {
                return;
            }
            
            // equals the column for the target directory
            if (e.ColumnIndex == 2)
            {
                var folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.SelectedPath = dataGridView1.SelectedCells[0].Value?.ToString();
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
            button1.Enabled = true;
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name.Equals("SearchPattern"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = "Dieser Eintrag ist obligatorisch.";
                    button1.Enabled = false;
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
                        button1.Enabled = false;
                        e.Cancel = true;
                    }
                }
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name.Equals("TargetDirectory"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = "Dieser Eintrag ist obligatorisch.";
                    button1.Enabled = false;
                    e.Cancel = true;
                }
                else if (!Directory.Exists(e.FormattedValue.ToString())) {
                    dataGridView1.Rows[e.RowIndex].ErrorText = "Der eingebene Pfad existiert nicht.";
                    button1.Enabled = false;
                    e.Cancel = true;
                }
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            button1.Enabled = true;
        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<NameAssociationsData> nameAssociationsData = new List<NameAssociationsData>();
            foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
            {
                if (dataGridViewRow.Index == (dataGridView1.Rows.Count -1))
                {
                    // skip last row
                    // does not contain any value because it is used for adding new items
                    continue;
                }

                nameAssociationsData.Add(new NameAssociationsData() {
                    Name = dataGridViewRow.Cells[0].Value?.ToString() ?? "",
                    SearchPattern = dataGridViewRow.Cells[1].Value.ToString(),
                    TargetDirectory = dataGridViewRow.Cells[2].Value.ToString()
                });
            }

            NameAssociations.WriteByList(nameAssociationsData);

            button1.Enabled = false;
        }
    }
}
