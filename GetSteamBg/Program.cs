using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace GetSteamBg
{
    class Program
    {
        static WebClient webClient = new WebClient();
        static WebProxy proxyObject = null;
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("PageNo: 按回车默认为0");
                string inputPageNo = Console.ReadLine();
                inputPageNo = inputPageNo.Equals("") ? "0" : inputPageNo;
                Console.WriteLine("设置http代理 例:http://127.0.0.1:1080/  不设置直接按回车");
                string httpProxyUrl = Console.ReadLine();
                if (!httpProxyUrl.Equals(""))
                {
                    proxyObject = new WebProxy(httpProxyUrl, true);
                    webClient.Proxy = proxyObject;
                    Console.WriteLine($"设置http代理为{httpProxyUrl}");
                }
                
                string retJson_ = _Get(@"https://steamcommunity.com/market/search/render/?query=&start=1&count=100&search_descriptions=0&sort_column=name&sort_dir=asc&appid=753&category_753_Game%5B%5D=any&category_753_item_class%5B%5D=tag_item_class_3");
                dynamic dynamic_ = JsonConvert.DeserializeObject(retJson_);
                double fullPageNo = int.Parse(dynamic_["total_count"].ToString()) / 100;
                fullPageNo = Math.Round(fullPageNo, 0);
                Console.WriteLine($"正在从第{inputPageNo}页开始抓取 共{fullPageNo}页");
                for (int i = int.Parse(inputPageNo); i < fullPageNo; i++)
                {
                    int page = i * 100;
                    string url = @"https://steamcommunity.com/market/search/render/?query=&start=" + page.ToString() + @"&count=100&search_descriptions=0&sort_column=name&sort_dir=asc&appid=753&category_753_Game%5B%5D=any&category_753_item_class%5B%5D=tag_item_class_3";
                    string retJson = _Get(url);
                    dynamic dynamic = JsonConvert.DeserializeObject(retJson);
                    string bgHtml = dynamic["results_html"].ToString();
                    string json = _GetBgHtml(bgHtml);
                    _Replace(i.ToString(), json);
                    Console.WriteLine($"第{i}页抓取完成");
                    Thread.Sleep(15000);
                }
            }
            catch (MyException ex)
            {

                Console.WriteLine(ex.MyMessage + "   详细: " + ex.Message);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }


            while (true)
            {
            }
        }

        static string _Get(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            try
            {
                string html = webClient.DownloadString(url);
                return html;
            }
            catch (Exception ex)
            {
                throw new MyException("十有八九网不通", ex.Message);
            }
        }

        private static string _GetBgHtml(string html)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            string imgUrl = string.Empty;
            string bgUrl = string.Empty;
            HtmlNodeCollection titleNodes = doc.DocumentNode.SelectNodes("//a[*]");

            if (titleNodes != null)
            {
                List<BgInfo> bhInfos = new List<BgInfo>();
                foreach (var item in titleNodes)
                {
                    bgUrl = item.GetAttributeValue("href", "");
                    try
                    {
                        imgUrl = item.SelectSingleNode(".//img").Attributes["src"].Value.Replace("/62fx62f", "/220fx220f");
                    }
                    catch (Exception)
                    {

                        imgUrl = "is null";
                    }

                    Console.WriteLine(imgUrl);
                    BgInfo bgInfo = new BgInfo();
                    bgInfo.url = imgUrl;
                    bgInfo.marketurl = bgUrl;
                    bhInfos.Add(bgInfo);
                }
                string json = JsonConvert.SerializeObject(bhInfos);
                return json;
            }
            return "";
        }
        class BgInfo
        {
            string url_;
            string marketurl_;

            public string url { get => url_; set => url_ = value; }
            public string marketurl { get => marketurl_; set => marketurl_ = value; }
        }
        static void _Replace(string pageNo, string json)
        {
            StreamReader sr = new StreamReader("html/mb.html");
            string temp_mb = sr.ReadToEnd();
            sr.Close();
            temp_mb = temp_mb.Replace("@@@@@@@@@@", json);
            StreamWriter sw = new StreamWriter("html/" + pageNo + ".html");
            sw.Write(temp_mb);
            sw.Close();
        }
    }
}
