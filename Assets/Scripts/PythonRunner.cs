using System.Diagnostics;
using UnityEngine;
using System.IO;

public class PythonRunner : MonoBehaviour
{
    private static PythonRunner instance;
    private Process pythonProcess;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Tu ruta fija de prueba en tu PC
        string pythonFolder = @"C:\Users\pablo\OneDrive\Desktop\Avance Teletón\UnityPythonMediaPipeBodyPose-main\mediapipebody";
        string pythonScriptPath = Path.Combine(pythonFolder, "main.py");

        if (!File.Exists(pythonScriptPath))
        {
            UnityEngine.Debug.LogError("No se encontró main.py en: " + pythonScriptPath);
            return;
        }

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "py",
            Arguments = "-3.9 \"" + pythonScriptPath + "\"",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = pythonFolder // 💡 hace el 'cd' automáticamente
        };

        pythonProcess = new Process();
        pythonProcess.StartInfo = startInfo;

        pythonProcess.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                UnityEngine.Debug.Log("[PY] " + e.Data);
        };
        pythonProcess.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                UnityEngine.Debug.LogError("[PY-ERR] " + e.Data);
        };

        pythonProcess.Start();
        pythonProcess.BeginOutputReadLine();
        pythonProcess.BeginErrorReadLine();
    }

    void OnApplicationQuit()
    {
        if (pythonProcess != null && !pythonProcess.HasExited)
        {
            pythonProcess.Kill();
            UnityEngine.Debug.Log("Python detenido");
        }
    }
}
