using System;
using System.Collections.Generic;
using System.Text;

namespace News.Models
{
    public class news
    {
        public List<News_io> News { get; set; }
    }
    public class currentPage
    {
        public string page { get; set; }
    }

    public class News_io
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string urgent { get; set; }
        public string content { get; set; }
        public string background { get; set; }
    }
}
