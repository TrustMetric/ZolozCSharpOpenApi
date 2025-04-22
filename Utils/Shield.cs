using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ZolozCSharpOpenApi.Utils.Log;

namespace ZolozCSharpOpenApi.Utils.Shield;

public static class SignatureUtils
{
    public static string CreateSignature(string unsignedContent, string privateKeyPem)
    {
        Logger.Info("Normalizing Private Key");
        var normalizedKey = NormalizePEM(privateKeyPem, "PRIVATE");

        Logger.Info("Creating Sign");
        byte[] dataBytes = Encoding.UTF8.GetBytes(unsignedContent);
        byte[] signatureBytes;

        using (var rsa = RSA.Create())
        {
            rsa.ImportFromPem(normalizedKey.ToCharArray());

            signatureBytes = rsa.SignData(
                dataBytes,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1
            );
        }

        Logger.Info("Signature Signed");

        string base64Signature = Convert.ToBase64String(signatureBytes);
        string encodedSignature = WebUtility.UrlEncode(base64Signature);

        return encodedSignature;
    }

    //TODO Create Verify Signature
    public static bool VerifySignature(string unsignedContent, string base64UrlEncodedSignature, string publicKeyPem)
    {
        var normalizedKey = NormalizePEM(publicKeyPem, "PUBLIC");
        // Step 1: Decode the signature from URL-safe Base64
        string base64Signature = HttpUtility.UrlDecode(base64UrlEncodedSignature);
        byte[] signatureBytes = Convert.FromBase64String(base64Signature);

        // Step 2: Convert PEM to RSA
        using RSA rsa = RSA.Create();
        rsa.ImportFromPem(normalizedKey.ToCharArray());

        // Step 3: Convert unsigned content to bytes
        byte[] dataBytes = Encoding.UTF8.GetBytes(unsignedContent);

        // Step 4: Verify signature
        bool isValid = rsa.VerifyData(
            dataBytes,
            signatureBytes,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1
        );

        return isValid;
    }

    private static string NormalizePEM(string pem, string type)
    {
        // Remove any existing headers/footers and reformat
        string header = $"-----BEGIN {type} KEY-----";
        string footer = $"-----END {type} KEY-----";

        var sb = new StringBuilder();
        sb.AppendLine(header);

        string base64 = pem
            .Replace(header, "")
            .Replace(footer, "")
            .Replace("\n", "")
            .Replace("\r", "")
            .Trim();

        int lineLength = 64;
        for (int i = 0; i < base64.Length; i += lineLength)
        {
            sb.AppendLine(base64.Substring(i, Math.Min(lineLength, base64.Length - i)));
        }

        sb.AppendLine(footer);
        return sb.ToString();
    }
}