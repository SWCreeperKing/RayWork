namespace RayWork.SaveSystem;

public interface Savable
{
    public string FileName();
    public string SaveString();
    public void LoadString(string data, string file);
}