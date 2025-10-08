using System.Security.Cryptography;

namespace NM.Studio.Domain.Utilities;

public static class RsaHelper
{
    public static RSA CreateRsaFromPrivateKey(string privateKey)
    {
        // Remove headers and whitespace
        var keyString = privateKey
            .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
            .Replace("-----END RSA PRIVATE KEY-----", "")
            .Replace("\n", "")
            .Trim();
                
        // Convert base64 to byte array
        var keyBytes = Convert.FromBase64String(keyString);
            
        // Create RSA instance and import key
        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(keyBytes, out _);
            
        return rsa;
    }
    
    public static RSA CreateRsaFromPublicKey(string publicKey)
    {
        // Remove headers and whitespace
        var keyString = publicKey
            .Replace("-----BEGIN PUBLIC KEY-----", "")
            .Replace("-----END PUBLIC KEY-----", "")
            .Replace("\n", "")
            .Trim();
                
        // Convert base64 to byte array
        var keyBytes = Convert.FromBase64String(keyString);
            
        // Create RSA instance and import key
        var rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(keyBytes, out _);
            
        return rsa;
    }
    
    public static string ExportPrivateKey(RSA rsa)
    {
        var privateKeyBytes = rsa.ExportRSAPrivateKey(); // PKCS#1 DER
        return Convert.ToBase64String(privateKeyBytes, Base64FormattingOptions.InsertLineBreaks);
    }

    public static string ExportPublicKey(RSA rsa)
    {
        var publicKeyBytes = rsa.ExportSubjectPublicKeyInfo(); // X.509 DER
        return Convert.ToBase64String(publicKeyBytes, Base64FormattingOptions.InsertLineBreaks);
    }
}