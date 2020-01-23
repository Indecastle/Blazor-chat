
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Westwind.AspNetCore.Markdown;
using Chat.Models;

namespace Chat.Data
{
    public static class ChatHelper
    {
        static readonly string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".svg", ".kek" };

        public static MarkupString Parse(string markdown)
        {
            return new MarkupString(Markdown.Parse(markdown));
        }
        public static MarkupString ConvertUrl(Message message)
        {
            if (message.IsFile)
                if (imageExtensions.Contains(Path.GetExtension(message.FileName)))
                    return new MarkupString($"<img src=\"{message.Text}\">");
                else
                    return new MarkupString($"<a href=\"{message.Text}\">{message.FileName}</a>");
            else
                return new MarkupString(Markdown.Parse(message.Text));
            
            
        }
    }
}
