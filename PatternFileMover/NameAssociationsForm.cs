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
            dataGridView1.RowHeadersVisible = false;

            var dataList = new BindingList<NameAssociationsData>(NameAssociations.LoadFromExistingConfigFile());
            var dataSource = new BindingSource(dataList, null);
            dataGridView1.DataSource = dataSource;

            dataGridView1.AutoResizeColumns();
        }
    }
}
