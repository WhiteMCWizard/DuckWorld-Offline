using UnityEngine;
using System;
using SLAM.SaveSystem; 

namespace SLAM.SaveSystem;

public class SaveManager
{
    private static SaveManager _instance;
    public static SaveManager Instance => _instance ??= new SaveManager();

    private SaveData _saveData = null;
    public bool IsLoaded { get; private set; } = false;
    public bool IsDirty { get; private set; } = false;

    private SaveManager() { }

    public void Load(ISaveDataProvider provider)
    {
        if (IsLoaded)
            return;

        _saveData = provider.Load();
        if (_saveData == null)
        {
            InitializeNewSaveData();
        }
        IsLoaded = true;
    }

    private void EnsureLoaded()
    {
        if (!IsLoaded)
            throw new InvalidOperationException("SaveManager not loaded. Call Load() first.");
    }

    public void InitializeNewSaveData()
    {
        _saveData = new SaveData();
        IsDirty = true;
    }

    public SaveData GetSaveData()
    {
        EnsureLoaded();
        return _saveData;
    }

    public void SetSaveData(SaveData data)
    {
        _saveData = data;
        IsDirty = true;
    }

    public void MarkClean()
    {
        IsDirty = false;
    }

    public void Close()
    {
        IsLoaded = false;
    }
}