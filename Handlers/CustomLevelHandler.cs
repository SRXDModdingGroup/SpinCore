using SMU.Reflection;
namespace SpinCore.Handlers
{
    public static class CustomLevelHandler
    {
        public static string UniqueNameToFileReference(string uniqueName)
        {
            if (uniqueName.Contains("CUSTOM_spinshare_"))
            {
                uniqueName = uniqueName.Replace("CUSTOM_spinshare_", "");
                
                return $"spinshare_{uniqueName.Split('_')[0]}";
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
            InstanceHandler.XDCustomLevelSelectMenuInstance.GetMetadataHandleForIndex(InstanceHandler.XDCustomLevelSelectMenuInstance.GetTrackIndexFromName(fileref)).TrackInfoRef.TryGetLoadedAsset(out var trackInfo);
            
            return trackInfo;
        }

        public static void DeleteChartFromFileRef(string fileref)
        {
            var trackInfo = GetTrackInfoFromFileRef(fileref);
            
            if (trackInfo != null)
            {
                var customTrackBundleSaveFile = trackInfo.CustomFile as CustomTrackBundleSaveFile;
                if (customTrackBundleSaveFile != null)
                {
                    customTrackBundleSaveFile.Delete();
                    CustomAssetLoadingHelper.Instance.RemoveFileNow(customTrackBundleSaveFile);
                    InstanceHandler.XDCustomLevelSelectMenuInstance.SelectedHandle = null;
                    InstanceHandler.XDCustomLevelSelectMenuInstance.PreviewHandle = null;
                }
            }
        }

        public static void PlayChartFromFileRef(string fileref, TrackData.DifficultyType difficulty)
        {
            var handle = InstanceHandler.XDCustomLevelSelectMenuInstance.GetMetadataHandleForIndex(InstanceHandler.XDCustomLevelSelectMenuInstance.GetTrackIndexFromName(fileref));
            var setup = new PlayableTrackDataSetup(handle.TrackInfoRef, handle.TrackDataRefForActiveIndex(handle.TrackDataMetadata.GetClosestActiveIndexForDifficulty(difficulty)), default(SetupParameters));
            GameStates.LoadIntoPlayingGameState.LoadHandleUserRequest(TrackLoadingSystem.Instance.BorrowHandle(setup));
        }
    }
}
