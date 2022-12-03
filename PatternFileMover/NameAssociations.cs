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
    }
}
