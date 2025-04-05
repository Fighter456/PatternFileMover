using Newtonsoft.Json;
using System.ComponentModel;

namespace PatternFileMover
{
    internal class NameAssociationsData_v2
    {
        public string Name { get; set; }
        public string SearchPattern { get; set; }
        public string TargetDirectory { get; set; }
        [JsonProperty(PropertyName = "FileExtension", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("*.*")]
        public string FileExtension { get; set; }
    }
}
