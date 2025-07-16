using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SLAM.KartRacing;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace SLAM.SaveSystem;

public class SaveManager
{
    private static SaveManager _instance;
    public static SaveManager Instance => _instance ??= new SaveManager();

    public event Action OnDataLoaded;

    private SaveData _saveData;
    private ISaveDataProvider saveDataProvider;
    public bool IsLoaded { get; private set; }
    public bool IsDirty { get; private set; }

    private SaveManager() { }

    public void Initialize(ISaveDataProvider provider)
    {
        if (saveDataProvider != null)
        {
            Debug.LogWarning("SaveManager is already initialized.");
            return;
        }
        saveDataProvider = provider;
        Load();
    }

    private void Load()
    {
        _saveData = saveDataProvider.Load();
        if (_saveData == null)
        {
            _saveData = new SaveData();
            Debug.Log("No save data found, creating new save data.");
            IsDirty = true; // New data should be saved at least once.
        }
        IsLoaded = true;
        OnDataLoaded?.Invoke();
    }

    public void Save()
    {
        if (saveDataProvider != null && IsDirty)
        {
            saveDataProvider.Save(_saveData);
            IsDirty = false;
        }
    }

    public SaveData GetSaveData()
    {
        if (!IsLoaded)
        {
            Debug.LogError("Save data is not loaded yet!");
            return null;
        }
        return _saveData;
    }

    public void MarkDirty()
    {
        if (IsLoaded)
        {
            IsDirty = true;
        }
    }

    public void SaveTextureToFile(Texture2D texture, string filePath)
    {
        if (texture == null)
        {
            Debug.LogError("Cannot save null texture.");
            return;
        }

        byte[] bytes = texture.EncodeToPNG();
        if (bytes == null || bytes.Length == 0)
        {
            Debug.LogError("Failed to encode texture to PNG.");
            return;
        }

        try
        {
            string fullPath = System.IO.Path.Combine(Application.persistentDataPath, filePath);
            System.IO.File.WriteAllBytes(fullPath, bytes);
            Debug.Log($"Texture saved to {filePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save texture: {ex.Message}");
        }
    }

    public void SaveGhost(int gameId, int difficulty, int elapsedMilliseconds, GhostRecordingData recording)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();
        binaryFormatter.Serialize(memoryStream, recording);
        string directoryPath = Path.Combine(Application.persistentDataPath, "ghosts");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        long unixTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        string fileName = $"ghost_{gameId}_{difficulty}_{elapsedMilliseconds}_{unixTimestamp}.dat";
        string fullPath = Path.Combine(directoryPath, fileName);
        try
        {
            File.WriteAllBytes(fullPath, memoryStream.ToArray());
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save ghost recording: {ex.Message}");
        }
    }

    public void GetBestGhost(int gameId, int difficulty, Action<GhostRecordingData> callback)
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, "ghosts");
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogWarning("No ghost recordings found.");
            callback?.Invoke(default);
            return;
        }

        string[] files = Directory.GetFiles(directoryPath, "ghost_*.dat");
        if (files.Length == 0)
        {
            Debug.LogWarning("No ghost recordings found.");
            callback?.Invoke(default);
            return;
        }

        var map11to39 = new Dictionary<int, int>
        {
            {1, 1}, {2, 1}, {3, 2}, {4, 3}, {5, 3}, {6, 4}, {7, 4}, {8, 5}, {9, 6}, {10, 6}
        };

        var bestFile = files
            .Select(file =>
            {
                var parts = Path.GetFileNameWithoutExtension(file).Split('_');
                if (parts.Length < 4) return null;

                if (!int.TryParse(parts[1], out int fileGameId)) return null;
                if (!int.TryParse(parts[2], out int fileDifficulty)) return null;
                if (!int.TryParse(parts[3], out int elapsed)) return null;

                var mappedDifficulty = map11to39.FirstOrDefault(x => x.Key == fileDifficulty).Value;

                bool isRelevant =
                    (fileGameId == 39 && fileDifficulty == difficulty) ||
                    (fileGameId == 11 && mappedDifficulty == difficulty);

                return isRelevant ? new { File = file, Elapsed = elapsed } : null;
            })
            .Where(x => x != null)
            .OrderBy(x => x.Elapsed)
            .FirstOrDefault();

        if (bestFile == null)
        {
            Debug.LogWarning("No valid ghost recordings found.");
            callback?.Invoke(default);
            return;
        }
        
        try
        {
            using var fs = File.OpenRead(bestFile.File);
            var formatter = new BinaryFormatter();
            var data = (GhostRecordingData)formatter.Deserialize(fs);
            callback?.Invoke(data);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load ghost recording: {ex.Message}");
            callback?.Invoke(default);
        }
    }
}