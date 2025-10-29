using BuildingBlocks.Domain.Constant;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Services.Encryption;

public interface IEncryptionService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}

public sealed class EncryptionService : IEncryptionService
{
    private string encryptionKey;
    public EncryptionService(IConfiguration configuration)
    {
        encryptionKey = configuration[ConfigurationKeys.EncryptionKey];
        ArgumentException.ThrowIfNullOrWhiteSpace(encryptionKey, nameof(encryptionKey));
    }

    public string Encrypt(string flatData)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(flatData);

        using Aes aes = Aes.Create();
        using ICryptoTransform cryptoTransform = aes.CreateEncryptor(Encoding.UTF8.GetBytes(encryptionKey), aes.IV);
        using MemoryStream memoryStream = new();
        using CryptoStream cryptoStream = new(memoryStream, cryptoTransform, CryptoStreamMode.Write);
        using (StreamWriter streamWriter = new(cryptoStream))
        {
            streamWriter.Write(flatData);
        }

        byte[] iv = aes.IV;
        byte[] decryptedContent = memoryStream.ToArray();
        byte[] result = new byte[iv.Length + decryptedContent.Length];

        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

        return Convert.ToBase64String(result);
    }

    public string Decrypt(string cipherText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cipherText);

        byte[] fullCipher = Convert.FromBase64String(cipherText);
        byte[] iv = new byte[16];
        byte[] cipher = new byte[fullCipher.Length - iv.Length];

        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);

        using Aes aes = Aes.Create();
        using ICryptoTransform cryptoTransform = aes.CreateDecryptor(Encoding.UTF8.GetBytes(encryptionKey), iv);

        string flatData;
        using (MemoryStream memoryStream = new(cipher))
        {
            using CryptoStream cryptoStream = new(memoryStream, cryptoTransform, CryptoStreamMode.Read);
            using StreamReader streamWriter = new(cryptoStream);
            flatData = streamWriter.ReadToEnd();
        }

        return flatData;
    }
}
