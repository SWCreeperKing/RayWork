using System.Diagnostics;
using System.Reflection;
using static System.Environment;
using static System.Environment.SpecialFolder;

namespace RayWork.SaveSystem;

public static class SaveSystem
{
    public static readonly string CoreDir = GetFolderPath(LocalApplicationData);

    public static bool SaveSystemInitialized { get; private set; }
    public static string? DeveloperName { get; private set; }
    public static string ApplicationName { get; private set; } = "Unknown App";
    public static string? SaveDirectory { get; private set; }
    public static Encryptor? Encryption { get; set; } = null;

    private static readonly List<ISavable> Savables = new();

    public static void InitializeSaveSystem(string developerName, string applicationName)
    {
        if (SaveSystemInitialized) throw new ApplicationException("Save System can only be initialized ONCE!");
        DeveloperName = developerName;
        ApplicationName = applicationName;
        SaveDirectory = $"{CoreDir}/{DeveloperName}/{ApplicationName}";
        SaveSystemInitialized = true;
    }

    public static void SaveSystemCheck()
    {
        if (SaveSystemInitialized) return;
        throw new ApplicationException("Save System has not been initialized!");
    }

    /// <summary>
    /// REMEMBER: objects need to keep references otherwise loaded items will fail to update
    /// </summary>
    public static void AddSaveItem<TSaveType>(string fileName, TSaveType saveType)
    {
        var savableObject = new SavableObject<TSaveType>(saveType, fileName);
        Savables.Add(savableObject);
    }

    public static void SaveItems()
    {
        if (!Directory.Exists(SaveDirectory)) Directory.CreateDirectory(SaveDirectory!);

        foreach (var savable in Savables)
        {
            var data = savable.SaveString();
            if (Encryption is not null) data = Encryption.Encrypt(data);

            using var sw = File.CreateText($"{SaveDirectory}/{savable.GetFileName()}.json");
            sw.Write(data);
            sw.Close();
        }
    }

    public static void LoadItems()
    {
        foreach (var savable in Savables)
        {
            var file = $"{SaveDirectory}/{savable.GetFileName()}.json";
            if (!File.Exists(file)) continue;

            using StreamReader sr = new(file);
            var data = sr.ReadToEnd();
            sr.Close();

            if (Encryption is not null) data = Encryption.Decrypt(data);
            savable.LoadString(data, file);
        }
    }

    public static void OpenDirectory() => Process.Start("explorer.exe", $@"{SaveDirectory}".Replace("/", "\\"));

    /// <summary>
    /// this is basically = but it doesn't not reset the original reference
    /// </summary>
    /// <param name="t">original</param>
    /// <param name="overrider">new</param>
    /// <typeparam name="T">any type</typeparam>
    public static void Set<T>(this T t, T overrider)
    {
        foreach (var field in typeof(T).GetRuntimeFields().Where(f => !f.IsStatic))
        {
            try
            {
                field.SetValue(t, field.GetValue(overrider));
            }
            catch (TargetException)
            {
                throw new ArgumentException("Item corruption possibly detected");
            }
        }
    }
}