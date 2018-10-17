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
            throw new NotImplementedException();
        }

        protected override string GetName(IDocument dom)
        {
            throw new NotImplementedException();
        }

        protected override string GetServerIP(IDocument dom)
        {
            throw new NotImplementedException();
        }

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
        {
            throw new NotImplementedException();
        }

        protected override string GetWebsite(IDocument dom)
        {
            throw new NotImplementedException();
        }

    }
}
