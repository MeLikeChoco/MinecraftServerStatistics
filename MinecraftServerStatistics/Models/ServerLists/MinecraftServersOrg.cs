using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServerStatistics.Models.ServerLists
{
    public class MinecraftServersOrg : Scraper
    {

        public MinecraftServersOrg() : base("https://minecraftservers.org/",
            "First 5 servers are featured/sponsored servers, so they are most likely not top servers. " +
            "Unlike minecraft-server-list, where it was not clear if the featured servers are actually the top servers, minecraftservers.org is clear on which servers are featured") { }

        protected override string GetName(IDocument dom)
            => dom.GetElementsByClassName("section-head").First().TextContent.Trim();

        protected override IEnumerable<string> GetFeatures(IDocument dom, List<string> features)
        {

            var table = GetTable(dom);
            var tags = table.GetElementsByClassName("tags")
                .First()
                .Children
                .Select(element => element.TextContent.Trim());

            features.AddRange(tags);

            return features;

        }

        protected override string GetServerIP(IDocument dom)
        {

            var table = GetTable(dom);
            var ipSection = table.Children
                .First(element => element.GetElementsByClassName("header").First().TextContent.Contains("IP"));

            return ipSection.Children.ElementAt(1).TextContent.Trim();

        }

        protected override async Task<List<string>> GetServerLinks(List<string> links, int page, int remaining)
        {

            var link = $"{Site}index/{page}";
            var dom = await Context.OpenAsync(link);
            var container = dom.GetElementById("main").GetElementsByClassName("sponsored-servers container cf").First();
            var servers = container.GetElementsByTagName("table")
                .SelectMany(element => element.GetElementsByTagName("tbody"))
                .SelectMany(element => element.GetElementsByTagName("tr"))
                .Take(remaining)
                .Select(element => $"{Site}server/{element.GetAttribute("data-id")}")
                .Distinct();

            links.AddRange(servers);

            return links;

        }

        protected override string GetVersion(IDocument dom)
            => GetTable(dom).GetElementsByClassName("version").First().TextContent.Trim();

        protected override string GetWebsite(IDocument dom)
        {

            var table = GetTable(dom);
            var websiteSection = table.Children
                .FirstOrDefault(element => element.GetElementsByClassName("header").First().TextContent.Contains("Website"));

            if (websiteSection == null)
                return null;

            return websiteSection.GetElementsByTagName("a").First().GetAttribute("href");

            //works too but more convoluted
            //return GetTable(dom).Children
            //    .FirstOrDefault(element => element.GetElementsByClassName("header").First().TextContent.Contains("Website"))?
            //    .GetElementsByTagName("a").First()
            //    .GetAttribute("href");
            
        }

        /// <summary>
        /// Returns tbody of table
        /// </summary>
        /// <param name="dom"></param>
        /// <returns></returns>
        private IElement GetTable(IDocument dom)
            => dom.GetElementsByClassName("server-info").FirstOrDefault()?.FirstElementChild;

    }
}
