using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace MinecraftServerStatistics.Models.ServerLists
{
    public class Minecraft_ServerNet : Scraper
    {

        public Minecraft_ServerNet() : base("https://minecraft-server.net/") { }

        protected override Task<int> GetFeaturedServerCount()
        {
            throw new NotImplementedException();
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

        protected override Task<List<string>> GetServerLinks(List<string> links, int page, int remaining)
        {
            throw new NotImplementedException();
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
