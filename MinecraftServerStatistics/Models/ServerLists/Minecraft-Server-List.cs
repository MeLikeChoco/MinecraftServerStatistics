using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using MinecraftServerStatistics.Models.Attributes;

namespace MinecraftServerStatistics.Models.ServerLists
{
    public class Minecraft_Server_List : Scraper
    {

        public Minecraft_Server_List() : base("https://minecraft-server-list.com/",
            "First 10 servers are featured servers, so they may not actually be top servers.") { }

        protected override string GetName(IDocument dom)
        {

            var table = GetDataTable(dom);

            if (!string.IsNullOrEmpty(table.FirstChild.TextContent))
                return table.FirstChild.TextContent.Trim();
            else
                return table.Children[1].TextContent.Trim();

        }

        protected override IEnumerable<string> GetFeatures(IDocument dom, List<string> features)
        {
            
            var table = GetDataTable(dom);
            var featureSection = table.LastElementChild;
            
            foreach(var element in featureSection.FirstElementChild.Children)
                features.Add(element.TextContent.Trim());

            return features;

        }

        protected override string GetServerIP(IDocument dom)
        {

            var table = GetDataTable(dom);
            var ipSection = table.Children
                .First(element => element.FirstChild.TextContent.Contains("Server IP:"));

            return ipSection.Children[1].TextContent.Trim();

        }

        protected override async Task<List<string>> GetServerLinks(List<string> links, int page, int remaining)
        {

            var link = $"{Site}page/{page}";
            var dom = await Context.OpenAsync(link);
            var table = dom.GetElementsByClassName("serverdatadiv1")
                .First()
                .GetElementsByTagName("tbody")
                .First()
                .Children
                .Take(remaining);

            foreach (var element in table)
            {

                var pageLink = element.GetElementsByClassName("column-heading")
                    .FirstOrDefault()?
                    .FirstElementChild.GetAttribute("href");

                if (pageLink == null)
                    continue;

                links.Add($"https:{pageLink}");

            }

            return links;

        }

        protected override string GetVersion(IDocument dom)
        {

            var table = GetDataTable(dom);
            var versionElement = table.Children
                .First(element => element.FirstChild.TextContent.Contains("Server Version:"));
            return versionElement.Children
                .ElementAt(1)
                .TextContent.Trim().Replace("[", "").Replace("]", "");

        }

        protected override string GetWebsite(IDocument dom)
        {

            var table = GetDataTable(dom);
            var websiteElement = table.Children
                .FirstOrDefault(element => element.FirstChild.TextContent.Contains("Website:"));

            if (websiteElement == null)
                return null;

            return websiteElement.GetElementsByTagName("a").First().GetAttribute("href");

        }

        private IElement GetDataTable(IDocument dom)
            => dom.GetElementsByClassName("serverdata w300").FirstOrDefault().FirstElementChild;

    }
}
