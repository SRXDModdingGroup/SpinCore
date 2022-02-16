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
            InstanceHandler.XDCustomLevelSelectMenu.GetField<XDLevelSelectMenuBase, GenericWheelInput>("_wheelInput").SetPosition(InstanceHandler.XDCustomLevelSelectMenu.GetTrackIndexFromName(fileref));
            InstanceHandler.XDCustomLevelSelectMenu.SetField<XDLevelSelectMenuBase, bool>("SnapToTrack", true);
            InstanceHandler.XDCustomLevelSelectMenu.SetField<XDLevelSelectMenuBase, MetadataHandle>("trackToIndexToAfterSortingOrFiltering", InstanceHandler.XDCustomLevelSelectMenu.WillLandAtHandle);
        }

        public static TrackInfo GetTrackInfoFromFileRef(string fileref)
        {
            InstanceHandler.XDCustomLevelSelectMenu.GetMetadataHandleForIndex(InstanceHandler.XDCustomLevelSelectMenu.GetTrackIndexFromName(fileref)).TrackInfoRef.TryGetLoadedAsset(out var trackInfo);
            
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
                    InstanceHandler.XDCustomLevelSelectMenu.SelectedHandle = null;
                    InstanceHandler.XDCustomLevelSelectMenu.PreviewHandle = null;
                }
            }
        }

        public static void PlayChartFromFileRef(string fileref, TrackData.DifficultyType difficulty)
        {
            var handle = InstanceHandler.XDCustomLevelSelectMenu.GetMetadataHandleForIndex(InstanceHandler.XDCustomLevelSelectMenu.GetTrackIndexFromName(fileref));
            var setup = new PlayableTrackDataSetup(handle.TrackInfoRef, handle.TrackDataRefForActiveIndex(handle.TrackDataMetadata.GetClosestActiveIndexForDifficulty(difficulty)), default(SetupParameters));
            GameStates.LoadIntoPlayingGameState.LoadHandleUserRequest(TrackLoadingSystem.Instance.BorrowHandle(setup));
        }
    }
}
