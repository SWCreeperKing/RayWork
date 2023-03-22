using Newtonsoft.Json;

namespace RayWork.SaveSystem;

public class SavableObject<SaveType> : Savable
{
    public static JsonSerializerSettings? settings = null;
    private SaveType _saveType;
    private string _fileName;

    public SavableObject(SaveType saveType, string fileName)
    {
        _saveType = saveType ?? throw new ArgumentException("Save item given was null");
        _fileName = fileName;
    }

    public string FileName() => _fileName;

    public string SaveString()
    {
        return JsonConvert.SerializeObject(_saveType, settings);
    }

    public void LoadString(string data, string file)
    {
        try
        {
            _saveType.Set(JsonConvert.DeserializeObject<SaveType>(data, settings));
        }
        catch (ArgumentException)
        {
            throw new ArgumentException($"Item corruption possibly detected in file [{file}]");
        }
    }
}