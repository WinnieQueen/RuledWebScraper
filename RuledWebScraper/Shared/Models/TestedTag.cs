using System.Collections.Generic;

namespace RuledWebScraper.Shared.Models {
  public class TestedTag {
    private string tag = "";
    private List<string> missingAttributes;
    private bool passed;

    public TestedTag(string tag, List<string> missingAttributes, bool passed) {
      this.tag = tag;
      this.missingAttributes = missingAttributes;
      this.passed = passed;
    }

    public string GetTag() {
      return tag;
    }
    public string[] GetMissingAttributes() {
      return missingAttributes.ToArray();
    }
    public string GetMissingAttributesAsString() {
      string allAtts = "";
      if (missingAttributes.Count > 0) {
        allAtts = string.Join(", ", missingAttributes);
      }
      return allAtts;
    }
    public bool IsPassing() {
      return passed;
    }
  }
}
