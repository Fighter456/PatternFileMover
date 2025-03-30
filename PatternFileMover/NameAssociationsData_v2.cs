using Newtonsoft.Json;
using System.ComponentModel;

namespace PatternFileMover
{
    internal class NameAssociationsData_v2
    {
        [System.ComponentModel.DisplayName("Name")]
        public string Name { get; set; }
        [System.ComponentModel.DisplayName("Suchmuster")]
        public string SearchPattern { get; set; }
        [System.ComponentModel.DisplayName("Ziel-Pfad")]
        public string TargetDirectory { get; set; }
        [System.ComponentModel.DisplayName("Dateiendung")]
        [JsonProperty(PropertyName = "FileExtension", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("*.*")]
        public string FileExtension { get; set; }
    }
}
