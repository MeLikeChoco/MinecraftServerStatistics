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
        public string Notes { get; }
        public SortedDictionary<int, Server> Servers { get; }

        public Data(string site, string notes)
        {

            Site = site;
            Notes = notes;
            Servers = new SortedDictionary<int, Server>();

        }

        public void AddServer(int ranking, Server server)
            => Servers[ranking] = server;

    }
}
