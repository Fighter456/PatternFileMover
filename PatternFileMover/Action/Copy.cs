using System.IO;
using System.Windows.Forms;

namespace PatternFileMover.Action
{
    internal class Copy : AbstractAction
    {
        public bool DoCopy()
        {
            try
            {
                File.Copy(
                    this.GetSourcePath(),
                    this.GetTargetPath() +
                        Path.DirectorySeparatorChar +
                        Path.GetFileName(this.GetCurrent().Cells[0].Value.ToString()
                    )
                );
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
                    ) &&
                        (
                            Directory.Exists(this.GetTargetPath() + Path.DirectorySeparatorChar)
                        )
                )
            {
                return true;
            }

            return false;
        }
    }
}
