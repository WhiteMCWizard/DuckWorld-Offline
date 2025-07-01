using System;
using UnityEngine;

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
}