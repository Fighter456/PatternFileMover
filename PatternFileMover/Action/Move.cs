using System.IO;
using System.Windows.Forms;

namespace PatternFileMover.Action
{
    internal class Move : AbstractAction
    {
        public bool DoMove() {
            File.Move(
                this.GetSourcePath(),
                this.GetTargetPath() +
                    Path.DirectorySeparatorChar +
                    Path.GetFileName(this.GetCurrent().Cells[0].Value.ToString()
                )
            );

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

        public override void Prepare()
        {
            base.Prepare();

            if (File.Exists(
                    this.GetTargetPath() +
                    Path.DirectorySeparatorChar +
                    Path.GetFileName(this.GetCurrent().Cells[0].Value.ToString())
                )
            )
            {
                // delete the existing file before it gets replaced with the current
                // processed file
                File.Delete(
                    this.GetTargetPath() +
                    Path.DirectorySeparatorChar +
                    Path.GetFileName(this.GetCurrent().Cells[0].Value.ToString())
                );
            }
        }
    }
}
