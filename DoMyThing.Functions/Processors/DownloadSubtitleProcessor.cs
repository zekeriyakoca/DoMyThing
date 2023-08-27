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
using DoMyThing.Common.Services.Interfaces;
using DoMyThing.Functions.Services;

namespace DoMyThing.Functions.Processors
{
    public class DownloadSubtitleProcessor : IProcessor<DownloadSubtitleModel, DownloadSubtitleResponseModel>
    {
        private readonly string _baseUrl = "https://www.opensubtitles.org";
        private readonly HttpClient client;
        private readonly SubtitleStorageAppService subtitleStorageService;

        public DownloadSubtitleProcessor(IHttpClientFactory clientFactory, SubtitleStorageAppService subtitleStorageService)
        {
            this.client = clientFactory.CreateClient();
            this.client.BaseAddress = new Uri(_baseUrl);
            this.subtitleStorageService = subtitleStorageService;
        }
        public async Task<DownloadSubtitleResponseModel> ProcessAsync(DownloadSubtitleModel request)
        {
            using IPage page = await OpenBrowserPage();

            await SearchSubtitle(request.SearchText, request.LanguageCode, page);

            await PickFirstSubtitleAndNavigateTo(page);
            try
            {
                await TryToNavigateToDownloadPage(page);
            }
            catch
            {
                // We're already in Download Page
            }

            string href = await ExtractDownloadUrl(page);
            string title = await ExtractMovieTitle(page);

            if (String.IsNullOrWhiteSpace(href))
            {
                throw new Exception("Unable to find link to download subtitle!");
            }

            var file = await DownloadSubtitle(href);

            var fileName = await subtitleStorageService.UploadFileAsync(title, file);

            return new DownloadSubtitleResponseModel(fileName, title);
        }

        private async Task<byte[]> DownloadSubtitle(string href)
            => await client.GetByteArrayAsync(href);


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

        private static async Task<string> ExtractMovieTitle(IPage page)
        {
            var doc = new HtmlDocument();
            var content = await page.GetContentAsync();
            doc.LoadHtml(content);
            var title = doc.GetElementbyId("bt-dwl-bt")
                               .GetAttributes(new string[] { "data-product-title" })
                               .SingleOrDefault()?.Value ?? String.Empty;
            return title;
        }


        private async Task TryToNavigateToDownloadPage(IPage page)
        {
            await page.ClickAsync("table#search_results tr:nth-child(2) .bnone");

            await page.WaitForSelectorAsync("#bt-dwl-bt", GetTimeoutOption(10));

            await page.WaitForTimeoutAsync(5);
        }

        private static async Task PickFirstSubtitleAndNavigateTo(IPage page)
        {
            await page.ClickAsync("table#search_results tr:nth-child(2) .bnone");

            await page.WaitForTimeoutAsync(5000);
        }

        private async Task SearchSubtitle(string searchText, string languageCode, IPage page)
        {
            var url = BuildSearchUrl(languageCode, searchText);

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
