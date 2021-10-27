using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuledWebScraper.Shared.Models {
  public class WebScraper {
    private List<string> required;

    public WebScraper() {
      this.required = new List<string>();
    }

    public WebScraper(List<string> required) {
      this.required = required;
    }

    public List<string> GetRequired() {
      return new List<string>(this.required.ToArray());
    }

    public void AddRules(params string[] required) {
      this.required.AddRange(required);
    }

    public void RemoveRules(params string[] toRemove) {
      this.required.RemoveAll(x => toRemove.Contains(x));
    }
  }
}
