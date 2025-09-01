using System.IO;
using System.Windows.Forms;

namespace PatternFileMover.Action
{
    internal class Delete : AbstractAction
    {
        public bool DoDelete()
        {
            try
            {
                File.Delete(this.GetSourcePath());
            }
            catch (IOException)
            {
                return false;
            }

            return true;
        }

        public override bool ExecuteAction()
        {
            return base.ExecuteAction();
        }

        public override bool Matches(DataGridViewRow row)
        {
            if (!base.Matches(row))
            {
                return false;
            }

            if (
                    (
                        this.GetFileExtension() == "*.*" &&
                        Path.GetFileNameWithoutExtension(
                            this.GetCurrent().Cells[(int)NameAssociationCellIndex.Name].Value.ToString()
                        ).Contains(this.GetSearchPattern())
                    ) ||
                    (
                       Path.GetFileNameWithoutExtension(
                           this.GetCurrent().Cells[(int)NameAssociationCellIndex.Name].Value.ToString()
                       ).Contains(this.GetSearchPattern()) &&
                       this.GetFileExtension() == Path.GetExtension(this.GetCurrent().Cells[(int)NameAssociationCellIndex.Name].Value.ToString())
                    )
                )
            {
                return true;
            }

            return false;
        }
    }
}
