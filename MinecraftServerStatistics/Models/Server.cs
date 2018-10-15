using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServerStatistics.Models
{
    public class Server
    {

        public string Name { get; set; }
        public string ServerListLink { get; set; }
        public string WebsiteLink { get; set; }
        public string ServerIP { get; set; }
        public string Version { get; set; }
        public List<string> Plugins { get; set; }

    }
}
