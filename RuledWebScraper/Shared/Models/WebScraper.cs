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
         List<string> allTags = GetAllTags(html);
         int numOfTags = allTags.Count;

         for (int tagIndex = 0; tagIndex < numOfTags; tagIndex++)
         {
            bool tagContainedAttributes = true;
            List<string> missingAttributes = new List<string>();

            string currentTag = allTags[tagIndex];

            Console.WriteLine(tagIndex + ": " + currentTag);
            for (int attributeIndex = 0; attributeIndex < attributes.Length; attributeIndex++)
            {

               string currentAttribute = attributes[attributeIndex];
               if (!currentTag.Contains(currentAttribute))
               {
                  tagContainedAttributes = false;
                  missingAttributes.Add(currentAttribute);
               }
            }

            TestedTag testedTag = new TestedTag(currentTag, missingAttributes, tagContainedAttributes);
            results.Add(tagIndex, testedTag);
         }

         return results;
      }

      public List<string> GetAllTags(string html)
      {
         List<string> allTags = new List<string>();
         HtmlDocument doc = new HtmlDocument();
         doc.LoadHtml(html);
         IEnumerable<HtmlNode> coll = doc.DocumentNode.Descendants(HtmlDocument.MaxDepthLevel);

         List<string> nodeList = new List<string>();
         LevelOneNodes(coll, ref nodeList);

         foreach (string node in nodeList)
         {
            allTags.Add(node);
            Console.WriteLine("Node: " + node);
         }

         return allTags;
      }

      private void LevelOneNodes(IEnumerable<HtmlNode> nodeList, ref List<string> finalList)
      {
         foreach (HtmlNode node in nodeList)
         {
            if (node.OuterHtml.StartsWith("<") && !node.OuterHtml.StartsWith("<!--"))
            {
               if (node.HasChildNodes)
               {
                  LevelOneNodes(node.ChildNodes, ref finalList);
               }
               else
               {
                  finalList.Add(node.OuterHtml);
               }
            }
         }
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