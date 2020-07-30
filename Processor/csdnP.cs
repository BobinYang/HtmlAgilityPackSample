using HtmlAgilityPack;
using System;
using System.Text.RegularExpressions;

namespace HtmlAgilityPack1.Processor
{
    internal class csdnP : IWebP
    {
        public HtmlNode GetContent(Uri uri, HtmlDocument htmlDoc, out string title)
        {

            title = htmlDoc.DocumentNode.SelectSingleNode("//h1[@id='articleContentId']").InnerText;

            HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='content_views']");
            //如果存在H1，则把H2改成H3，H1改成H2

            //找到所有的H2标签，然后加上顺序。
            var h2Node = node.SelectNodes("//h2");
            var arr3 = new string[] { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十" };
            if (h2Node != null)
            {
                for (int i = 0; i < h2Node.Count; i++)
                {
                    h2Node[i].InnerHtml = arr3[i] + "、" + h2Node[i].InnerHtml;
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
            //去除图片水印
            //< img src = "https://img-blog.csdnimg.cn/20200625123350705.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MDcxOTk0Mw==,size_16,color_FFFFFF,t_70" alt = "在这里插入图片描述" >
            var imgNodes = node.SelectNodes("//img");
            foreach (var item in imgNodes)
            {
                if (item.Attributes["src"] != null)
                    item.Attributes["src"].Value = Regex.Replace(item.Attributes["src"].Value, @"(http[s]?://.*\.png)\?.*", "$1");
            }

            HtmlNode myNode = htmlDoc.CreateElement("div");
            var allNodes = node.SelectNodes("*");
            myNode.AppendChildren(allNodes);
            return myNode;
        }
    }
}