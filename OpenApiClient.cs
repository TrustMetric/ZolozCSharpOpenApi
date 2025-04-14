using System.Dynamic;

namespace ZolozCSharpOpenApi;

public class OpenApiClient(string hostUrl, bool encrypted)
{
    public required string HostUrl { get; init; } = hostUrl;
    public required string ClientID {get; set;}
    public required string MerchantPrivateKey {get; set;} 
	public required string OpenApiPublicKey {get; set;}
    public bool Encrypted { get; set; } = encrypted;
    public bool IsLoadTest {get; set;}
	public int AesLength {get; set;}

    public OpenApiClient (
        string hostUrl,
        string clientID,
        string merchantPrivateKey,
        string openApiPublicKey,
        bool encrypted,
        bool isLoadTest,
        int aesLength) : this(hostUrl, encrypted)
    {
        ClientID = clientID;
        MerchantPrivateKey = merchantPrivateKey;
        OpenApiPublicKey = openApiPublicKey;
        IsLoadTest = isLoadTest;
        AesLength = aesLength;
    }
}
