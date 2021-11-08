using RuledWebScraper.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace RuledWebScraperTests {
  public class HtmlTests {
    private string sampleLink = "https://cbracco.github.io/html5-test-page/";


    [Fact]
    public void CanCreateScraper() {
      WebScraper scraper = new();

      Assert.NotNull(scraper);
    }

    [SkippableFact]
    public void CanAccessWebpage() {
      WebScraper scraper;
      try {
        scraper = new(sampleLink);

      } catch (Exception e) {
        throw new SkipException("Unable to create WebScraper, cannot test methods", e);
      }
      string html = scraper.GetHtmlFromPage();
      Assert.Contains("<!doctype html>", html.ToLower());
    }

    [Fact]
    public void TagsContainAttribute() {
      WebScraper scraper;
      try {
        scraper = new();
      } catch (Exception e) {
        throw new SkipException("Unable to create WebScraper, cannot test methods", e);
      }
      string html = "<p id='myId'> </p>";
      string attribute = "id";
      bool expected = true;
      Dictionary<int, bool> results;
      bool actual = scraper.AllTagsContainAttributes(html, out results, attribute);

      Assert.Equal(expected, actual);
      Assert.NotEmpty(results);
      Assert.True(results[0]);
    }

    [SkippableFact]
    public void TagsDoNotContainAttribute() {
      WebScraper scraper;
      try {
        scraper = new();
      } catch (Exception e) {
        throw new SkipException("Unable to create WebScraper, cannot test methods", e);
      }
      string html = "<p> </p>";
      string attribute = "id";
      bool expected = false;
      Dictionary<int, bool> results;
      bool actual = scraper.AllTagsContainAttributes(html, out results, attribute);

      Assert.Equal(expected, actual);
      Assert.NotEmpty(results);
      Assert.False(results[0]);
    }

  }
}
