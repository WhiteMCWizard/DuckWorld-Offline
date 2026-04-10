using System;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    private StreamWriter logWriter;
    private string logPath;
    private const long MaxLogSize = 1024 * 1024; // 1 MB
    private const int MaxLogFiles = 3;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        logPath = Path.Combine(Application.persistentDataPath, "player.log");

        RotateLogsIfNeeded();
        logWriter = new StreamWriter(logPath, append: true);
        logWriter.AutoFlush = true;
        logWriter.WriteLine($"--- Session started at {DateTime.Now:yyyy-MM-dd HH:mm:ss} ---");

        Application.logMessageReceived += HandleLog;
    }

    private void RotateLogsIfNeeded()
    {
        if (!File.Exists(logPath))
            return;

        try
        {
            var info = new FileInfo(logPath);
            if (info.Length < MaxLogSize)
                return;

            // Rotate: delete oldest, shift others
            string oldest = logPath.Replace(".log", $".{MaxLogFiles - 1}.log");
            if (File.Exists(oldest))
                File.Delete(oldest);

            for (int i = MaxLogFiles - 2; i >= 1; i--)
            {
                string src = logPath.Replace(".log", $".{i}.log");
                string dst = logPath.Replace(".log", $".{i + 1}.log");
                if (File.Exists(src))
                    File.Move(src, dst);
            }

            string first = logPath.Replace(".log", ".1.log");
            File.Move(logPath, first);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Log rotation failed: {ex.Message}");
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (logWriter == null) return;

        logWriter.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{type}] {logString}");
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