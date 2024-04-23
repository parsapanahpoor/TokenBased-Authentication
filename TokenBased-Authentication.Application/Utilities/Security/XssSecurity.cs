using Ganss.Xss;

namespace TokenBased_Authentication.Application.Utilities.Security;

public static class XssSecurity
{
    public static string SanitizeText(this string text)
    {
        var htmlSanitizer = new HtmlSanitizer
        {
            KeepChildNodes = true,
            AllowDataAttributes = true
        };

        return htmlSanitizer.Sanitize(text);
    }
}
