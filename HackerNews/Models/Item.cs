using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Models
{
    class Item
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string By { get; set; }
        public int Id { get; set; }
        public int Score { get; set; }
        public long Time { get; set; }
        public string Type { get; set; }
        public int[] Kids { get; set; }
        public int Descendants { get; set; }

        public bool IsHighInterest { get { return Score > 100 && Kids.Count() >= 50; } }
    }
}
