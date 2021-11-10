using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuledWebScraper.Shared.Models;
using System.Diagnostics;

namespace RuledWebScraper.Server {
  public class Program {
    public static void Main(string[] args) {
      //WebScraper scraper = new WebScraper("https://cbracco.github.io/html5-test-page/", "id");
      //Dictionary<int, bool> results;
      //scraper.AllTagsContainAttributes(scraper.GetHtmlFromPage().Result, out results, "id");

      CreateHostBuilder(args).Build().Run();

    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => {
              webBuilder.UseStartup<Startup>();
            });
  }
}
