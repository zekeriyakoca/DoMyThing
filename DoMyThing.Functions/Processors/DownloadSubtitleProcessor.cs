using AngleSharp.Dom;
using DoMyThing.Functions.Models;
using PuppeteerSharp;
using HtmlAgilityPack;
using PuppeteerSharp.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DoMyThing.Functions.Processors
{
    public class DownloadSubtitleProcessor : IProcessor<DownloadSubtitleModel>
    {
        private readonly string _baseUrl = "https://www.opensubtitles.org";
        private const string TRCODE = "tur";
        private const string ENCODE = "en";
        public DownloadSubtitleProcessor()
        {

        }
        public async Task ProcessAsync(DownloadSubtitleModel request)
        {
            IPage page = await OpenBrowserPage();

            await SearchSubtitle(request.SearchText, page);

            await PickFirstSubtitleAndNavigateTo(page);

            await NavigateToDownloadPage(page);

            string href = await ExtractDownloadUrl(page);

            if (href == String.Empty)
            {
                throw new Exception("Unable to find link to download subtitle!");
            }

            var file = await DownloadSubtitle(href);

            // TODO : Store into blob storage
        }

        private async Task<byte[]> DownloadSubtitle(string href)
        {
            byte[] fileInByteArrays = new byte[0];
            using (HttpClient client = new HttpClient())
            {
                fileInByteArrays = await client.GetByteArrayAsync(_baseUrl + href);
            }
            return fileInByteArrays;
        }

        private static async Task<string> ExtractDownloadUrl(IPage page)
        {
            var doc = new HtmlDocument();
            var content = await page.GetContentAsync();
            doc.LoadHtml(content);
            var href = doc.GetElementbyId("bt-dwl-bt")
                                   .GetAttributes(new string[] { "href" })
                                   .SingleOrDefault()?.Value ?? String.Empty;
            return href;
        }

        private async Task NavigateToDownloadPage(IPage page)
        {
            await page.ClickAsync("table#search_results tr:nth-child(2) .bnone");

            await page.WaitForSelectorAsync("#bt-dwl-bt", GetTimeoutOption(10));

            await page.WaitForTimeoutAsync(3000);
        }

        private static async Task PickFirstSubtitleAndNavigateTo(IPage page)
        {
            await page.ClickAsync("table#search_results tr:nth-child(2) .bnone");

            //var sortByDownloadUrlSuffix = "/sort-7/asc-0";
            //await page.GoToAsync( page.Url + sortByDownloadUrlSuffix);

            await page.WaitForTimeoutAsync(3000);
        }

        private async Task SearchSubtitle(string searchText, IPage page)
        {
            var url = BuildSearchUrl(TRCODE, searchText);

            await page.GoToAsync(url, new NavigationOptions() { WaitUntil = new[] { WaitUntilNavigation.DOMContentLoaded }, Timeout = 0 });
            await page.WaitForSelectorAsync("table#search_results", GetTimeoutOption(10));
        }

        private static async Task<IPage> OpenBrowserPage()
        {
            var browser = await PuppeteerExtentions.CreateLocalPuppeteerAsync();
            var page = await browser.NewPageAsync();
            return page;
        }

        private string BuildSearchUrl(string langCode, string movieName) => $"{_baseUrl}/en/search2/sublanguageid-{langCode}/moviename-{EncodeMovieName(movieName)}";

        private string EncodeMovieName(string movieName) => String.Join("+", movieName.Split(" "));
        private WaitForSelectorOptions GetTimeoutOption(int sec) => new WaitForSelectorOptions() { Timeout = TimeSpan.FromSeconds(sec).Microseconds };
    }
}
