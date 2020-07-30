using HtmlAgilityPack;
using System;

namespace HtmlAgilityPack1.Processor
{
    internal class cnblogsP : IWebP
    {
        public HtmlNode GetContent(Uri uri, HtmlDocument htmlDoc, out string title)
        {

            title = htmlDoc.DocumentNode.SelectSingleNode("//a[@id='cb_post_title_url']").InnerText;

            HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='cnblogs_post_body']");
            // https://www.cnblogs.com/doomclouds/p/13251785.html
            //如果存在H1，则把H2改成H3，H1改成H2

            //找到所有的H2标签，然后加上顺序。
            var h2Node1 = node.SelectNodes("//h2");
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
            HtmlNode myNOde = htmlDoc.CreateElement("div");
            var allNodes = node.SelectNodes("*");
            myNOde.AppendChildren(allNodes);
            return myNOde;
        }
    }
}