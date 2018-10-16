//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AngleSharp;
//using AngleSharp.Dom;

//namespace MinecraftServerStatistics.Models.ServerLists
//{
//    public class MinecraftServersOrg : Scraper
//    {

//        public MinecraftServersOrg() : base("https://minecraftservers.org/") { }

//        protected override string GetName(IDocument dom)
//        {
//            throw new NotImplementedException();
//        }

//        protected override IEnumerable<string> GetPlugins(IDocument dom, List<string> plugins)
//        {
//            throw new NotImplementedException();
//        }

//        protected override string GetServerIP(IDocument dom)
//        {
//            throw new NotImplementedException();
//        }

//        protected override async Task<List<string>> GetServerLinks(List<string> links, int page, int remaining)
//        {

//            var link = $"{Site}index/{page}";
//            var dom = await Context.OpenAsync(link);
            

//            return links;

//        }

//        protected override string GetVersion(IDocument dom)
//        {
//            throw new NotImplementedException();
//        }

//        protected override string GetWebsite(IDocument dom)
//        {
//            throw new NotImplementedException();
//        }

//    }
//}
