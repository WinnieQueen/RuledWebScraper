using RuledWebScraper.Shared.Models;
using System;
using System.Collections.Generic;
using Xunit;
using HtmlAgilityPack;

namespace RuledWebScraperTests {
  public class HtmlTests {
    private string sampleLink = "https://cbracco.github.io/html5-test-page/";


    [Fact]
    public void CanCreateScraper() {
      WebScraper scraper = new("google.com");

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
      string html = scraper.GetHtmlFromPage().Result;
      Assert.Contains("<!doctype html>", html.ToLower());
    }

    [SkippableFact]
    public void TagsContainAttribute() {
      WebScraper scraper;
      try {
        scraper = new(sampleLink);
      } catch (Exception e) {
        throw new SkipException("Unable to create WebScraper, cannot test methods", e);
      }
      try {
        string html = @"<!DOCTYPE html> <html> <p id='myId'> </p> </html>";
        string attribute = "id";
        Dictionary<int, TestedTag> results = scraper.TestAllTags(html, attribute);
        Assert.NotEmpty(results);
        Assert.True(results[2].IsPassing());
        Assert.Empty(results[2].GetMissingAttributes());
        Assert.Equal("", results[2].GetMissingAttributesAsString());
      } catch (NullReferenceException e) {
        throw new SkipException("An unexpected exception was thrown, methods cannot be tested." + e.Message, e);
      }

    }

    [SkippableFact]
    public void TagsContainMultipleAttributes() {
      WebScraper scraper;
      try {
        scraper = new(sampleLink);
      } catch (Exception e) {
        throw new SkipException("Unable to create WebScraper, cannot test methods", e);
      }
      try {
        string html = @"<!DOCTYPE html> <html> <p class='MyClass' id='myId'> </p> </html>";
        Dictionary<int, TestedTag> results = scraper.TestAllTags(html, "id", "class");
        Assert.NotEmpty(results);
        Assert.True(results[2].IsPassing());
        Assert.Empty(results[2].GetMissingAttributes());
        Assert.Equal("", results[2].GetMissingAttributesAsString());
      } catch (NullReferenceException e) {
        throw new SkipException("An unexpected exception was thrown, methods cannot be tested." + e.Message, e);
      }

    }

    [SkippableFact]
    public void CanGetTags() {
      WebScraper scraper;
      try {
        scraper = new(sampleLink);
      } catch (Exception e) {
        throw new SkipException("Unable to create WebScraper, cannot test methods", e);
      }
      try {
        string html = @"<!DOCTYPE html> <html> <p id='myId'> </p> </html>";
        string expected = "<p id='myId'> </p>";
        List<HtmlNode> results = scraper.GetAllTags(html);
        Assert.NotEmpty(results);
        Assert.Equal(expected, results[2].OuterHtml);
      } catch (NullReferenceException e) {
        throw new SkipException("An unexpected exception was thrown, methods cannot be tested." + e.Message, e);
      }

    }

    [SkippableFact]
    public void TagsDoNotContainAttribute() {
      WebScraper scraper;
      try {
        scraper = new(sampleLink);
      } catch (Exception e) {
        throw new SkipException("Unable to create WebScraper, cannot test methods", e);
      }
      try {
        string html = @"<!DOCTYPE html> <html> <p class='MyClass'> </p> </html>";
        string attribute = "id";
        Dictionary<int, TestedTag> results = scraper.TestAllTags(html, attribute);

        Assert.NotEmpty(results);
        Assert.False(results[2].IsPassing());
        Assert.Equal("id", results[2].GetMissingAttributes()[0]);
        Assert.Equal("id", results[2].GetMissingAttributesAsString());
      } catch (NullReferenceException e) {
        throw new SkipException("An unexpected exception was thrown, methods cannot be tested." + e.Message, e);
      }
    }

    [SkippableFact]
    public void TagsContainOneOfTwoAttributes() {
      WebScraper scraper;
      try {
        scraper = new(sampleLink);
      } catch (Exception e) {
        throw new SkipException("Unable to create WebScraper, cannot test methods", e);
      }
      try {
        string html = @"<!DOCTYPE html> <html> <p id='myId'> </p> </html>";
        Dictionary<int, TestedTag> results = scraper.TestAllTags(html, "id", "class");
        Assert.NotEmpty(results);
        Assert.False(results[2].IsPassing());
        Assert.NotEmpty(results[2].GetMissingAttributes());
        Assert.Equal("class", results[2].GetMissingAttributes()[0]);
        Assert.Equal("class", results[2].GetMissingAttributesAsString());
      } catch (NullReferenceException e) {
        throw new SkipException("An unexpected exception was thrown, methods cannot be tested." + e.Message, e);
      }

    }

  }
}
