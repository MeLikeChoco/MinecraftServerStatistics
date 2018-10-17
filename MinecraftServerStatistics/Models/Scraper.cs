using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServerStatistics.Models
{
    public abstract class Scraper
    {

        //protected static HttpClient HttpClient = new HttpClient();
        protected static IBrowsingContext Context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());

        protected string Site { get; set; }
        protected string Notes { get; set; }

        public Scraper(string site, string notes = "First {0} servers are featured servers.")
        {

            Site = site;
            Notes = notes;

        }

        public async Task<Data> Scrape()
        {

            Console.WriteLine($"Beginning to scrape {Site}");

            var serverLinks = new List<string>(50);
            int remaining = 50, page = 0;

            do
            {

                serverLinks = await GetServerLinks(serverLinks, ++page, remaining);
                serverLinks = serverLinks.Distinct().ToList();
                remaining = 50 - serverLinks.Count;

            } while (remaining > 0);

            var data = new Data(Site, Notes) { FeaturedServerCount = await GetFeaturedServerCount() };
            var counter = 1;

            foreach (var link in serverLinks)
            {

                var server = new Server();
                var serverPageDom = await Context.OpenAsync(link);
                server.ServerListLink = link;
                server.Name = GetName(serverPageDom);
                server.WebsiteLink = GetWebsite(serverPageDom);
                server.ServerIP = GetServerIP(serverPageDom);
                server.Version = GetVersion(serverPageDom);
                server.Features = GetFeatures(serverPageDom, new List<string>());

                data.AddServer(counter++, server);

            }

            SetFeaturedServers(data);

            Console.WriteLine($"Finished scraping {Site}");

            return data;

        }

        protected virtual void SetFeaturedServers(Data data)
        {

            for (var i = 1; i <= data.FeaturedServerCount; i++)
                data.Servers[i].IsFeatured = true;

        }

        protected abstract Task<List<string>> GetServerLinks(List<string> links, int page, int remaining);
        protected abstract Task<int> GetFeaturedServerCount();
        protected abstract string GetName(IDocument dom);
        protected abstract string GetWebsite(IDocument dom);
        protected abstract string GetServerIP(IDocument dom);
        protected abstract string GetVersion(IDocument dom);
        protected abstract IEnumerable<string> GetFeatures(IDocument dom, List<string> features);

    }
}
