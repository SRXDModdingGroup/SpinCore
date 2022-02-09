using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMU.Reflection;
namespace SpinCore.Handlers
{
    public class CustomLevelHandler
    {

        public static string UniqueNametoFileReference(string uniqueName)
        {
            if (uniqueName.Contains("CUSTOM_spinshare_"))
            {
                uniqueName = uniqueName.Replace("CUSTOM_spinshare_", "");
                return "spinshare_" + uniqueName.Split('_')[0];
            }
            return "";
        }

        public static void OpenCustomChartFromFileRef(string fileref)
        {
            InstanceHandler.XDCustomLevelSelectMenuInstance.GetField<XDLevelSelectMenuBase, GenericWheelInput>("_wheelInput").SetPosition(InstanceHandler.XDCustomLevelSelectMenuInstance.GetTrackIndexFromName(fileref));
            InstanceHandler.XDCustomLevelSelectMenuInstance.SetField<XDLevelSelectMenuBase, bool>("SnapToTrack", true);
            InstanceHandler.XDCustomLevelSelectMenuInstance.SetField<XDLevelSelectMenuBase, MetadataHandle>("trackToIndexToAfterSortingOrFiltering", InstanceHandler.XDCustomLevelSelectMenuInstance.WillLandAtHandle);
        }

        public static TrackInfo GetTrackInfoFromFileRef(string fileref)
        {
            TrackInfo trackInfo = null;
            InstanceHandler.XDCustomLevelSelectMenuInstance.GetMetadataHandleForIndex(InstanceHandler.XDCustomLevelSelectMenuInstance.GetTrackIndexFromName(fileref)).TrackInfoRef.TryGetLoadedAsset(out trackInfo);
            return trackInfo;
        }


        public static void DeleteChartFromFileRef(string fileref)
        {
            TrackInfo trackInfo = GetTrackInfoFromFileRef(fileref);
            if (trackInfo != null)
            {
                CustomTrackBundleSaveFile customTrackBundleSaveFile = trackInfo.CustomFile as CustomTrackBundleSaveFile;
                if (customTrackBundleSaveFile != null)
                {
                    customTrackBundleSaveFile.Delete(false);
                    CustomAssetLoadingHelper.Instance.RemoveFileNow(customTrackBundleSaveFile);
                    InstanceHandler.XDCustomLevelSelectMenuInstance.SelectedHandle = null;
                    InstanceHandler.XDCustomLevelSelectMenuInstance.PreviewHandle = null;
                }
            }

        }

        


        public static void PlayChartFromFileRef(string fileref, TrackData.DifficultyType difficulty)
        {
            MetadataHandle handle = InstanceHandler.XDCustomLevelSelectMenuInstance.GetMetadataHandleForIndex(InstanceHandler.XDCustomLevelSelectMenuInstance.GetTrackIndexFromName(fileref));
            PlayableTrackDataSetup setup = new PlayableTrackDataSetup(handle.TrackInfoRef, handle.TrackDataRefForActiveIndex(handle.TrackDataMetadata.GetClosestActiveIndexForDifficulty(difficulty)), default(SetupParameters), null);
            GameStates.LoadIntoPlayingGameState.LoadHandleUserRequest(TrackLoadingSystem.Instance.BorrowHandle(setup), true);
            
            
        }
    }
}
