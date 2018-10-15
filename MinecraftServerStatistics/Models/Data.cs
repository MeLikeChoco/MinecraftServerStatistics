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
        public List<Server> Servers { get; }

        public Data(string site)
        {

            Site = site;
            Servers = new List<Server>();

        }

        public void AddServer(Server server)
            => Servers.Add(server);

    }
}
