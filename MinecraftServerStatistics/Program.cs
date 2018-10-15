using MinecraftServerStatistics.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServerStatistics
{
    public class Program
    {

        public static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Minecraft Server Statistic Scraper");
            Console.WriteLine("Getting modules...");

            var scrapers = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsClass && type.Namespace == "MinecraftServerStatistics.Models.ServerLists" && type.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                .Select(Activator.CreateInstance)
                .Cast<Scraper>();

            Console.WriteLine("Scraping...");

            var tasks = scrapers.Select(scraper => scraper.GetData()).ToArray();

            Task.WaitAll(tasks);

            var dataList = tasks.Select(task => task.Result);

            Console.WriteLine("Scraping finished.");

            var json = JsonConvert.SerializeObject(dataList, Formatting.Indented);

            Console.WriteLine("Writing to file...");

            File.WriteAllText("stats.json", json);
            Process.Start("notepad.exe", Path.Combine(Directory.GetCurrentDirectory(), "stats.json"));

        }

    }
}
