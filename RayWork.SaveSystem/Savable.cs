namespace RayWork.SaveSystem;

public interface ISavable
{
    public string GetFileName();
    public string SaveString();
    public void LoadString(string data, string file);
}