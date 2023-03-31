using Newtonsoft.Json;

namespace RayWork.SaveSystem;

public class SavableObject<TSaveType> : ISavable
{
    public static JsonSerializerSettings? Settings = null;

    private TSaveType SaveType;
    private string FileName;

    public SavableObject(TSaveType saveType, string fileName)
    {
        SaveType = saveType ?? throw new ArgumentException("Save item given was null");
        FileName = fileName;
    }

    public string GetFileName() => FileName;

    public string SaveString()
    {
        return JsonConvert.SerializeObject(SaveType, Settings);
    }

    public void LoadString(string data, string file)
    {
        try
        {
            SaveType.Set(JsonConvert.DeserializeObject<TSaveType>(data, Settings));
        }
        catch (ArgumentException)
        {
            throw new ArgumentException($"Item corruption possibly detected in file [{file}]");
        }
    }
}