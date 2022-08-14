using System.Text.RegularExpressions;

namespace MiniBlog.Tests.Config;

public static class AntiForgeryExtractor
{
    public const string AntiforgeryCookieName = "TestAntiforgeryCookie";
    public const string AntiforgeryFormFieldName = "TestAntiforgeryField";

    public static async Task<(string cookie, string field)> ExtractAntiforgeryKeys(this HttpResponseMessage response)
    {
        var cookie = ExtractAntiForgeryCookie(response);
        var field = await ExtractAntiforgeryFormFieldValue(response);
        return (cookie, field);
    } 

    private static string ExtractAntiForgeryCookie(HttpResponseMessage response)
    {
        string? cookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault(c => c.Contains(AntiforgeryCookieName));
        if(cookie == null) throw new ArgumentException($"Cookie {AntiforgeryCookieName} not found.");
        var regexCapture = Regex.Match(cookie, $@"{AntiforgeryCookieName}=([^;]+?); .*");
        cookie = cookie.Substring(cookie.IndexOf('=')+1, cookie.IndexOf(';')-cookie.IndexOf('=')-1);
        return cookie;
    }

    private async static Task<string> ExtractAntiforgeryFormFieldValue(HttpResponseMessage response)
    {
        string body = await response.Content.ReadAsStringAsync();
        var valueMatch = Regex.Match(body, $@"\<input name=""{AntiforgeryFormFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");
        if(!valueMatch.Success) throw new ArgumentException($"Form field {AntiforgeryCookieName} not found.");
        return valueMatch.Groups[1].Captures[0].Value;
    }
}