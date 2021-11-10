using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RuledWebScraper.Shared.Models {
  public class WebScraper {
    private List<string> required;
    private string url;

    public WebScraper(string url, params string[] required) {
      SetRequired(required);
      this.url = url;
    }

    public async Task<string> GetHtmlFromPage() {
      string html = null;
      try {
        HttpClient client = new();
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

        html = await client.GetStringAsync(GetUrl());
      } catch (Exception e) {
        Console.WriteLine("Hit exception " + e.Message);
      }
      return html;
    }

    public bool AllTagsContainAttributes(string html, out Dictionary<int, bool> results, params string[] attributes) {
      results = new();
      List<string> allTags = GetAllTags(html);
      int numOfTags = allTags.Count;
      bool allTagsContainedAttributes = true;

      for (int currentTagIndex = 0; currentTagIndex < numOfTags; currentTagIndex++) {
        bool tagContainedAttributes = true;

          if(!attributes.All(allTags[currentTagIndex].Contains)) {
            tagContainedAttributes = false;
            allTagsContainedAttributes = false;
          }

        results.Add(currentTagIndex, tagContainedAttributes);
      }

      return allTagsContainedAttributes;
    }

    public List<string> GetAllTags(string html) {
      List<string> allTags = new List<string>();
      string pattern = @"<((\w*)( \w*=(""|')\w*(""|'))*)> ";
      var matches = Regex.Matches(html, pattern);

      for (int i = 0; i < matches.Count; i++) {

        allTags.Add(matches[i].ToString());
        Console.WriteLine(allTags[i]);

      }
      return allTags;
    }

    public string GetUrl() {
      return url;
    }

    public void SetUrl(string url) {
      if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) {
        throw new ArgumentException("Must pass in a valid URL");
      }
      this.url = url;
    }

    #region Alter Rules

    public List<string> GetRequired() {
      return new List<string>(this.required.ToArray());
    }

    public void SetRequired(params string[] required) {
      this.required = new List<string>(required.ToArray());
    }

    public void AddRules(params string[] required) {
      this.required.AddRange(required);
    }

    public void RemoveRules(params string[] toRemove) {
      this.required.RemoveAll(x => toRemove.Contains(x));
    }

    #endregion

  }
}
