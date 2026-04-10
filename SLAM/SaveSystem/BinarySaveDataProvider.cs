using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SLAM.SaveSystem
{
    public class BinarySaveDataProvider : ISaveDataProvider
    {
        private readonly string _filePath;
        private string BackupPath => _filePath + ".bak";

        public BinarySaveDataProvider(string filePath)
        {
            _filePath = filePath;
        }

        public void Save(SaveData data)
        {
            try
            {
                // Create backup of existing save before overwriting
                if (File.Exists(_filePath))
                {
                    File.Copy(_filePath, BackupPath, overwrite: true);
                }

                using (FileStream fs = new FileStream(_filePath, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, data);
                }
                Debug.Log("Game saved successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save game data: {ex.Message}");
                // Attempt to restore from backup if save failed mid-write
                if (File.Exists(BackupPath) && !File.Exists(_filePath))
                {
                    try
                    {
                        File.Copy(BackupPath, _filePath);
                        Debug.Log("Restored save from backup after failed save.");
                    }
                    catch (Exception restoreEx)
                    {
                        Debug.LogError($"Failed to restore backup: {restoreEx.Message}");
                    }
                }
            }
        }

        public SaveData Load()
        {
            SaveData data = TryLoadFrom(_filePath);
            if (data != null) return data;

            // Primary save failed, try backup
            if (File.Exists(BackupPath))
            {
                Debug.LogWarning("Primary save file failed to load, trying backup...");
                data = TryLoadFrom(BackupPath);
                if (data != null)
                {
                    Debug.Log("Successfully loaded from backup save.");
                    return data;
                }
            }

            return null;
        }

        private SaveData TryLoadFrom(string path)
        {
            if (!File.Exists(path))
                return null;

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (SaveData)formatter.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load save data from {Path.GetFileName(path)}: {ex.Message}");
                return null;
            }
        }
    }
}
