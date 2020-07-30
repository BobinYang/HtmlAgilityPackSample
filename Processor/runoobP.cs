using HtmlAgilityPack;
using System;

namespace HtmlAgilityPack1.Processor
{
    internal class runoobP : IWebP
    {
        public HtmlNode GetContent(Uri uri, HtmlDocument htmlDoc, out string title)
        {
            title = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='content']/h1").InnerText;

            HtmlNode MyNode = htmlDoc.CreateElement("div");
            var nodes1 = htmlDoc.DocumentNode.SelectNodes("//div[@id='content']/h1/following-sibling::*");
            MyNode.AppendChildren(nodes1);

            //找到所有的H2标签，然后加上顺序。
            var h2Node1 = MyNode.SelectNodes("//h2");
            var arr1 = new string[] { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十" };
            if (h2Node1 != null)
            {
                for (int i = 0; i < h2Node1.Count; i++)
                {
                    h2Node1[i].InnerHtml = arr1[i] + "、" + h2Node1[i].InnerHtml;
                    //找到所有的H3标签，然后加上顺序。

                    var h3Node = h2Node1[i].SelectNodes("following-sibling::h2|following-sibling::h3");
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

            return MyNode;
        }
    }
}