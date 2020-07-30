using HtmlAgilityPack;
using HtmlAgilityPack1.Processor;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

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

            HtmlDocument htmlDoc = htmlWeb.Load(Url);
            HtmlNode node;
            string title = "";
            var uri = new Uri(Url, UriKind.Absolute);
            IWebP webP = new defaultP();
            switch (uri.Host)
            {
                case "docs.microsoft.com":
                    webP = new MicrosoftP();
                    break;

                case "www.cnblogs.com":
                    webP = new cnblogsP();
                    break;

                case "blog.csdn.net":
                    webP = new csdnP();
                    break;

                case "www.runoob.com":
                    webP = new runoobP();
                    break;

                default:
                    break;
            }
            node = webP.GetContent(uri, htmlDoc, out title);

            //添加原文连接：
            HtmlNode nodeOriUrl = htmlDoc.CreateElement("p");
            nodeOriUrl.InnerHtml = "原文：<a href='" + htmlWeb.ResponseUri + "'>" + htmlWeb.ResponseUri + "</a><hr/>";
            node.PrependChild(nodeOriUrl);

            ////写入到本地文件
            //TextWriter wr = new StreamWriter(@"aa.html");
            //myNOde.WriteTo(wr);
            //wr.Close();
            richTextBox1.Focus();
            this.richTextBox1.Text = node.OuterHtml;
            this.txtTitle.Text = title.Trim();
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