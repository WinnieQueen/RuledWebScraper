using RuledWebScraper.Shared.Models;
using System.Collections.Generic;
using Xunit;

namespace RuledWebScraperTests {
  public class RulesTests {
    private string sampleLink = "https://cbracco.github.io/html5-test-page/";

    [Fact]
    public void CanCreateScraper() {
      WebScraper scraper = new("");

      Assert.NotNull(scraper);
    }

    [Fact]
    public void CanCreateScraperWithPresetRules() {
      string[] expected = { "id", "class" };
      WebScraper scraper = new("", expected);

      Assert.NotNull(scraper);
      Assert.Equal(expected, scraper.GetRequired());
      Assert.Equal("", scraper.GetUrl());
    }

    [Fact]
    public void CanCreateScraperWithPresetUrl() {
      WebScraper scraper = new(sampleLink);

      Assert.Equal(sampleLink, scraper.GetUrl());
    }

    [Fact]
    public void CanCreateScraperWithPresetUrlAndRules() {
      string[] attributes = { "id", "class" };
      WebScraper scraper = new(sampleLink, attributes);

      Assert.Equal(attributes, scraper.GetRequired());
      Assert.Equal(sampleLink, scraper.GetUrl());
    }
    [Fact]
    public void CanSetUrlAfterCreation() {
      WebScraper scraper = new("");
      scraper.SetUrl(sampleLink);

      Assert.Equal(sampleLink, scraper.GetUrl());
    }

    [Fact]
    public void CanSetRulesAfterCreation() {
      string[] expected = { "id", "class" };
      WebScraper scraper = new("");
      scraper.SetRequired(expected);

      Assert.Equal(expected, scraper.GetRequired());
    }

    [Fact]
    public void CanAddARule() {
      List<string> expected = new() { "id" };
      WebScraper scraper = new("");

      scraper.AddRules("id");
      Assert.Equal(expected, scraper.GetRequired());
    }

    [Fact]
    public void CanAddMultipleRules() {
      List<string> expected = new() { "id", "class" };
      WebScraper scraper = new("");

      scraper.AddRules("id", "class");
      Assert.Equal(expected, scraper.GetRequired());
    }

    [Fact]
    public void CanRemoveARule() {
      string[] required = { "id", "class" };
      List<string> expected = new() { "id" };
      WebScraper scraper = new("", required);

      scraper.RemoveRules("class");
      Assert.Equal(expected, scraper.GetRequired());
    }

    [Fact]
    public void CanRemoveMultipleRules() {
      string[] required = { "id", "class" };
      List<string> expected = new();
      WebScraper scraper = new("", required);

      scraper.RemoveRules("class", "id");
      Assert.Equal(expected, scraper.GetRequired());
    }
  }
}
