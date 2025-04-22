using System.Dynamic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ZolozCSharpOpenApi.Utils.Chrono;
using ZolozCSharpOpenApi.Utils.Log;
using ZolozCSharpOpenApi.Utils.Shield;

namespace ZolozCSharpOpenApi;

public class OpenApiClient
{
    public string HostUrl { get; }
    public string ClientID { get; }
    public string MerchantPrivateKey { get;}
    public string OpenApiPublicKey { get; }
    public bool Encrypted { get;}
    public bool IsLoadTest { get;}
    public int AesLength { get;}

    public OpenApiClient(
        string hostUrl,
        string clientID,
        string merchantPrivateKey,
        string openApiPublicKey,
        bool encrypted,
        bool isLoadTest,
        int aesLength)
    {
        HostUrl = hostUrl;
        ClientID = clientID;
        MerchantPrivateKey = merchantPrivateKey;
        OpenApiPublicKey = openApiPublicKey;
        Encrypted = encrypted;
        IsLoadTest = isLoadTest;
        AesLength = aesLength;
    }

    // public async Task<T> CallOpenApi<T, P>(string url, P payload){
    //     url = HostUrl + "/" + url;
    //     var httpClient = new HttpClient();
    //     var json = JsonSerializer.Serialize(payload);
    //     var content = new StringContent(json, Encoding.UTF8, "application/json");

    //     HttpResponseMessage response = await httpClient.PostAsync(url, content);

    //     var responseContent = await response.Content.ReadAsStringAsync();
    //     return JsonSerializer.Deserialize<T>(responseContent)!;
    // }

    // public async Task<string> CallOpenApi<T>(string url, T payload){
    //     url = HostUrl + "/" + url;
    //     var httpClient = new HttpClient();
    //     var json = JsonSerializer.Serialize(payload);
    //     var content = new StringContent(json, Encoding.UTF8, "application/json; charset=UTF-8");

    //     HttpResponseMessage response = await httpClient.PostAsync(url, content);

    //     var responseContent = await response.Content.ReadAsStringAsync();
    //     return responseContent;
    // }

    public async Task<string> CallOpenApi<T>(string apiName, T payload)
    {
        var url = HostUrl + apiName;
        var httpClient = new HttpClient();
        var requestString = JsonSerializer.Serialize(payload);
        var formattedTime = DateUtils.GetFormattedTime();
        Logger.Info($"URL is: {url}");
        Logger.Info($"Request string is: {requestString}");
        Logger.Info($"Formatted time is: {formattedTime}");

        // You can add additional content headers if needed
        // var content = new StringContent(requestString, Encoding.UTF8, "application/json");
        // content.Headers.ContentType!.CharSet = "UTF-8";  // Already default, but you can explicitly set it

        MediaTypeHeaderValue mediaType;
        if (Encrypted)
        {
            mediaType = MediaTypeHeaderValue.Parse("text/plain; charset=UTF-8");
        }
        else
        {
            mediaType = MediaTypeHeaderValue.Parse("application/json; charset=UTF-8");
        }

        var unsignedContent = "POST " + apiName + "\n" + ClientID + "." + formattedTime + "." + requestString;
        var requestSignature = SignatureUtils.CreateSignature(unsignedContent, MerchantPrivateKey);

        // Create the request
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(requestString, Encoding.UTF8)
        };

        request.Content.Headers.ContentType = mediaType;

        // Add custom headers to the request
        request.Headers.Add("Client-Id", ClientID);
        request.Headers.Add("Request-Time", formattedTime);
        request.Headers.Add("Signature", $"algorithm=RSA256, signature={requestSignature}");
        
        if (IsLoadTest) {
            request.Headers.Add("loadTestMode", "true");
            // requestConfig.headers["loadTestMode"] = "true";
        }

        // Send the request
        HttpResponseMessage response = await httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }
}
