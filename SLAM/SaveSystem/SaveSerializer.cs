using UnityEngine;
using System.Collections;
using System.IO;

namespace SLAM.SaveSystem
{
    public class SaveSerializer : MonoBehaviour
    {
        private float saveInterval = 20f;
        private ISaveDataProvider saveDataProvider;
        private string savePath;

        private void Awake()
        {
            savePath = Path.Combine(Application.persistentDataPath, "savegame.dat");
            saveDataProvider = new BinarySaveDataProvider(savePath);
            // Load save data at startup
            SaveManager.Instance.Load(saveDataProvider);
        }

        private void Start()
        {
            StartCoroutine(AutoSaveRoutine());
        }

        private IEnumerator AutoSaveRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(saveInterval);
                TrySave();
            }
        }

        private void TrySave()
        {
            var manager = SaveManager.Instance;
            if (manager.IsDirty)
            {
                saveDataProvider.Save(manager.GetSaveData());
                manager.MarkClean();
            }
        }

        private void OnApplicationQuit()
        {
            // Ensure data is saved on quit
            TrySave();
        }
    }
}