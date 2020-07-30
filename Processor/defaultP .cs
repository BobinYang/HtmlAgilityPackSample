using HtmlAgilityPack;
using System;

namespace HtmlAgilityPack1.Processor
{
    internal class defaultP : IWebP
    {
        public HtmlNode GetContent(Uri uri, HtmlDocument htmlDoc, out string title)
        {

            title = "";

            HtmlNode myNOde = htmlDoc.CreateElement("div");
            var allNodes = htmlDoc.DocumentNode.SelectNodes("*");
            myNOde.AppendChildren(allNodes);
            return myNOde;
        }
    }
}