using System.IO;
using UnityEditor.Overlays;
using UnityEngine;
using Event = UnityEngine.Events.UnityEvent;
public class SaveLoad : MonoBehaviour
{
    public static SaveLoad Instance;
    public string user;
    public string filePath;
    public string directoryPath;
    public Event OnSaved;
    public Event OnLoaded;
    private void Awake()
    {
        Instance = this;
        directoryPath = $"{Application.persistentDataPath}/{user}";
        if (!Directory.Exists(directoryPath))Directory.CreateDirectory(directoryPath);
        filePath = $"{directoryPath}/{user}_Save.json";
    }
    public void SaveGame() => OnSaved?.Invoke();
    public void LoadGame() => OnLoaded?.Invoke();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) 
        {
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.F9)) 
        {
            LoadGame();
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            Application.OpenURL(Application.persistentDataPath);
        }
    }
}