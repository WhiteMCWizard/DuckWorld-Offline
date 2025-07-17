using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    private StreamWriter logWriter;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        string logPath = Path.Combine(Application.persistentDataPath, "player.log");

        // Delete the log file if it already exists
        if (File.Exists(logPath))
        {
            File.Delete(logPath);
        }

        logWriter = new StreamWriter(logPath, false); // 'false' to overwrite just in case
        logWriter.AutoFlush = true;

        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logWriter.WriteLine($"[{type}] {logString}");
        if (type == LogType.Exception || type == LogType.Error)
        {
            logWriter.WriteLine(stackTrace);
        }
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
        logWriter?.Close();
    }
}