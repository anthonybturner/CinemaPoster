using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPosterApp.Utilities
{
    class HtmlParser
    {

        public HtmlParser()
        {

        }

        public string ParseHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNodeCollection collection = htmlDoc.DocumentNode.SelectNodes("//div[@class='playing-title']");

            foreach (var x in collection)
            {
                Console.WriteLine(x.InnerText);
            }

            //  var link = htmlDoc.DocumentNode.Descendants("div").Where(node => !node.GetAttributeValue("class", "").Contains("playing-title")).ToList();
            return "";

        }

    }
}
