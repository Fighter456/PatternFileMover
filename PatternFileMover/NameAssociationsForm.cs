using System;
using System.ComponentModel;
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
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            var dataList = new BindingList<NameAssociationsData>(NameAssociations.LoadFromExistingConfigFile());
            var dataSource = new BindingSource(dataList, null);
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
    }
}
