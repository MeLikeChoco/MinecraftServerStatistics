using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServerStatistics.Models
{
    public class Data
    {

        public string Site { get; }

        private readonly string _notes;
        [JsonIgnore]
        public int FeaturedServerCount { get; set; }
        public string Notes { get => string.Format(_notes, FeaturedServerCount); }

        public SortedDictionary<int, Server> Servers { get; }

        public Data(string site, string notes)
        {

            Site = site;
            _notes = notes;
            Servers = new SortedDictionary<int, Server>();

        }

        public void AddServer(int ranking, Server server)
            => Servers[ranking] = server;

    }
}
