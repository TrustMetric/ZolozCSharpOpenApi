using System.Dynamic;

namespace ZolozCSharpOpenApi;

public class OpenApiClient(
    string hostUrl,
    string clientID,
    string merchantPrivateKey,
    string openApiPublicKey,
    bool encrypted,
    bool isLoadTest,
    int aesLength)
{
    public required string HostUrl { get; init; } = hostUrl;
    public required string ClientID { get; init; } = clientID;
    public required string MerchantPrivateKey { get; init; } = merchantPrivateKey;
    public required string OpenApiPublicKey { get; init; } = openApiPublicKey;
    public bool Encrypted { get; init; } = encrypted;
    public bool IsLoadTest { get; init; } = isLoadTest;
    public int AesLength { get; init; } = aesLength;
}
