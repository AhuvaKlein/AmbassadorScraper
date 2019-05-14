using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AUScraper.Api
{
    public static class AUApi
    {
        public static IEnumerable<Product> GetProducts(string query)
        {
            return SearchProducts(GetHtml(query));
        }

        private static string GetHtml(string query)
        {
            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("user-agent", "amazon is trash");
                var url = $"https://ambassadoruniform.com/catalogsearch/result/?q={query}";
                var html = client.GetStringAsync(url).Result;
                return html;
            }
        }

        private static IEnumerable<Product> SearchProducts(string html)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(html);
            var productDiv = document.QuerySelectorAll("li.item");
            List<Product> products = new List<Product>();
            foreach (var p in productDiv)
            {
                Product product = new Product();
                var atag = p.QuerySelector(".product-name a");
                product.Url = atag.Attributes["href"].Value;
                product.Name = atag.TextContent;
                var pricetag = p.QuerySelector(".price-box .regular-price");
                product.Price = decimal.Parse(pricetag.TextContent.Replace("$",""));
                var image = p.QuerySelector("a img");
                product.ImageUrl = image.Attributes["src"].Value;


                products.Add(product);
            }
            return products;
        }
    }
}
