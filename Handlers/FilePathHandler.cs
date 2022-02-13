using System.IO;

namespace SpinCore.Handlers
{
    public static class FilePathHandler {
        public static string CustomFolderPath { get; private set; }
     
        internal static string SpinCoreConfigName { get; } = "/SpinCoreConfig.json";

        internal static void Init(string fileDirectory)
        {
            CustomFolderPath = fileDirectory;
            
            //SpinCore.Logger.LogMessage(configFolderPath);

            if (!Directory.Exists(fileDirectory + "/Mods"))
            {
                Directory.CreateDirectory(fileDirectory + "/Mods");
            }

            if (!Directory.Exists(fileDirectory + "/TempSongDownload"))
            {
                Directory.CreateDirectory(fileDirectory + "/TempSongDownload");
            }

            if (!Directory.Exists(fileDirectory + "/Mods/SpinCore"))
            {
                Directory.CreateDirectory(fileDirectory + "/Mods/SpinCore");
            }
        }
    }
}
