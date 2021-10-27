using RuledWebScraper.Shared.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace RuledWebScraperTests {
  public class UnitTest1 {
    [Fact]
    public void CanCreateScraper() {
      WebScraper scraper = new WebScraper();

      Assert.NotNull(scraper);
    }

    [Fact]
    public void CanCreateScraperWithPresetRules() {
      List<string> expected = new() { "id", "class" };
      WebScraper scraper = new(expected);

      Assert.Equal(expected, scraper.GetRequired());
    }

    [Fact]
    public void CanAddARule() {
      List<string> expected = new() { "id" };
      WebScraper scraper = new();

      scraper.AddRules("id");
      Assert.Equal(expected, scraper.GetRequired());
    }

    [Fact]
    public void CanAddMultipleRules() {
      List<string> expected = new() { "id", "class" };
      WebScraper scraper = new();

      scraper.AddRules("id", "class");
      Assert.Equal(expected, scraper.GetRequired());
    }

    [Fact]
    public void CanRemoveARule() {
      List<string> required = new() { "id", "class" };
      List<string> expected = new() { "id" };
      WebScraper scraper = new(required);

      scraper.RemoveRules("class");
      Assert.Equal(expected, scraper.GetRequired());
    }

    [Fact]
    public void CanRemoveMultipleRules() {
      List<string> required = new() { "id", "class" };
      List<string> expected = new();
      WebScraper scraper = new(required);

      scraper.RemoveRules("class", "id");
      Assert.Equal(expected, scraper.GetRequired());
    }
  }
}
