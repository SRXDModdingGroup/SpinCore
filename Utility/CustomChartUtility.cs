using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SMU.Reflection;
using SpinCore.Handlers;

namespace SpinCore.Utility; 

/// <summary>
/// Contains utility functions for accessing custom levels
/// </summary>
public static class CustomChartUtility {
    private static readonly Regex MATCH_CUSTOM_REFERENCE = new("CUSTOM_(.+)_.+");
        
    /// <summary>
    /// Extracts the file name of a level from its unique name
    /// </summary>
    /// <param name="uniqueName">The unique name of the level</param>
    /// <returns>The file name of the level</returns>
    public static string UniqueNameToFileReference(string uniqueName) {
        var match = MATCH_CUSTOM_REFERENCE.Match(uniqueName);
            
        if (match.Success)
            return match.Groups[1].Value;

        return uniqueName;
    }

    /// <summary>
    /// Selects the chart with a given file name
    /// </summary>
    /// <param name="fileRef">The file name of the chart to select</param>
    public static void SelectChartFromFileRef(string fileRef)
    {
        InstanceHandler.XDCustomLevelSelectMenu.GetField<XDLevelSelectMenuBase, GenericWheelInput>("_wheelInput").SetPosition(InstanceHandler.XDCustomLevelSelectMenu.GetTrackIndexFromName(fileRef));
        InstanceHandler.XDCustomLevelSelectMenu.SetField<XDLevelSelectMenuBase, bool>("SnapToTrack", true);
        InstanceHandler.XDCustomLevelSelectMenu.SetField<XDLevelSelectMenuBase, MetadataHandle>("trackToIndexToAfterSortingOrFiltering", InstanceHandler.XDCustomLevelSelectMenu.WillLandAtHandle);
    }

    /// <summary>
    /// Gets the track info for the chart with a given file name
    /// </summary>
    /// <param name="fileRef">The file name of the chart</param>
    /// <returns>The track info for the chart</returns>
    public static TrackInfo GetTrackInfoFromFileRef(string fileRef)
    {
        InstanceHandler.XDCustomLevelSelectMenu.GetMetadataHandleForIndex(InstanceHandler.XDCustomLevelSelectMenu.GetTrackIndexFromName(fileRef)).TrackInfoRef.TryGetLoadedAsset(out var trackInfo);
            
        return trackInfo;
    }

    /// <summary>
    /// Deletes the chart with a given file name
    /// </summary>
    /// <param name="fileRef">The file name of the chart to delete</param>
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

    /// <summary>
    /// Plays the chart with a given file name
    /// </summary>
    /// <param name="fileRef">The file name of the chart to play</param>
    /// <param name="difficulty">The difficulty type of the chart to play</param>
    public static void PlayChartFromFileRef(string fileRef, TrackData.DifficultyType difficulty)
    {
        var handle = InstanceHandler.XDCustomLevelSelectMenu.GetMetadataHandleForIndex(InstanceHandler.XDCustomLevelSelectMenu.GetTrackIndexFromName(fileRef));
        var setup = new PlayableTrackDataSetup(handle.TrackInfoRef, handle.TrackDataRefForActiveIndex(handle.TrackDataMetadata.GetClosestActiveIndexForDifficulty(difficulty)), default);
            
        GameStates.LoadIntoPlayingGameState.LoadHandleUserRequest(TrackLoadingSystem.Instance.BorrowHandle(setup));
    }

    /// <summary>
    /// Saves miscellaneous data to a chart file 
    /// </summary>
    /// <param name="customFile">The file to write to</param>
    /// <param name="modData">The data to write</param>
    /// <param name="save">Save the file immediately</param>
    public static void SetModData(IMultiAssetSaveFile customFile, SRTBModData modData, bool save = false) {
        customFile.GetLargeStringOrJson("ModData").Value = JsonConvert.SerializeObject(modData);
        customFile.MarkDirty();
        
        if (save)
            customFile.WriteToDiskIfDirty(true);
    }

    /// <summary>
    /// Gets miscellaneous data from a chart file
    /// </summary>
    /// <param name="customFile">The file to read from</param>
    /// <returns>The acquired info</returns>
    public static SRTBModData GetModData(IMultiAssetSaveFile customFile) {
        if (!customFile.HasJsonValueForKey("ModData"))
            return new SRTBModData();

        var modData = JsonConvert.DeserializeObject<SRTBModData>(customFile.GetLargeStringOrJson("ModData").Value);

        if (modData is { Empty: false })
            return modData;

        return new SRTBModData();
    }
}