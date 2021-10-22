using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Xunit;
using BlazorWebAssemblyCrawlerWithRules.Server.Controllers;

namespace BlazorWebAssemblyCrawlerWithRules.Server.Controllers {
  public class Test {
    [Fact]
    public void CanFindPage() {
      WebCrawler crawler = new WebCrawler();

      string page = crawler.GetHtmlPage("www.advancedmd.com");
      Assert.NotEmpty(page);
      Assert.NotNull(page);
    }
  }
}