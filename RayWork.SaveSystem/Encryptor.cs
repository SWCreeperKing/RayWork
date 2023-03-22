using System.Text;

namespace RayWork.SaveSystem;

public class Encryptor
{
    private Func<string, string> _encrypt;
    private Func<string, string> _decrypt;

    public Encryptor(Func<string, string> encrypt, Func<string, string> decrypt)
    {
        _encrypt = encrypt;
        _decrypt = decrypt;
        CheckEncryption();
    }

    public void CheckEncryption()
    {
        if (_encrypt is null || _decrypt is null)
        {
            if (_encrypt is null) throw new ArgumentException("Encryption Method is null");
            if (_decrypt is null) throw new ArgumentException("Decryption Method is null");
        }

        StringBuilder sb = new();
        Random r = new();
        var charStop = r.Next(100, 151);

        for (var i = 0; i < charStop; i++) sb.Append((char) r.Next(0, 256));

        var str = sb.ToString();
        var enc = _encrypt.Invoke(str);
        var dec = _decrypt.Invoke(enc);
        var flawless = str != enc;

        if (!flawless) throw new ArgumentException("The Encryption results in the starting string");
        if (str != dec) throw new ArgumentException("Encryption/Decryption failed final decryption check");
    }

    public string Encrypt(string data)
    {
        return _encrypt(data);
    }

    public string Decrypt(string data)
    {
        return _decrypt(data);
    }
}