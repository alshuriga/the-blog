using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

namespace MiniBlog.Web.TagHelpers;

public class ModalTagHelper : TagHelper
{
    private IUrlHelperFactory factory;

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    public string BodyText { get; set; } = null!;

    public string HeaderText { get; set; } = null!;

    public string ButtonText { get; set; } = null!;

    public string Id { get; set; } = null!;

    public ModalTagHelper(IUrlHelperFactory _factory)
    {
        factory = _factory;
    }
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        TagBuilder button = new("button");
        button.Attributes.Add("class", "btn btn-primary");
        button.Attributes.Add("data-bs-toggle", "modal");
        button.Attributes.Add("data-bs-target", $"#{Id}");
        button.InnerHtml.Append(ButtonText);

        var modal = await GetModal(output);

        output.Content.Reinitialize();
        output.Content.AppendHtml(button);
        output.Content.AppendHtml(modal);
    }

    private async Task<TagBuilder> GetModal(TagHelperOutput output)
    {
        TagBuilder modal = new("div");
        modal.Attributes.Add("class", "modal");
        modal.Attributes.Add("id", $"{Id}");
        modal.Attributes.Add("tabindex", "-1");
        modal.Attributes.Add("aria-hidden", "true");

        TagBuilder modalDialog = new("div");
        modalDialog.Attributes.Add("class", "modal-dialog modal-dialog-centered");

        TagBuilder modalContent = new("div");
        modalContent.Attributes.Add("class", "modal-content");

        TagBuilder modalBody = new("div");
        modalBody.Attributes.Add("class", "modal-body");
        modalBody.InnerHtml.Append(BodyText);

        TagBuilder modalHeader = new("div");
        modalHeader.Attributes.Add("class", "modal-header");

        TagBuilder modalTitle = new("h5");
        modalTitle.Attributes.Add("class", "modal-title");
        modalTitle.InnerHtml.Append(HeaderText);

        modalHeader.InnerHtml.AppendHtml(modalTitle);

        TagBuilder modalFooter = new("div");
        modalFooter.Attributes.Add("class", "modal-footer");

        TagBuilder closeButton = new("button");
        closeButton.Attributes.Add("class", "btn btn-secondary");
        closeButton.Attributes.Add("data-bs-dismiss", "modal");
        closeButton.InnerHtml.Append("Close");

        modalFooter.InnerHtml.AppendHtml(closeButton);
        modalFooter.InnerHtml.AppendHtml(await output.GetChildContentAsync());

        modalContent.InnerHtml.AppendHtml(modalHeader);
        modalContent.InnerHtml.AppendHtml(modalBody);
        modalContent.InnerHtml.AppendHtml(modalFooter);

        modalDialog.InnerHtml.AppendHtml(modalContent);

        modal.InnerHtml.AppendHtml(modalDialog);

        return modal;
    }
}