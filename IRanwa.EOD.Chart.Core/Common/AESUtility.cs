using System.Security.Cryptography;

namespace IRanwa.EOD.Chart.Core;

/// <summary>
/// AES utility.
/// </summary>
public static class AESUtility
{
    /// <summary>
    /// The aes key
    /// </summary>
    private static readonly string aesKey = "";

    /// <summary>
    /// The aes iv
    /// </summary>
    private static readonly string aesIV = "";

    /// <summary>
    /// The aes key byte array
    /// </summary>
    private static byte[] aesKeyByteArray;

    /// <summary>
    /// The aes iv byte array
    /// </summary>
    private static byte[] aesIVByteArray;

    /// <summary>
    /// Initializes the <see cref="AESUtility"/> class.
    /// </summary>
    static AESUtility()
    {
        aesKeyByteArray = Convert.FromBase64String(aesKey);
        aesIVByteArray = Convert.FromBase64String(aesIV);
    }

    /// <summary>
    /// Encrypts the aes asynchronous.
    /// </summary>
    /// <param name="plainText">The plain text.</param>
    /// <returns>Returns encrypted value.</returns>
    public static async Task<string> EncryptAESAsync(string plainText)
    {
        byte[] encrypted = await EncryptAESAsync(plainText, aesKeyByteArray, aesIVByteArray);
        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    /// Decrypts the aes asynchronous.
    /// </summary>
    /// <param name="cypherText">The cypher text.</param>
    /// <returns>Returns decrypted value.</returns>
    public static async Task<string> DecryptAESAsync(string cypherText)
    {
        byte[] encryptedByteArray = Convert.FromBase64String(cypherText);
        return await DecryptAESAsync(encryptedByteArray, aesKeyByteArray, aesIVByteArray);
    }

    /// <summary>
    /// Encrypts the aes asynchronous.
    /// </summary>
    /// <param name="plainText">The plain text.</param>
    /// <param name="key">The key.</param>
    /// <param name="IV">The iv.</param>
    /// <returns>Returns encrypted bytes array.</returns>
    public static async Task<byte[]> EncryptAESAsync(string plainText, byte[] key, byte[] IV)
    {
        if (plainText == null || plainText == "" || plainText.Length <= 0)
            throw new ArgumentException("plainText");

        if (key == null || key.Length <= 0)
            throw new ArgumentException("key");

        if (IV == null || IV.Length <= 0)
            throw new ArgumentException("IV");

        byte[] encrypted;
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = IV;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (var mEncryptor = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(mEncryptor, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                        await swEncrypt.WriteAsync(plainText);
                    encrypted = mEncryptor.ToArray();
                }
            }
        }
        return encrypted;
    }

    /// <summary>
    /// Decrypts the aes asynchronous.
    /// </summary>
    /// <param name="cypherText">The cypher text.</param>
    /// <param name="key">The key.</param>
    /// <param name="IV">The iv.</param>
    /// <returns>Returns decrypted bytes array.</returns>
    public static async Task<string> DecryptAESAsync(byte[] cypherText, byte[] key, byte[] IV)
    {
        if (cypherText == null || cypherText.Length <= 0)
            throw new ArgumentException("cypherText");

        if (key == null || key.Length <= 0)
            throw new ArgumentException("key");

        if (IV == null || IV.Length <= 0)
            throw new ArgumentException("IV");

        string decrypted = null;
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = IV;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (var mDecrytor = new MemoryStream(cypherText))
            {
                using (var csDecrypt = new CryptoStream(mDecrytor, decryptor, CryptoStreamMode.Read))
                {
                    using (var swDecrypt = new StreamReader(csDecrypt))
                        decrypted = await swDecrypt.ReadToEndAsync();
                }
            }
        }
        return decrypted;
    }
}