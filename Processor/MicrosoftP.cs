using HtmlAgilityPack;
using System;

namespace HtmlAgilityPack1.Processor
{
    internal class MicrosoftP : IWebP
    {
        public HtmlNode GetContent(Uri uri, HtmlDocument htmlDoc, out string title)
        {

            title = htmlDoc.DocumentNode.SelectSingleNode("//main[@id='main']/h1/span[1]").InnerText;

            HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode("//main[@id='main']");

            //去掉英文翻译
            var a = node.SelectNodes("//span[@class='sxs-lookup']");
            foreach (HtmlNode b in a)

            {
                b.Remove();
            }

            string src = "";
            //图片相对路径改成绝对路径
            var imgNode = node.SelectNodes("//img[@data-linktype='relative-path']");
            foreach (HtmlNode node1 in imgNode)
            {
                src = node1.GetAttributeValue("src", "");
                var url = new Uri(uri, src);
                node1.SetAttributeValue("src", url.AbsoluteUri);
            }

            //链接路径转换
            var hrefNode = node.SelectNodes("//a[@data-linktype='relative-path']|//a[@data-linktype='absolute-path']");
            foreach (HtmlNode node1 in hrefNode)
            {
                src = node1.GetAttributeValue("href", "");
                var url = new Uri(uri, src);
                node1.SetAttributeValue("href", url.AbsoluteUri);
            }

            //找到所有的H2标签，然后加上顺序。
            var h2Node = node.SelectNodes("//h2");
            var arr = new string[] { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十" };
            if (h2Node != null)
            {
                for (int i = 0; i < h2Node.Count; i++)
                {
                    h2Node[i].InnerHtml = arr[i] + "、" + h2Node[i].InnerHtml;
                    //找到所有的H3标签，然后加上顺序。

                    var h3Node = h2Node[i].SelectNodes("following-sibling::h2|following-sibling::h3");
                    if (h3Node is null)
                        break;
                    for (int j = 0; j < h3Node.Count; j++)
                    {
                        if (h3Node[j].Name == "h2")
                            break;
                        else
                            h3Node[j].InnerHtml = (j + 1) + "、" + h3Node[j].InnerHtml;
                    }
                }
            }

            HtmlNode myNOde = htmlDoc.CreateElement("div");
            //去掉前面无用的部分
            var allNodes = node.SelectNodes("nav[1]/following-sibling::*");
            myNOde.AppendChildren(allNodes);
            return myNOde;
        }
    }
}