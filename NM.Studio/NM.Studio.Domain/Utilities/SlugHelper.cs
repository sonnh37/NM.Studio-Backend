using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NM.Studio.Domain.Utilities;

public static class SlugHelper
{
    public static string ToSlug(string? title)
    {
        if (string.IsNullOrWhiteSpace(title)) return string.Empty;

        // Convert to lowercase
        var slug = title.ToLowerInvariant();
        // Remove accents
        slug = RemoveAccents(slug);
        // Replace spaces and other characters with hyphens
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", string.Empty); // Remove non-alphanumeric characters

        slug = Regex.Replace(slug, @"\s+", " "); // Convert multiple spaces to a single space
        slug = Regex.Replace(slug, @"\s", "-"); // Replace spaces with hyphens
        // Remove trailing hyphen if exists
        slug = slug.Trim('-');

        return slug;
    }

    // Chuyển đổi slug trở lại thành tiêu đề
    public static string FromSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return string.Empty;

        // Chuyển slug thành chữ thường, thay thế dấu gạch ngang bằng khoảng trắng
        var title = slug.Replace("-", " ");

        return title;
    }

    private static string RemoveAccents(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                // Special handling for 'đ'
                if (c == 'đ')
                    stringBuilder.Append('d');
                else
                    stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}