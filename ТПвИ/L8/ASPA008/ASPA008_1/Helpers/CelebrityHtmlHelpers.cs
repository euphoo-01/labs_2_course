using ANC25_WEBAPI_DLL;
using DAL_Celebrity;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace ASPA008_1.Helpers;

public static class CelebrityHtmlHelpers
{
    public static IHtmlContent CelebrityFoto(this IHtmlHelper html, Celebrity celebrity, string cssClass = "celebrity-photo")
    {
        var config = html.ViewContext.HttpContext.RequestServices
            .GetRequiredService<IOptions<CelebritiesConfig>>().Value;
        var src = $"{config.PhotosRequestPath.TrimEnd('/')}/{celebrity.ReqPhotoPath}";
        var tag = new TagBuilder("img");
        tag.Attributes["src"] = src;
        tag.Attributes["alt"] = celebrity.FullName;
        tag.AddCssClass(cssClass);
        return tag;
    }
}
