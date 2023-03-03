using Newtonsoft.Json;

namespace SpinCore.Utility; 

/// <summary>
/// Contains utility functions for accessing custom charts
/// </summary>
public static class CustomChartUtility {
    /// <summary>
    /// Selects the chart with a given file name
    /// </summary>
    /// <param name="fileName">The file name of the chart to select</param>
    public static void SelectChart(string fileName) {
        if (!XDSelectionListMenu.ShouldShowMenu)
            return;
        
        var handle = GetHandleForFile(fileName);
        
        if (handle != null)
            XDSelectionListMenu.Instance.ScrollToTrack(handle);
    }

    /// <summary>
    /// Deletes the chart with a given file name
    /// </summary>
    /// <param name="fileName">The file name of the chart to delete</param>
    public static void DeleteChart(string fileName) {
        var customFile = GetCustomFile(fileName);
        
        if (customFile != null)
            CustomAssetLoadingHelper.Instance.RemoveFileNow(customFile);
    }

    /// <summary>
    /// Plays the chart with a given file name
    /// </summary>
    /// <param name="fileName">The file name of the chart to play</param>
    /// <param name="difficulty">The difficulty type of the chart to play</param>
    public static void PlayChart(string fileName, TrackData.DifficultyType difficulty) {
        var setup = GetHandleForFile(fileName)?.GetSetupForDifficulty(difficulty);
        
        if (setup != null)
            GameStates.LoadIntoPlayingGameState.LoadTrack(setup);
    }

    /// <summary>
    /// Saves miscellaneous data to a chart file 
    /// </summary>
    /// <param name="customFile">The file to write to</param>
    /// <param name="key">The key used to identify the data</param>
    /// <param name="data">The data to write</param>
    /// <param name="save">Save the file immediately</param>
    public static void SetCustomData(CustomTrackBundleSaveFile customFile, string key, object data, bool save = false) {
        customFile.GetLargeStringOrJson(key).Value = JsonConvert.SerializeObject(data);
        customFile.MarkDirty();
        
        if (save)
            customFile.WriteToDiskIfDirty(true);
    }

    /// <summary>
    /// Removes miscellaneous data from a chart file
    /// </summary>
    /// <param name="customFile">The file to remove data from</param>
    /// <param name="key">The key used to identify the data</param>
    /// <param name="save">Save the file immediately</param>
    public static void RemoveCustomData(CustomTrackBundleSaveFile customFile, string key, bool save = false) {
        if (!customFile.HasJsonValueForKey(key))
            return;
        
        customFile.RemoveJsonValue(key);
        customFile.MarkDirty();
        
        if (save)
            customFile.WriteToDiskIfDirty(true);
    }

    /// <summary>
    /// Attempts to get miscellaneous data from a chart file
    /// </summary>
    /// <param name="customFile">The file to read from</param>
    /// <param name="key">The key used to identify the data</param>
    /// <param name="data">The acquired data</param>
    /// <typeparam name="T">The type of the data object</typeparam>
    /// <returns>True if data was found</returns>
    public static bool TryGetCustomData<T>(CustomTrackBundleSaveFile customFile, string key, out T data) {
        if (!customFile.HasJsonValueForKey(key)) {
            data = default;

            return false;
        }
        
        data = JsonConvert.DeserializeObject<T>(customFile.GetLargeStringOrJson(key).Value);

        return data is not null;
    }

    /// <summary>
    /// Gets the track info metadata for the chart with a given file name
    /// </summary>
    /// <param name="fileName">The file name of the chart</param>
    /// <returns>The track info for the chart</returns>
    public static TrackInfoMetadata GetTrackInfoMetadata(string fileName) => GetHandleForFile(fileName)?.TrackInfoMetadata;

    /// <summary>
    /// Gets the custom file for the chart with a given file name
    /// </summary>
    /// <param name="fileName">The file name of the chart</param>
    /// <returns>The custom file for the chart</returns>
    public static CustomTrackBundleSaveFile GetCustomFile(string fileName) {
        var trackInfoRef = GetHandleForFile(fileName)?.TrackInfoRef;

        if (trackInfoRef == null || !trackInfoRef.IsCustomFile || trackInfoRef.customFile is not CustomTrackBundleSaveFile customTrackBundleSaveFile)
            return null;
        
        return customTrackBundleSaveFile;
    }
    
    private static MetadataHandle GetHandleForFile(string fileName) {
        var saveFile = CustomAssetLoadingHelper.FindBundleForPath(fileName);

        if (saveFile == null)
            return null;

        return TrackLoadingSystem.Instance.PeekMetadataHandle(CustomAssetLoadingHelper.GetTrackInfoKeyForFile(saveFile));
    }
}