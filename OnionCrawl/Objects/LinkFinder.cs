using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace OnionCrawl.Objects
{
    public class LinkFinder
    {
        public Dictionary<string, string> PotentialLinks { get; set; }

        public LinkFinder()
        {
            PotentialLinks = new Dictionary<string, string>();
        }



        internal void FindPotentialScans(string source)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(source);
            var test = doc.DocumentNode.SelectNodes("//a").Select(x => x.OuterHtml).ToList();
            for(int i = 67; i < test.Count; i ++)
            {
                string[] parts = test[i].Split(new string[] { "href=" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in parts)
                {
                    if (s.Contains(@"http://") && s.Contains(".onion"))
                    {
                        string[] urlSplit = s.Split(new string[] { @"http://" }, StringSplitOptions.RemoveEmptyEntries);
                        
                        Console.WriteLine(s);
                    }
                }
            }
            //var test = doc.DocumentNode.SelectNodes("//a[starts-with(@href , '#') and string-length(@href) > 15]");
            Console.ReadLine();
        }
    }
}
