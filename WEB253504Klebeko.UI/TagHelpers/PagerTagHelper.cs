using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using WEB253504Klebeko.Domain.Entities;

namespace WEB253504Klebeko.UI.TagHelpers
{
    [HtmlTargetElement("Pager")]
    public class PagerTagHelper : TagHelper
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HtmlAttributeName("current-page")]
        public int CurrentPage { get; set; }

        [HtmlAttributeName("total-pages")]
        public int TotalPages { get; set; }

        [HtmlAttributeName("category")]
        public string? Category { get; set; }

        [HtmlAttributeName("admin")]
        public bool IsAdmin { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "nav";
            output.Attributes.SetAttribute("aria-label", "Page navigation");

            var ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("pagination");

            var prevLiTag = new TagBuilder("li");
            prevLiTag.AddCssClass(CurrentPage == 1 ? "page-item disabled" : "page-item");

            var prevATag = new TagBuilder("a");
            prevATag.AddCssClass("page-link");
            prevATag.Attributes["href"] = GenerateUrl(CurrentPage > 1 ? CurrentPage - 1 : 1);
            prevATag.InnerHtml.Append("<"); 

            prevLiTag.InnerHtml.AppendHtml(prevATag);
            ulTag.InnerHtml.AppendHtml(prevLiTag);

            for (int i = 1; i <= TotalPages; i++)
            {
                var liTag = new TagBuilder("li");
                liTag.AddCssClass(i == CurrentPage ? "page-item active" : "page-item");

                var aTag = new TagBuilder("a");
                aTag.AddCssClass("page-link");
                aTag.Attributes["href"] = GenerateUrl(i);
                aTag.InnerHtml.Append(i.ToString());

                liTag.InnerHtml.AppendHtml(aTag);
                ulTag.InnerHtml.AppendHtml(liTag);
            }

            var nextLiTag = new TagBuilder("li");
            nextLiTag.AddCssClass(CurrentPage == TotalPages ? "page-item disabled" : "page-item");

            var nextATag = new TagBuilder("a");
            nextATag.AddCssClass("page-link");
            nextATag.Attributes["href"] = GenerateUrl(CurrentPage < TotalPages ? CurrentPage + 1 : TotalPages);
            nextATag.InnerHtml.Append(">"); 

            nextLiTag.InnerHtml.AppendHtml(nextATag);
            ulTag.InnerHtml.AppendHtml(nextLiTag);

            output.Content.AppendHtml(ulTag);
        }

        private string GenerateUrl(int pageNumber)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            
            if (IsAdmin)
            {
                return $"/Admin/Index?pageNo={pageNumber}";
            }
            else
            {
                var routeValues = new { category = Category, pageNo = pageNumber };

                return _linkGenerator.GetPathByAction(
                    httpContext,
                    action: "Index",
                    controller: "Medicine",
                    values: routeValues
                );
            }
        }
    }
}
