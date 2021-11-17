using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        html = await client.GetStringAsync(GetUrl());
      } catch (Exception e) {
        Console.WriteLine("Hit exception " + e.Message);
      }
      return html;
    }

    public Dictionary<int, TestedTag> TestAllTags(string html, params string[] attributes) {
      Dictionary<int, TestedTag> results = new Dictionary<int, TestedTag>();
      List<string> allTags = GetAllTags(html);
      int numOfTags = allTags.Count;
      Console.WriteLine(html);

      for (int tagIndex = 0; tagIndex < numOfTags; tagIndex++) {
        bool tagContainedAttributes = true;
        List<string> missingAttributes = new List<string>();

        string currentTag = allTags[tagIndex];
        for (int attributeIndex = 0; attributeIndex < attributes.Length; attributeIndex++) {

          string currentAttribute = attributes[attributeIndex];
          if (!currentTag.Contains(currentAttribute)) {
            tagContainedAttributes = false;
            missingAttributes.Add(currentAttribute);
          }
        }

        TestedTag testedTag = new TestedTag(currentTag, missingAttributes, tagContainedAttributes);
        results.Add(tagIndex, testedTag);
      }

      return results;
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