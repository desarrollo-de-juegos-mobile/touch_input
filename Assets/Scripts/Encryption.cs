using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;


public class Encryption : MonoBehaviour
{
    // Encriptar y desencriptar datos
    private string secretKey = "yad4Hmn093!@#";

    public byte[] EncryptData(string data, string key)
    {

        byte[] encrypted;

        // AQUI comienza el algoritmo de encriptacion...
        using (Aes aes = Aes.Create())
        {
            aes.Key = System.Text.Encoding.UTF8.GetBytes(key);
            aes.GenerateIV();
            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new System.IO.StreamWriter(cs))
                        {
                            sw.Write(data);
                        }
                    }
                    encrypted = ms.ToArray();
                }
            }
        }


        return encrypted;
    }

    public string DecryptData(byte[] data, string key)
    {
        string decrypted = null;

        // AQUI comienza el algoritmo de desencriptacion...
        using (Aes aes = Aes.Create())
        {
            aes.Key = System.Text.Encoding.UTF8.GetBytes(key);
            byte[] iv = new byte[aes.BlockSize / 8];
            System.Array.Copy(data, iv, iv.Length);
            aes.IV = iv;
            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                using (var ms = new System.IO.MemoryStream(data, iv.Length, data.Length - iv.Length))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new System.IO.StreamReader(cs))
                        {
                            decrypted = sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        return decrypted;
    }


    public string GenerateChecksum(string data)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            byte[] hash = sha256.ComputeHash(bytes);
            return System.BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }

    // Ejemplo de uso de GenerateChecksum...
    public void ExampleUsage()
    {
        string data = "Hello, World!";
        string checksum = GenerateChecksum(data);
        Debug.Log("Checksum: " + checksum);
        byte[] encryptedData = EncryptData(data, secretKey);
        Debug.Log("Encrypted Data: " + System.Convert.ToBase64String(encryptedData));
        string decryptedData = DecryptData(encryptedData, secretKey);
        Debug.Log("Decrypted Data: " + decryptedData);
        // probar que el checksum del dato desencriptado es el mismo...
        string decryptedChecksum = GenerateChecksum(decryptedData);

        if (checksum == decryptedChecksum)
        {
            Debug.Log("Checksum verified successfully.");
        }
        else
        {
            Debug.Log("Checksum verification failed.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
