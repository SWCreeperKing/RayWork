using System.Text;

namespace RayWork.SaveSystem;

public class Encryptor
{
    private readonly Func<string, string> EncryptFunc;
    private readonly Func<string, string> DecryptFunc;

    public Encryptor(Func<string, string> encryptFunc, Func<string, string> decryptFunc)
    {
        EncryptFunc = encryptFunc;
        DecryptFunc = decryptFunc;
        CheckEncryption();
    }

    public void CheckEncryption()
    {
        if (EncryptFunc is null) throw new ArgumentException("Encryption Method is null");
        if (DecryptFunc is null) throw new ArgumentException("Decryption Method is null");

        StringBuilder sb = new();
        Random r = new();
        var charStop = r.Next(100, 151);

        for (var i = 0; i < charStop; i++) sb.Append((char) r.Next(0, 256));

        var str = sb.ToString();
        var enc = EncryptFunc.Invoke(str);
        var dec = DecryptFunc.Invoke(enc);
        var flawless = str != enc;

        if (!flawless) throw new ArgumentException("The Encryption results in the starting string");
        if (str != dec) throw new ArgumentException("Encryption/Decryption failed final decryption check");
    }

    public string Encrypt(string data) => EncryptFunc(data);
    public string Decrypt(string data) => DecryptFunc(data);
}