using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SLAM.SaveSystem
{
    public class BinarySaveDataProvider : ISaveDataProvider
    {
        private readonly string _filePath;

        public BinarySaveDataProvider(string filePath)
        {
            _filePath = filePath;
        }

        public void Save(SaveData data)
        {
            using (FileStream fs = new FileStream(_filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, data);
            }
        }

        public SaveData Load()
        {
            Debug.Log($"Loading save data from {_filePath}");
            if (!File.Exists(_filePath))
                return null;

            try
            {
                using (FileStream fs = new FileStream(_filePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (SaveData)formatter.Deserialize(fs);
                }
            }
            catch
            {
                Debug.LogWarning("Failed to load save data, returning null.");
                return null;
            }
        }
    }
}
