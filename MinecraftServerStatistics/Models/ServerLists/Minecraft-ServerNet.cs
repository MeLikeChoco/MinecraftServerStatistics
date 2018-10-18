using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServerStatistics.Models.ServerLists
{
    public class Minecraft_ServerNet : Scraper
    {

        public Minecraft_ServerNet() : base("https://minecraft-server.net/") { }

        protected override async Task<int> GetFeaturedServerCount()
        {

            var link = $"{Site}rank/1/in/";
            var dom = await Context.OpenAsync(link);

            return dom.GetElementById("Sponsored Servers").GetElementsByTagName("tbody").First().ChildElementCount;

        }

        protected override IEnumerable<string> GetFeatures(IDocument dom, List<string> features)
        {
            throw new NotImplementedException();
        }

        protected override string GetName(IDocument dom)
            => dom.GetElementsByClassName("center").First().TextContent.Trim();

        protected override string GetServerIP(IDocument dom)
            => GetSection(GetTable(dom), "Server IP")
            .ChildNodes
            .ElementAt(1)
            .TextContent.Trim();

        protected override async Task<List<string>> GetServerLinks(List<string> links, int page, int remaining)
        {

            //this site uses some weird paging system, i had to plot a graph and then find out an equation for it.... its so stupid
            page = 15 * (page - (14 / 15));
            var link = $"{Site}rank/{page}/in/";
            var dom = await Context.OpenAsync(link);
            IEnumerable<IElement> servers = new List<IElement>();

            if (page == 1)
                servers.Concat(dom.GetElementById("Sponsored Servers").GetElementsByClassName("gold"));

            servers.Concat(dom.GetElementById("Minecraft Servers").GetElementsByTagName("tbody").First().Children);
            links.AddRange(servers.Select(element => element.GetElementsByTagName("a").First().GetAttribute("href")));

            return links;

        }

        protected override string GetVersion(IDocument dom)
           => GetSection(GetTable(dom), "Server Version")
            .GetElementsByTagName("a")
            .First()
            .TextContent.Trim();

        protected override string GetWebsite(IDocument dom)
        {
            throw new NotImplementedException();
        }

        private IElement GetSection(IElement table, string match)
            => table.Children.FirstOrDefault(element => element.FirstChild.TextContent.Trim() == match);

        private IElement GetTable(IDocument dom)
            => dom.GetElementsByClassName("table table-hover").First().GetElementsByTagName("tbody").First();

    }
}
