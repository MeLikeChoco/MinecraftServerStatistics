using MinecraftServerStatistics.Models;
using MinecraftServerStatistics.Models.Attributes;
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

            try
            {

                Console.WriteLine("Welcome to Minecraft Server Statistic Scraper");
                Console.WriteLine("Getting modules...");

                var scrapers = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => type.IsClass && type.Namespace == "MinecraftServerStatistics.Models.ServerLists" 
                    && type.GetCustomAttribute<CompilerGeneratedAttribute>() == null 
                    && type.GetCustomAttribute<IgnoreAttribute>() == null)
                    .Select(Activator.CreateInstance)
                    .Cast<Scraper>();

                Console.WriteLine("Scraping...");

                var tasks = scrapers.AsParallel().Select(scraper => scraper.Scrape()).ToArray();

                Task.WaitAll(tasks);

                var dataList = tasks.Select(task => task.Result).ToDictionary(data => data.Site, data => data);

                Console.WriteLine("Scraping finished.");

                var json = JsonConvert.SerializeObject(dataList, Formatting.Indented);

                Console.WriteLine("Writing to file...");

                File.WriteAllText("stats.json", json);
                Console.Write("Finished. Press any key to view result...");
                Console.ReadKey();

                Process.Start("notepad.exe", Path.Combine(Directory.GetCurrentDirectory(), "stats.json"));

            }
            catch (AggregateException e)
            {
                                
                Console.WriteLine(e.InnerException.Message);
                Console.WriteLine(e.InnerException.StackTrace);
                Console.ReadKey();

            }

        }

    }
}
