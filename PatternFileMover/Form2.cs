using System;
using System.Windows.Forms;

namespace PatternFileMover
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            comboBox1.DataSource = AvailableActionItems.getAvailableActions();
            comboBox1.DisplayMember = "DisplayName";
            comboBox1.ValueMember = "Value";
        }

        public int? getValue(bool returnNullIfUnchanged = false, string currentValue = "")
        {
            if (returnNullIfUnchanged && !string.IsNullOrEmpty(currentValue))
            {
                if (!comboBox1.SelectedIndex.ToString().Equals(currentValue)) {
                    return comboBox1.SelectedIndex;
                }

                return null;
            }

            return comboBox1.SelectedIndex;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
