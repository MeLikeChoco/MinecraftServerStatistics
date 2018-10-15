using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace MinecraftServerStatistics.Models.ServerLists
{
    public class Minecraft_Server_List : Scraper
    {

        public override string Site { get; protected set; }

        public Minecraft_Server_List()
            => Site = "https://minecraft-server-list.com/";

        protected override string GetName(IDocument dom)
        {

            var table = GetDataTable(dom);

            if (!string.IsNullOrEmpty(table.FirstElementChild.TextContent))
                return table.FirstElementChild.TextContent.Trim();
            else
                return table.Children[1].TextContent.Trim();

        }

        protected override List<string> GetPlugins(IDocument dom)
        {

            var plugins = new List<string>();
            var table = GetDataTable(dom);
            var pluginSection = table.LastElementChild;
            
            foreach(var element in pluginSection.FirstElementChild.Children)
                plugins.Add(element.TextContent.Trim());

            return plugins;

        }

        protected override string GetServerIP(IDocument dom)
        {

            var table = GetDataTable(dom);
            var ipSection = table.Children
                .First(element => element.TextContent.Contains("Server IP:"));

            return ipSection.Children[1].TextContent.Trim();

        }

        protected override async Task<IEnumerable<string>> GetServerLinks()
        {

            var pageToAmount = await File.ReadAllLinesAsync("Minecraft-Server-ListPages.txt");
            var serverPages = new List<string>();

            foreach(var line in pageToAmount)
            {

                var array = line.Split(':');
                var subPage = array.First();
                var amount = array.ElementAtOrDefault(1);
                var listLink = $"{Site}{subPage}";
                var dom = await Context.OpenAsync(listLink);
                IEnumerable<IElement> table = dom.GetElementsByClassName("serverdatadiv1")
                    .First()
                    .GetElementsByTagName("tbody")
                    .First()
                    .Children;

                if (int.TryParse(amount, out var result))
                    table = table.Take(result);
                
                foreach(var element in table)
                {
                    
                    var pageLink = element.GetElementsByClassName("column-heading")
                        .FirstOrDefault()?
                        .FirstElementChild.GetAttribute("href");

                    if (pageLink == null)
                        continue;

                    serverPages.Add($"https:{pageLink}");

                }

            }

            return serverPages;

        }

        protected override string GetVersion(IDocument dom)
        {

            var table = GetDataTable(dom);
            var versionElement = table.Children
                .First(element => element.TextContent.Contains("Server Version:"));
            return versionElement.Children
                .ElementAt(1)
                .TextContent.Trim().Replace("[", "").Replace("]", "");

        }

        protected override string GetWebsite(IDocument dom)
        {

            var table = GetDataTable(dom);
            var websiteElement = table.Children
                .FirstOrDefault(element => element.TextContent.Contains("Website:"));

            if (websiteElement == null)
                return null;

            return websiteElement.GetElementsByTagName("a").First().TextContent;

        }

        private IElement GetDataTable(IDocument dom)
            => dom.GetElementsByClassName("serverdata w300").FirstOrDefault().FirstElementChild;

    }
}
