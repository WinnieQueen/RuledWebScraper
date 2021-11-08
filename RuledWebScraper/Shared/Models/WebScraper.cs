using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RuledWebScraper.Shared.Models {
  public class WebScraper {
    private List<string> required;
    private string url;

    public WebScraper() {
      this.required = new List<string>();
      this.url = "";
    }

    public WebScraper(string url, params string[] required) {
      SetRequired(required);
      SetUrl(url);
    }

    public string GetHtmlFromPage() {
      string html = null;
      try {
        WebClient client = new();
        client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

        Stream data = client.OpenRead(url);
        StreamReader reader = new StreamReader(data);
        html = reader.ReadToEnd();
        data.Close();
        reader.Close();
      } catch (Exception) {

      }
      return html;
    }

    public bool AllTagsContainAttributes(string html, out Dictionary<int, bool> results, params string[] attributes) {
      results = new();
      List<string> allTags = GetAllTags(html);
      int numOfTags = allTags.Count();
      bool allTagsContainedAttributes = true;

      for (int currentTag = 0; currentTag < numOfTags; currentTag++) {
        bool tagContainedAttributes = true;

        foreach (string attribute in attributes) {

          if (!allTags[currentTag].Contains(attribute)) {
            tagContainedAttributes = false;
            allTagsContainedAttributes = false;
            break;
          }

        }

        results.Add((currentTag), tagContainedAttributes);
      }

      return allTagsContainedAttributes;
    }

    public List<string> GetAllTags(string html) {
      HtmlDocument doc = new();
      doc.LoadHtml(html);
      foreach (HtmlNode node in doc.DocumentNode.ChildNodes) {
        Debug.WriteLine(node.OuterHtml);
      }
      return null;
    }

    public string GetUrl() {
      return url;
    }

    public void SetUrl(string url) {
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
