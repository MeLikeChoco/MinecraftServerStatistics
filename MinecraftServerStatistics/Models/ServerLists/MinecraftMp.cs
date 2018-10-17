using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using MinecraftServerStatistics.Models.Attributes;

namespace MinecraftServerStatistics.Models.ServerLists
{
    public class MinecraftMp : Scraper
    {

        public MinecraftMp() : base("https://minecraft-mp.com/") { }

        protected override async Task<int> GetFeaturedServerCount()
        {

            var dom = await Context.OpenAsync(Site);
            return dom.GetElementsByClassName("table table-bordered")
                .First()
                .GetElementsByTagName("tbody")
                .First().Children
                .Where(element => element.GetAttribute("height") == "90")
                .Count();

        }

        protected override IEnumerable<string> GetFeatures(IDocument dom, List<string> features)
        {

            var tagSection = GetSection(GetTable(dom), "Tag(s)");
            var tags = tagSection.Children.ElementAt(1).Children.Select(element => element.TextContent.Trim());

            features.AddRange(tags);

            return features;
            
        }

        protected override string GetName(IDocument dom)
            => dom.GetElementsByClassName("active").First().TextContent.Trim();

        protected override string GetServerIP(IDocument dom)
            => GetSection(GetTable(dom), "Address").Children.ElementAt(1).TextContent.Trim();

        protected override async Task<List<string>> GetServerLinks(List<string> links, int page, int remaining)
        {

            var link = page == 1 ? Site : $"{Site}servers/list/{page}/";
            var dom = await Context.OpenAsync(link);
            var servers = dom.GetElementsByClassName("table table-bordered")
                .SelectMany(element => element.GetElementsByTagName("tbody"))
                .SelectMany(element => element.Children.Where(child => child.GetAttribute("height") == "90"))
                .Take(remaining)
                .Select(element => element.Children.ElementAt(1).GetElementsByTagName("a").First())
                .Select(element => $"{Site}{element.GetAttribute("href").TrimStart('/')}");

            links.AddRange(servers);

            return links;

        }

        protected override string GetVersion(IDocument dom)
            => GetSection(GetTable(dom), "Minecraft Version").Children.ElementAt(1).TextContent.Trim();

        protected override string GetWebsite(IDocument dom)
            => GetSection(GetTable(dom), "Website")?.Children.ElementAt(1).TextContent.Trim();

        private IElement GetTable(IDocument dom)
            => dom.GetElementsByClassName("col-xs-7").First().GetElementsByTagName("table").First().FirstElementChild;

        private IElement GetSection(IElement table, string match)
            => table.Children.FirstOrDefault(element => element.GetElementsByTagName("td").First().TextContent.Trim() == match);

    }
}
