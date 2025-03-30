using Newtonsoft.Json;
using System.ComponentModel;

namespace PatternFileMover
{
    internal class NameAssociationsData_v1
    {
        [System.ComponentModel.DisplayName("Name")]
        public string Name { get; set; }
        [System.ComponentModel.DisplayName("Suchmuster")]
        public string SearchPattern { get; set; }
        [System.ComponentModel.DisplayName("Ziel-Pfad")]
        public string TargetDirectory { get; set; }
    }
}
