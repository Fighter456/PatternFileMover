using Newtonsoft.Json;
using System.ComponentModel;

namespace PatternFileMover
{
    public enum NameAssociationCellIndex : int
    {
        Name = 0,
        SearchPattern = 1,
        Action = 2,
        TargetDirectory = 3,
        FileExension = 4,
    }

    internal class NameAssociationsData_v3
    {
        public string Name { get; set; }
        public string SearchPattern { get; set; }
        [JsonProperty(PropertyName = "Action", DefaultValueHandling = DefaultValueHandling.Include)]
        [DefaultValue(AvailableActions.Move)]
        public AvailableActions Action { get; set; }
        public string TargetDirectory { get; set; }
        [JsonProperty(PropertyName = "FileExtension", DefaultValueHandling = DefaultValueHandling.Include)]
        [DefaultValue("*.*")]
        public string FileExtension { get; set; }
    }
}
