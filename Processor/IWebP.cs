using HtmlAgilityPack;
using System;

namespace HtmlAgilityPack1.Processor
{
    internal interface IWebP
    {
        HtmlNode GetContent(Uri uri, HtmlDocument htmlDoc, out string title);
    }
}