using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CsharpV8ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var HtmlPage = await GetHtmlPage("https://www.youtube.com");
            var FoundImg = ImgExtraction(HtmlPage);
            Console.WriteLine("...");
        }
        private static async Task<string> GetHtmlPage(string uri)
        {
            using (var httpClient = new HttpClient())
            {
                var HtmlPage = await httpClient.GetStringAsync(uri);
                return HtmlPage;
            }
        }
        private static IList<string> ImgExtraction(string HtmlPage)
        {
            var QueryImgRegex = new Regex(@"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?\.jpg");
            var FoundImg = QueryImgRegex.Matches(HtmlPage);
            return FoundImg.Select(x => x.Value).ToList();
        }
    }

}