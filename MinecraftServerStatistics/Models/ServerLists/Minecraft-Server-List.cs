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

        public Minecraft_Server_List() : base("https://minecraft-server-list.com/") { }

        protected override string GetName(IDocument dom)
        {

            var table = GetTable(dom);

            if (!string.IsNullOrEmpty(table.FirstChild.TextContent))
                return table.FirstChild.TextContent.Trim();
            else
                return table.Children[1].TextContent.Trim();

        }

        protected override IEnumerable<string> GetFeatures(IDocument dom, List<string> features)
        {
            
            var table = GetTable(dom);
            var featureSection = table.LastElementChild;
            var tagNodes = featureSection.FirstChild.ChildNodes;
            
            foreach(var nodes in tagNodes)
                features.Add(nodes.TextContent.Trim());

            return features;

        }

        protected override string GetServerIP(IDocument dom)
        {

            var ipSection = GetSection(GetTable(dom), "Server IP:");

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

            var versionSection = GetSection(GetTable(dom), "Server Version:");

            return versionSection.Children
                .ElementAt(1)
                .TextContent.Trim().Replace("[", "").Replace("]", "");

        }

        protected override string GetWebsite(IDocument dom)
        {

            var websiteSection = GetSection(GetTable(dom), "Website:");

            if (websiteSection == null)
                return null;

            return websiteSection.GetElementsByTagName("a").First().GetAttribute("href");

        }

        protected override async Task<int> GetFeaturedServerCount()
        {

            var dom = await Context.OpenAsync(Site);
            var table = dom.GetElementsByClassName("serverdatadiv1")
                .First()
                .GetElementsByTagName("tbody")
                .First();

            return table.GetElementsByClassName("featured").Count();

        }

        private IElement GetSection(IElement table, string match)
            => table.Children.FirstOrDefault(element => element.FirstChild.TextContent == match);

        private IElement GetTable(IDocument dom)
            => dom.GetElementsByClassName("serverdata w300").FirstOrDefault().FirstElementChild;

    }
}
