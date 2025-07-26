using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows.Forms;

namespace PatternFileMover
{
    public partial class NameAssociationsForm : Form
    {
        private ResourceManager i18n = new ResourceManager(
            "PatternFileMover.NameAssociationsForm",
            typeof(NameAssociationsForm).Assembly
        );

        public NameAssociationsForm()
        {
            InitializeComponent();
        }

        private void NameAssociationsForm_Load(object sender, EventArgs e)
        {
            var dataList = new BindingList<NameAssociationsData_v2>(NameAssociations.LoadFromExistingConfigFile()).OrderBy(x => x.Name).ToList();
            var dataSource = new BindingSource(dataList, null);

            panel1.Controls.Add(dataGridView1);

            dataGridView1.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView1_CellValidating);
            dataGridView1.DataSource = dataSource;
            dataGridView1.DefaultValuesNeeded += dataGridView1_DefaultValuesNeeded;

            if ((dataGridView1.Rows.Count - 1) > 0)
            {
                this.Text = this.Text + " (" + dataGridView1.Rows.Count.ToString() + ")";
            }
            else
            {
                actionToolStripMenuItem.Visible = false;
            }

            foreach (DataGridViewColumn column in dataGridView1.Columns) {
                column.HeaderCell.Value = i18n.GetString("grid." + column.Name);
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
            actionToolStripMenuItem.Visible = true;
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name.Equals("SearchPattern"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = i18n.GetString("grid.Error.Required");
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
                        dataGridView1.Rows[e.RowIndex].ErrorText = i18n.GetString("grid.Error.Required");
                        button1.Enabled = false;
                        e.Cancel = true;
                    }
                }
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name.Equals("TargetDirectory"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = i18n.GetString("grid.Error.Required");
                    button1.Enabled = false;
                    e.Cancel = true;
                }
                else if (!Directory.Exists(e.FormattedValue.ToString())) {
                    dataGridView1.Rows[e.RowIndex].ErrorText = i18n.GetString("grid.TargetDirectory.Error.NotExists");
                    button1.Enabled = false;
                    e.Cancel = true;
                }
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name.Equals("FileExtension"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1.Rows[e.RowIndex].SetValues("*.*");
                }
                else if (
                    e.FormattedValue.ToString() != "*.*" &&
                    e.FormattedValue.ToString().Substring(0, 1) == "*"
                )
                {
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = e.FormattedValue.ToString().Substring(1);
                }
                else if (
                    e.FormattedValue.ToString().IndexOf(".") == -1
                ) {
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = "." + e.FormattedValue.ToString();
                }
            }
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, System.Windows.Forms.DataGridViewRowEventArgs e)
        {
            e.Row.Cells["FileExtension"].Value = "*.*";
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            button1.Enabled = true;
        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            button1.Enabled = true;

            if ((dataGridView1.Rows.Count - 1) == 0)
            {
                actionToolStripMenuItem.Visible = false;
                filterToolStripMenuItem.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<NameAssociationsData_v2> nameAssociationsData = new List<NameAssociationsData_v2>();
            foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
            {
                if (dataGridViewRow.Index == (dataGridView1.Rows.Count -1))
                {
                    // skip last row
                    // does not contain any value because it is used for adding new items
                    continue;
                }

                nameAssociationsData.Add(new NameAssociationsData_v2() {
                    Name = dataGridViewRow.Cells[0].Value?.ToString() ?? "",
                    SearchPattern = dataGridViewRow.Cells[1].Value.ToString(),
                    TargetDirectory = dataGridViewRow.Cells[2].Value.ToString(),
                    FileExtension = dataGridViewRow.Cells[3].Value.ToString()
                });
            }

            NameAssociations.WriteByList(nameAssociationsData);

            button1.Enabled = false;
        }

        private void checkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool brokenAssociationFound = false;
            bool intactAssociationFound = false;

            foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
            {
                if (dataGridViewRow.Index == (dataGridView1.Rows.Count - 1))
                {
                    // skip last row
                    // does not contain any value because it is used for adding new items
                    continue;
                }

                if (Directory.Exists(dataGridViewRow.Cells[2].Value.ToString()))
                {
                    dataGridViewRow.DefaultCellStyle.BackColor = Color.Green;
                    intactAssociationFound = true;
                }
                else
                {
                    dataGridViewRow.DefaultCellStyle.BackColor = Color.Red;
                    brokenAssociationFound = true;
                }

                dataGridViewRow.DefaultCellStyle.ForeColor = Color.White;
            }

            if (brokenAssociationFound || intactAssociationFound)
            {
                filterToolStripMenuItem.Visible = true;

                if (brokenAssociationFound)
                {
                    brokenAssociationToolStripMenuItem.Visible = true;
                }

                if (intactAssociationFound)
                {
                    intactAssociationToolStripMenuItem.Visible = true;
                }
            }
        }

        private void brokenAssociationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            intactAssociationToolStripMenuItem.Enabled = false;
            checkToolStripMenuItem.Enabled = false;
            resetToolStripMenuItem.Enabled = true;

            CurrencyManager currencyManager = (CurrencyManager)BindingContext[dataGridView1.DataSource];
            currencyManager.SuspendBinding();

            foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
            {
                if (dataGridViewRow.Index == (dataGridView1.Rows.Count - 1))
                {
                    // skip last row
                    // does not contain any value because it is used for adding new items
                    continue;
                }

                if (Directory.Exists(dataGridViewRow.Cells[2].Value.ToString()))
                {
                    dataGridViewRow.Visible = false;
                }
            }

            currencyManager.ResumeBinding();
        }

        private void intactAssociationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            brokenAssociationToolStripMenuItem.Enabled = false;
            checkToolStripMenuItem.Enabled = false;
            resetToolStripMenuItem.Enabled = true;

            CurrencyManager currencyManager = (CurrencyManager)BindingContext[dataGridView1.DataSource];
            currencyManager.SuspendBinding();

            foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
            {
                if (dataGridViewRow.Index == (dataGridView1.Rows.Count - 1))
                {
                    // skip last row
                    // does not contain any value because it is used for adding new items
                    continue;
                }

                if (!Directory.Exists(dataGridViewRow.Cells[2].Value.ToString()))
                {
                    dataGridViewRow.Visible = false;
                }
            }

            currencyManager.ResumeBinding();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            brokenAssociationToolStripMenuItem.Enabled = true;
            checkToolStripMenuItem.Enabled = true;
            intactAssociationToolStripMenuItem.Enabled = true;
            resetToolStripMenuItem.Enabled = false;
            
            CurrencyManager currencyManager = (CurrencyManager)BindingContext[dataGridView1.DataSource];
            currencyManager.SuspendBinding();

            foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
            {
                dataGridViewRow.Visible = true;
            }

            currencyManager.ResumeBinding();
        }
    }
}