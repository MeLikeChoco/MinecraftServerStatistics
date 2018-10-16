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

        public Scraper(string site, string notes = "")
        {

            Site = site;
            Notes = notes;

        }

        public async Task<Data> Scrape()
        {

            var serverLinks = new List<string>(50);
            int remaining, page = 0;

            do
            {

                remaining = 50 - serverLinks.Count;
                page++;
                serverLinks = await GetServerLinks(serverLinks, page, remaining);
                
            } while (remaining > 0);

            var data = new Data(Site, Notes);
            var counter = 1;

            foreach(var link in serverLinks)
            {

                var server = new Server();
                var serverPageDom = await Context.OpenAsync(link);
                server.ServerListLink = link;
                server.Name = GetName(serverPageDom);
                server.WebsiteLink = GetWebsite(serverPageDom);
                server.ServerIP = GetServerIP(serverPageDom);
                server.Version = GetVersion(serverPageDom);
                server.Plugins = GetPlugins(serverPageDom, new List<string>());

                data.AddServer(counter++, server);

            }

            return data;

        }

        protected abstract Task<List<string>> GetServerLinks(List<string> links, int page, int remaining);
        //protected abstract bool GetIsFeatured(IDocument dom);
        protected abstract string GetName(IDocument dom);
        protected abstract string GetWebsite(IDocument dom);
        protected abstract string GetServerIP(IDocument dom);
        protected abstract string GetVersion(IDocument dom);
        protected abstract IEnumerable<string> GetPlugins(IDocument dom, List<string> plugins);

    }
}
