using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace PatternFileMover
{
    class NameAssociations
    {
        public static string _legacyConfigPath = Application.StartupPath + Path.DirectorySeparatorChar + "nameAssociations.json";
        public static string _configManifestPath = Application.StartupPath + Path.DirectorySeparatorChar + "manifest.json";
        public static string legacyConfigPath { get => _legacyConfigPath; }
        public static string configManifestPath { get => _configManifestPath; }

        public static bool CreateEmptyConfigFile()
        {
            try
            {
                File.WriteAllText(configManifestPath, "v2");
                File.WriteAllText(GetConfigFilePath(), JsonConvert.SerializeObject(new List<NameAssociationsData_v2>()));
            }
            catch (System.ArgumentException)
            {
                return false;
            }

            return true;
        }

        public static string GetConfigFilePath()
        {
            return Application.StartupPath +
                    Path.DirectorySeparatorChar +
                    "nameAssociation_" +
                    File.ReadAllText(Path.GetFullPath(configManifestPath)) +
                    ".json";
        }

        public static void checkAndUpgradeConfigurationFile() {
            string manifestVersion = File.ReadAllText(Path.GetFullPath(configManifestPath));

            if (!File.Exists(Path.GetFullPath(GetConfigFilePath())))
            { 
                // we could not find a configuration file that matches the config manifest
                // lets check if we find an outdated file that should be updated

                // version 1 (before implementing the file manifest)
                if (File.Exists(Path.GetFullPath(legacyConfigPath)))
                {
                    List<NameAssociationsData_v1> legacyValues = LoadFromExistingConfigFile_v1(legacyConfigPath);
                    List<NameAssociationsData_v2> convertedValues = new List<NameAssociationsData_v2>();

                    foreach (NameAssociationsData_v1 value in legacyValues) {
                        convertedValues.Add(new NameAssociationsData_v2()
                        {
                            Name = value.Name,
                            SearchPattern = value.SearchPattern,
                            TargetDirectory = value.TargetDirectory,
                            // version 1 only allows the processing of .pdf files
                            FileExtension = ".pdf"
                        });
                    }

                    WriteByList(convertedValues);
                }
            }
        }

        public static List<NameAssociationsData_v2> LoadFromExistingConfigFile() {
            return LoadFromExistingConfigFile_v2();
        }

        public static List<NameAssociationsData_v1> LoadFromExistingConfigFile_v1(string path = "")
        {
            if (String.IsNullOrEmpty(path))
            {
               path = GetConfigFilePath();
            }

            return JsonConvert.DeserializeObject<List<NameAssociationsData_v1>>(File.ReadAllText(Path.GetFullPath(path)));
        }

        public static List<NameAssociationsData_v2> LoadFromExistingConfigFile_v2(string path = "")
        {
            if (String.IsNullOrEmpty(path))
            {
               path = GetConfigFilePath();
            }

            return JsonConvert.DeserializeObject<List<NameAssociationsData_v2>>(File.ReadAllText(Path.GetFullPath(path)));
        }

        public static void WriteByList(List<NameAssociationsData_v2> list)
        {
            File.WriteAllText(
                GetConfigFilePath(),
                JsonConvert.SerializeObject(
                    list,
                    Formatting.None,
                    new JsonSerializerSettings { 
                        DefaultValueHandling = DefaultValueHandling.Include,
                    }
                )
            );
        }
    }
}
