using System;
using System.IO;

namespace SpinCore.Handlers
{
    public class FilePathHandler
    {
        public static Action<string> HandleConfigs;

        public static string CustomFolderPath;
     
        public static string SpinCoreConfigName = "/SpinCoreConfig.json";



        public static void HandleConfig(string fileDirectory)
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
            SMU.Events.EventHelper.InvokeAll(HandleConfigs, fileDirectory + "/Mods");

        }

    }
}
