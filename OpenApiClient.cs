using System.Dynamic;

namespace ZolozCSharpOpenApi;

public class OpenApiClient
{
    public string HostUrl {get; init;}
    public string ClientID {get; set;}
    public string MerchantPrivateKey {get; set;} 
	public string OpenApiPublicKey {get; set;}
	public bool Encrypted {get; set;}
	public bool IsLoadTest {get; set;}
	public int AesLength {get; set;}

    public OpenApiClient (string hostUrl, bool encrypted){
        HostUrl = hostUrl;
        Encrypted = encrypted;
    }

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
