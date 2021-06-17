using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace verge_app.Models
{
    public class ArticleModel
    {
        public List<Articles> list_articles = new List<Articles>();

        public string article_url = "https://www.theverge.com/rss/front-page/index.xml";

        public ArticleModel()
        {

        }
    }

    public class Articles
    {
        public string article_title { get; set; } 
        public string article_img_src { get; set; } 
        public string article_author { get; set; } 
        public string article_publish_date { get; set; }
        public string article_summary { get; set; }
        public string article_link { get; set; }
    }
}