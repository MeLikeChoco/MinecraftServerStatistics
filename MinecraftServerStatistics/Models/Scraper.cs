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

        public abstract string Site { get; protected set; }

        public async Task<Data> GetData()
        {

            var serverListDom = await Context.OpenNewAsync(Site);
            var serverLinks = await GetServerLinks();
            var data = new Data(Site);

            foreach(var link in serverLinks)
            {

                var server = new Server();
                var serverPageDom = await Context.OpenAsync(link);
                server.ServerListLink = link;
                server.Name = GetName(serverPageDom);
                server.WebsiteLink = GetWebsite(serverPageDom);
                server.ServerIP = GetServerIP(serverPageDom);
                server.Version = GetVersion(serverPageDom);
                server.Plugins = GetPlugins(serverPageDom);

                data.AddServer(server);

            }

            return data;

        }

        protected abstract Task<IEnumerable<string>> GetServerLinks();
        protected abstract string GetName(IDocument dom);
        protected abstract string GetWebsite(IDocument dom);
        protected abstract string GetServerIP(IDocument dom);
        protected abstract string GetVersion(IDocument dom);
        protected abstract List<string> GetPlugins(IDocument dom);

    }
}
