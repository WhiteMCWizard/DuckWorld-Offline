using UnityEngine;
using System.Collections;
using System.IO;

namespace SLAM.SaveSystem;

public class SaveSerializer : MonoBehaviour
{
    private float saveInterval = 20f;

    private void Awake()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "savegame.dat");
        ISaveDataProvider saveDataProvider = new BinarySaveDataProvider(savePath);
        SaveManager.Instance.Initialize(saveDataProvider);
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
            SaveManager.Instance.Save();
        }
    }

    private void OnApplicationQuit()
    {
        SaveManager.Instance.Save();
    }
}