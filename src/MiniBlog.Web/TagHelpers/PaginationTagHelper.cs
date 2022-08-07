using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using MiniBlog.Core.Models;

namespace MiniBlog.Web.TagHelpers;

public class PaginationTagHelper : TagHelper
{
    private IUrlHelperFactory factory;

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;
    public PaginationData PaginationData { get; set; } = null!;
    public PaginationTagHelper(IUrlHelperFactory _factory)
    {
        factory = _factory;
    }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        int paginationFrom = PaginationData.CurrentPage <= 1 ? 1 : PaginationData.CurrentPage - 1;
        int paginationTo = PaginationData.CurrentPage >= PaginationData.PageNumber ? PaginationData.CurrentPage : PaginationData.CurrentPage + 1;
        string firstPageClass = PaginationData.CurrentPage <= 1 ? "disabled" : "";
        string lastPageClass = PaginationData.CurrentPage >= PaginationData.PageNumber ? "disabled" : "";
        output.TagName = "ul";
        output.Attributes.SetAttribute("class", "pagination justify-content-center");
        if (PaginationData.PageNumber > 1) output.Content.AppendHtml(GetTag(1, firstPageClass, "First"));

        for (int i = paginationFrom; i <= paginationTo; i++)
        {
            string isActive = i == PaginationData.CurrentPage ? "active" : "";
            output.Content.AppendHtml(GetTag(i, isActive));
        }

        output.Content.AppendHtml(GetTag(PaginationData.PageNumber, lastPageClass, "Last"));
    }

    private TagBuilder GetTag(int linkPage, string pageClass = "", string? aContent = null)
    {
        IUrlHelper helper = factory.GetUrlHelper(ViewContext);
        string? url = helper.ActionLink(values: new { currentPage = linkPage, tagName = ViewContext.RouteData.Values["tagName"] });
        var a = new TagBuilder("a");
        a.Attributes.Add("href", $"{url}");
        a.Attributes.Add("class", "page-link");
        a.InnerHtml.Append(aContent ?? linkPage.ToString());
        var li = new TagBuilder("li");
        li.Attributes.Add("class", $"page-item {pageClass}");
        li.InnerHtml.AppendHtml(a);
        return li;
    }
}