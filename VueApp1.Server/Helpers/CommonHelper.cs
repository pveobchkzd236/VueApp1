using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiTNMHelper
{
    public static class CommonHelper
    {
        public static string ExtractContent(string text, string tag)
        {
            var startTag = $"<{tag}>";
            var endTag = $"</{tag}>";

            var startIndex = text.IndexOf(startTag);
            var endIndex = text.IndexOf(endTag);

            if (startIndex != -1 && endIndex != -1)
            {
                return text.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length).Trim();
            }

            return null;
        }

        public static int? ExtractContentAsInt(string text, string tag)
        {
            var content = ExtractContent(text, tag);
            if (int.TryParse(content, out int result))
            {
                return result;
            }
            return null;
        }
    }
}
