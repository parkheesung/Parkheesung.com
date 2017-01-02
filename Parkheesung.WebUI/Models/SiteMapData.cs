using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parkheesung.WebUI.Models
{
    public class SiteMapData
    {
        public List<SitemapURL> url { get; set; }

        public static readonly string Daily = "daily";
        public static readonly string Weekly = "weekly";
        public static readonly string Monthly = "monthly";

        public SiteMapData()
        {
            this.url = new List<SitemapURL>();
        }

        public void Add(SitemapURL siteurl)
        {
            this.url.Add(siteurl);
        }
    }

    public class SitemapURL
    {
        public string loc { get; set; }
        public string lastmod { get; set; }
        public string changefreq { get; set; }
        public float priority { get; set; }

        public SitemapURL()
        {
            this.loc = String.Empty;
            this.lastmod = String.Empty;
            this.changefreq = String.Empty;
            this.priority = 0.0f;
        }

        public SitemapURL(string url, string update, string mode, float timer = 0.8f)
        {
            this.loc = url;
            this.lastmod = update;
            this.changefreq = mode;
            this.priority = timer;
        }
    }
}