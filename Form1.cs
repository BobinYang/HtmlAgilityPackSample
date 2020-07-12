using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    [ComVisible(true)]
    public partial class Form1 : Form
    {
        private string content = "";

        public Form1()
        {
            InitializeComponent();
            this.webBrowser1.Url = new System.Uri(Application.StartupPath + "\\kindeditor\\e.html", System.UriKind.Absolute);
            this.webBrowser1.ObjectForScripting = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {


            this.richTextBox1.Clear();
            //网页地址：
            string Url = this.textBox1.Text.Trim();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            List<string> list = new List<string>(); ;
            HtmlWeb htmlWeb = new HtmlWeb();
            htmlWeb.OverrideEncoding = Encoding.UTF8;

            HtmlAgilityPack.HtmlDocument htmlDoc = htmlWeb.Load(Url);
            HtmlNode node;
            HtmlNode myNOde = htmlDoc.CreateElement("div");
            var uri = new Uri(Url, UriKind.Absolute);
            switch (uri.Host)
            {
                case "docs.microsoft.com":
                    node = htmlDoc.DocumentNode.SelectSingleNode("//main[@id='main']");

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
                        var url = new Uri(htmlWeb.ResponseUri, src);
                        node1.SetAttributeValue("src", url.AbsoluteUri);
                    }

                    //链接路径转换
                    var hrefNode = node.SelectNodes("//a[@data-linktype='relative-path']|//a[@data-linktype='absolute-path']");
                    foreach (HtmlNode node1 in hrefNode)
                    {
                        src = node1.GetAttributeValue("href", "");
                        var url = new Uri(htmlWeb.ResponseUri, src);
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


                    //去掉前面无用的部分
                    var OK = node.SelectNodes("nav[1]/following-sibling::*");
                    myNOde.AppendChildren(OK);
                    break;
                case "www.cnblogs.com":
                    node = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='cnblogs_post_body']");
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
                    var OK1 = node.SelectNodes("*");
                    myNOde.AppendChildren(OK1);
                    break;
                default:
                    break;
            }




            //添加原文连接：
            HtmlNode nodeOriUrl = htmlDoc.CreateElement("p");
            nodeOriUrl.InnerHtml = "原文：<a href='" + htmlWeb.ResponseUri + "'>" + htmlWeb.ResponseUri + "</a>";
            myNOde.PrependChild(nodeOriUrl);

            ////写入到本地文件
            //TextWriter wr = new StreamWriter(@"aa.html");
            //myNOde.WriteTo(wr);
            //wr.Close();
            richTextBox1.Focus();
            this.richTextBox1.Text = myNOde.OuterHtml;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.richTextBox1.SelectAll();//全选文本框种的文本
            this.richTextBox1.Copy();//把选中的文本复制大剪切板
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.Focus();
            this.textBox1.SelectAll();
        }

        public void SetDetailContent()
        {
            webBrowser1.Document.InvokeScript("setContent", new object[] { content });
        }

        public string GetContent()
        {
            return content;
        }

        public void RequestContent(string str)
        {
            content = str;
            richTextBox1.Text = content;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Focused)
            {
                content = richTextBox1.Text;
                SetDetailContent();
            }
        }

        private void webBrowser1_Resize(object sender, EventArgs e)
        {
            this.webBrowser1.Refresh();
        }
    }
}