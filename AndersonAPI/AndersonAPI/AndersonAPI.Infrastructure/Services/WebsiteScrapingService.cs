using AndersonAPI.Application.Common.ConfigOptions;
using AndersonAPI.Application.Common.Interfaces;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Microsoft.Extensions.Options;
using PuppeteerSharp;
using System.Text;
using System.Text.RegularExpressions;

namespace AndersonAPI.Infrastructure.Services
{
    public class WebsiteScrapingService: IWebsiteScrapingService
    {
        private readonly HttpClient _httpClient;
        private readonly AzureAIOptions _options;

        public WebsiteScrapingService(HttpClient httpClient, IOptions<AzureAIOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<string> ScrapeWebsiteAsync(string url, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("URL must not be empty", nameof(url));

            //using var response = await _httpClient.GetAsync(url, cancellationToken);
            //response.EnsureSuccessStatusCode();
            //var content = await response.Content.ReadAsStringAsync(cancellationToken);
            //return content;
            await new BrowserFetcher().DownloadAsync();

            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            using var page = await browser.NewPageAsync();

            await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);
            var html = await page.GetContentAsync();

            var parser = new HtmlParser();
            IHtmlDocument document;
            
            try
            {
                document = await parser.ParseDocumentAsync(html, cancellationToken);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Url not found");
            }

            foreach( var n in document.QuerySelectorAll("script, style, noscript, template, svg"))
            {
                n.Remove();
            }

            var md = ToMarkdown(document.Body);
            md = NormalizeMarkdown(md.ToString() ?? string.Empty);

            return md.ToString() ?? string.Empty;
        }

        private object ToMarkdown(IHtmlElement? root)
        {
            if (root is null) return "";

            var sb = new StringBuilder();
            RenderNode(root, sb, listIndent: 0, orderedIndex: 1);
            return sb.ToString();
        }

        private void RenderNode(INode node, StringBuilder sb, int listIndent, int orderedIndex)
        {
            switch (node.NodeType)
            {
                case NodeType.Text:
                    var text = CollapseInlineWhitespace(node.TextContent);
                    if (!string.IsNullOrWhiteSpace(text))
                        sb.Append(text);
                    return;

                case NodeType.Element:
                    break;

                default:
                    foreach (var child in node.ChildNodes)
                        RenderNode(child, sb, listIndent, orderedIndex);
                    return;
            }

            var el = (IElement)node;
            var tag = el.TagName.ToLowerInvariant();

            // skip hidden-ish content (optional heuristic)
            var style = (el.GetAttribute("style") ?? "").ToLowerInvariant();
            if (style.Contains("display:none") || style.Contains("visibility:hidden"))
                return;

            if (tag is "h1" or "h2" or "h3" or "h4" or "h5" or "h6")
            {
                var level = int.Parse(tag.Substring(1, 1));
                sb.AppendLine();
                sb.Append(new string('#', level)).Append(' ');
                foreach (var child in el.ChildNodes) RenderNode(child, sb, 0, 1);
                sb.AppendLine().AppendLine();
                return;
            }

            if (tag == "p")
            {
                sb.AppendLine();
                foreach (var child in el.ChildNodes) RenderNode(child, sb, listIndent, orderedIndex);
                sb.AppendLine().AppendLine();
                return;
            }

            if (tag == "br")
            {
                sb.AppendLine();
                return;
            }

            if (tag == "a")
            {
                var href = el.GetAttribute("href") ?? "";
                var inner = new StringBuilder();
                foreach (var child in el.ChildNodes) RenderNode(child, inner, listIndent, orderedIndex);
                var label = inner.ToString().Trim();

                if (string.IsNullOrWhiteSpace(label))
                    label = href;

                if (!string.IsNullOrWhiteSpace(href))
                    sb.Append('[').Append(label).Append("](").Append(href).Append(')');
                else
                    sb.Append(label);

                return;
            }

            if (tag == "ul" || tag == "ol")
            {
                sb.AppendLine();
                int idx = 1;
                foreach (var li in el.Children)
                {
                    if (!li.TagName.Equals("LI", StringComparison.OrdinalIgnoreCase)) continue;
                    RenderListItem(li, sb, listIndent, tag == "ol", idx++);
                }
                sb.AppendLine();
                return;
            }

            // default: recurse
            foreach (var child in el.ChildNodes)
                RenderNode(child, sb, listIndent, orderedIndex);
        }

        private void RenderListItem(IElement li, StringBuilder sb, int indent, bool ordered, int number)
        {
            var prefix = ordered ? $"{number}. " : "- ";
            sb.Append(new string(' ', indent)).Append(prefix);

            // render li children
            var inner = new StringBuilder();
            foreach (var child in li.ChildNodes)
                RenderNode(child, inner, indent + 2, 1);

            var content = inner.ToString().Trim();
            // keep it on one line if possible
            content = Regex.Replace(content, @"\s*\n\s*", " ");
            sb.AppendLine(content);
        }

        private static string CollapseInlineWhitespace(string s)
            => Regex.Replace(s, @"\s+", " ");

        private static string NormalizeMarkdown(string s)
        {
            s = s.Replace("\r\n", "\n");
            s = Regex.Replace(s, @"[ \t]+\n", "\n");
            s = Regex.Replace(s, @"\n{3,}", "\n\n");
            return s.Trim();
        }
    }
}
