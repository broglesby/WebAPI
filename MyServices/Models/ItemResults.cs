using System.Collections.Generic;
using System.Linq;

namespace MyServices.Models
{
    public class ItemResults {
        public ItemResults()
        {
            Items = new List<Item>();
        }
        public IEnumerable<Item> Items {get; set;}
        public long TotalCount { get; set; }
    }
}