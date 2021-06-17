using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using verge_app.Models;

namespace verge_app.Controllers
{
    public class HomeController : Controller
    {
        public ArticleModel article_model;

        public ActionResult Index()
        {
            article_model = new ArticleModel();

            //convert url to xml
            XmlDocument xml_doc = new XmlDocument();
            xml_doc.Load(article_model.article_url);
            foreach (XmlNode xml_nodes in xml_doc.ChildNodes)
            {
                if (xml_nodes.Name == "feed")
                {
                    foreach (XmlNode xml_feed in xml_nodes.ChildNodes)
                    {
                        if (xml_feed.Name == "entry")
                        {
                            Articles articles = new Articles();
                            articles.article_title = xml_feed["title"].InnerText;
                            articles.article_link = xml_feed["id"].InnerText;
                            string article_content = xml_feed["content"].InnerText;
                            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                            doc.LoadHtml(article_content);
                            articles.article_img_src = doc.DocumentNode.SelectSingleNode("//img").Attributes["src"].Value;
                            articles.article_summary = doc.DocumentNode.SelectSingleNode("//p/text()").OuterHtml;
                            articles.article_publish_date = DateTime.Parse(xml_feed["published"].InnerText).ToString("dd MMM yyyy");
                            XmlNode xml_author = xml_feed["author"];
                            foreach (XmlNode xml_names in xml_author.ChildNodes)
                            {
                                if (string.IsNullOrEmpty(articles.article_author))
                                {
                                    articles.article_author = xml_names.InnerText;
                                }
                                else
                                {
                                    articles.article_author += ", " + xml_names.InnerText;
                                }
                            }

                            article_model.list_articles.Add(articles);
                        }
                    }
                }
            }
            Session.Add("Articles", article_model);
            return View();
        }

        [HttpPost]
        public JsonResult getArticles(string searchtitle)
        {
            article_model = (ArticleModel)Session["Articles"];

            var results = string.IsNullOrEmpty(searchtitle)? article_model.list_articles  :  article_model.list_articles.Where(x => x.article_title.Contains(searchtitle));
            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}