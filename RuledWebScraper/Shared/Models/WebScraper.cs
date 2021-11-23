using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RuledWebScraper.Shared.Models {
   public class WebScraper {
      private List<string> required;
      private string url;
      public WebScraper(string url, params string[] required)
      {
         SetRequired(required);
         this.url = url;
      }

      public async Task<string> GetHtmlFromPage()
      {
         string html = null;
         try
         {
            HttpClient client = new();

            html = await client.GetStringAsync(GetUrl());
         }
         catch (Exception e)
         {
            Console.WriteLine("Hit exception " + e.Message);
         }
         return html;
      }

      public Dictionary<int, TestedTag> TestAllTags(string html, params string[] attributes)
      {
         Dictionary<int, TestedTag> results = new Dictionary<int, TestedTag>();
         List<HtmlNode> allTags = GetAllTags(html);
         int numOfTags = allTags.Count;

         for (int tagIndex = 0; tagIndex < numOfTags; tagIndex++)
         {
            bool tagContainedAttributes = true;
            List<string> missingAttributes = new List<string>();

            HtmlNode currentTag = allTags[tagIndex];

            for (int attributeIndex = 0; attributeIndex < attributes.Length; attributeIndex++)
            {

               string currentAttribute = attributes[attributeIndex];
               if (!currentTag.Attributes.Contains(currentAttribute))
               {
                  tagContainedAttributes = false;
                  missingAttributes.Add(currentAttribute);
               }
            }
            string tagToString = currentTag.OuterHtml;
            string inner = currentTag.InnerHtml;
            if (inner.Length > 0)
            {
               tagToString = currentTag.OuterHtml.Replace(inner, " ... ");
            }
            TestedTag testedTag = new TestedTag(tagToString, missingAttributes, tagContainedAttributes);
            results.Add(tagIndex, testedTag);
         }

         return results;
      }

      public List<HtmlNode> GetAllTags(string html)
      {
         List<HtmlNode> allTags = new List<HtmlNode>();
         HtmlDocument doc = new HtmlDocument();
         doc.LoadHtml(html);
         IEnumerable<HtmlNode> coll = doc.DocumentNode.Descendants(HtmlDocument.MaxDepthLevel);

         foreach (HtmlNode node in coll)
         {
            string nodeText = node.OuterHtml;
            if (nodeText.StartsWith("<") && !nodeText.StartsWith("<!--") && !nodeText.StartsWith("<!doctype") && !nodeText.StartsWith("<meta"))
            {
               allTags.Add(node);
            }
         }

         return allTags;
      }

      public string GetUrl()
      {
         return url;
      }

      public void SetUrl(string url)
      {
         if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
         {
            throw new ArgumentException("Must pass in a valid URL");
         }
         this.url = url;
      }

      #region Alter Rules

      public List<string> GetRequired()
      {
         return new List<string>(this.required.ToArray());
      }

      public void SetRequired(params string[] required)
      {
         this.required = new List<string>(required.ToArray());
      }

      public void AddRules(params string[] required)
      {
         this.required.AddRange(required);
      }

      public void RemoveRules(params string[] toRemove)
      {
         this.required.RemoveAll(x => toRemove.Contains(x));
      }

      #endregion
   }
}