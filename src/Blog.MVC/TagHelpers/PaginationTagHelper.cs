using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

namespace MiniBlog.Web.TagHelpers;

public class PaginationTagHelper : TagHelper
{
    private IUrlHelperFactory factory;

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
    public IQueryCollection Query { get; set; } = null!;
    public PaginationTagHelper(IUrlHelperFactory _factory)
    {
        factory = _factory;
    }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (PageCount <= 1)
        {
            output.SuppressOutput();
            return;
        }
        int paginationFrom = CurrentPage <= 0 ? 0 : CurrentPage - 1;
        int paginationTo = CurrentPage >= (PageCount-1) ? CurrentPage : CurrentPage + 1;
        string firstPageClass = CurrentPage <= 0 ? "disabled" : "";
        string lastPageClass = CurrentPage >= PageCount-1 ? "disabled" : "";
        output.TagName = "ul";
        output.Attributes.SetAttribute("class", "pagination justify-content-center");
        output.Content.AppendHtml(GetTag(0, firstPageClass, "First"));

        for (int i = paginationFrom; i <= paginationTo; i++)
        {
            string isActive = i == CurrentPage ? "active" : "";
            output.Content.AppendHtml(GetTag(i, isActive));
        }

        output.Content.AppendHtml(GetTag(PageCount-1, lastPageClass, "Last"));
    }

    private TagBuilder GetTag(int linkPage, string pageClass = "", string? aContent = null)
    {
        IUrlHelper helper = factory.GetUrlHelper(ViewContext);
        var routeData = Query.ToDictionary(q => q.Key, q => q.Value.ToString());
        routeData["currentPage"] = linkPage.ToString();
        string? url = helper.ActionLink(values: routeData);
        var a = new TagBuilder("a");
        a.Attributes.Add("href", $"{url}");
        a.Attributes.Add("class", "page-link");
        a.InnerHtml.Append(aContent ?? (linkPage + 1).ToString());
        var li = new TagBuilder("li");
        li.Attributes.Add("class", $"page-item {pageClass}");
        li.InnerHtml.AppendHtml(a);
        return li;
    }
}