using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CsharpV8ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var tasks = new List<Task>();
            for(int i = 0; i < 8; i++)
            { 
             var task = Task.Run(async () =>
              {
                  var HtmlPage = await GetHtmlPage("https://www.youtube.com");
                  var FdIm = ImgExtraction(HtmlPage);
                  await SaveImages(FdIm);
              });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("...");
        }
        private static async Task<string> GetHtmlPage(string uri)
        {
            Console.WriteLine("...");
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
            var FdIm =  FoundImg.Select(X => X.Value).ToList();
            return FdIm;
        }
        static async Task SaveImages(IList <string> imagesUtl)
        {
            var DirName = "img";
            if (!Directory.Exists(DirName))
                Directory.CreateDirectory(DirName);
            using (var httpClient = new HttpClient())
            {
              foreach(var UrlImage in imagesUtl) 
                {
                    Console.WriteLine($"Downloading Image: {UrlImage}");
                    var ImgName = Path.Combine(DirName, $"{Guid.NewGuid()}.jpg");
                    var img = await httpClient.GetByteArrayAsync(UrlImage);
                     await File.WriteAllBytesAsync(ImgName, img);
                }
            }
        }
    }

}