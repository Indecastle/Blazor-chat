
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Westwind.AspNetCore.Markdown;

namespace Chat.Data
{
    public static class MarkdownParser
    {
        public static MarkupString Parse(string markdown)
        {
            return new MarkupString(Markdown.Parse(markdown, true, true, true));
        }
    }
}
