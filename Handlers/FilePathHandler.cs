using System;
using System.IO;

namespace SpinCore.Handlers
{
    public class FilePathHandler
    {
        public static Action<string> handleConfigs;

        public static string customFolderPath;
     
        public static string SpinCoreConfigName = "/SpinCoreConfig.json";



        public static void HandleConfig(string fileDirectory)
        {
            customFolderPath = fileDirectory;



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
            SMU.Events.EventHelper.InvokeAll(handleConfigs, fileDirectory + "/Mods");

        }

    }
}
