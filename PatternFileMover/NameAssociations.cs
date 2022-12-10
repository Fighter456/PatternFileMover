using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace PatternFileMover
{
    class NameAssociations
    {
        public static string _configPath = Application.StartupPath + Path.DirectorySeparatorChar + "nameAssociations.json";

        public static string configPath { get => _configPath; }

        public static bool CreateEmptyConfigFile()
        {
            try
            {
                File.WriteAllText(configPath, JsonConvert.SerializeObject(new List<NameAssociationsData>()));
            }
            catch (System.ArgumentException)
            {
                return false;
            }

            return true;
        }

        public static List<NameAssociationsData> LoadFromExistingConfigFile()
        {
            return JsonConvert.DeserializeObject<List<NameAssociationsData>>(File.ReadAllText(Path.GetFullPath(configPath)));
        }

        public static void WriteByList(List<NameAssociationsData> list)
        {
            File.WriteAllText(configPath, JsonConvert.SerializeObject(list));
        }
    }
}
