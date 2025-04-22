namespace ZolozCSharpOpenApi.Utils.Chrono;
public static class DateUtils
{
    public static string GetFormattedTime()
    {
        var offset = DateTimeOffset.Now;
        string dateTimePart = offset.ToString("yyyy-MM-ddTHH:mm:ss");
        string timezonePart = offset.ToString("zzz").Replace(":", "");
        return dateTimePart + timezonePart;
    }
}