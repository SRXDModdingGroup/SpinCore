using System.Text.RegularExpressions;
using SMU.Reflection;

namespace SpinCore.Handlers
{
    public static class CustomLevelHandler {
        private static readonly Regex MATCH_CUSTOM_REFERENCE = new Regex("CUSTOM_(.+)_.+");
        
        public static string UniqueNameToFileReference(string uniqueName) {
            var match = MATCH_CUSTOM_REFERENCE.Match(uniqueName);
            
            if (match.Success)
                return match.Groups[1].Value;

            return uniqueName;
        }

        public static void OpenChartFromFileRef(string fileRef)
        {
            InstanceHandler.XDCustomLevelSelectMenu.GetField<XDLevelSelectMenuBase, GenericWheelInput>("_wheelInput").SetPosition(InstanceHandler.XDCustomLevelSelectMenu.GetTrackIndexFromName(fileRef));
            InstanceHandler.XDCustomLevelSelectMenu.SetField<XDLevelSelectMenuBase, bool>("SnapToTrack", true);
            InstanceHandler.XDCustomLevelSelectMenu.SetField<XDLevelSelectMenuBase, MetadataHandle>("trackToIndexToAfterSortingOrFiltering", InstanceHandler.XDCustomLevelSelectMenu.WillLandAtHandle);
        }

        public static TrackInfo GetTrackInfoFromFileRef(string fileRef)
        {
            InstanceHandler.XDCustomLevelSelectMenu.GetMetadataHandleForIndex(InstanceHandler.XDCustomLevelSelectMenu.GetTrackIndexFromName(fileRef)).TrackInfoRef.TryGetLoadedAsset(out var trackInfo);
            
            return trackInfo;
        }

        public static void DeleteChartFromFileRef(string fileRef)
        {
            var trackInfo = GetTrackInfoFromFileRef(fileRef);

            if (trackInfo == null || !trackInfo.IsCustom || !(trackInfo.CustomFile is CustomTrackBundleSaveFile customTrackBundleSaveFile))
                return;
            
            customTrackBundleSaveFile.Delete();
            CustomAssetLoadingHelper.Instance.RemoveFileNow(customTrackBundleSaveFile);
            InstanceHandler.XDCustomLevelSelectMenu.SelectedHandle = null;
            InstanceHandler.XDCustomLevelSelectMenu.PreviewHandle = null;
        }

        public static void PlayChartFromFileRef(string fileRef, TrackData.DifficultyType difficulty)
        {
            var handle = InstanceHandler.XDCustomLevelSelectMenu.GetMetadataHandleForIndex(InstanceHandler.XDCustomLevelSelectMenu.GetTrackIndexFromName(fileRef));
            var setup = new PlayableTrackDataSetup(handle.TrackInfoRef, handle.TrackDataRefForActiveIndex(handle.TrackDataMetadata.GetClosestActiveIndexForDifficulty(difficulty)), default);
            
            GameStates.LoadIntoPlayingGameState.LoadHandleUserRequest(TrackLoadingSystem.Instance.BorrowHandle(setup));
        }
    }
}
