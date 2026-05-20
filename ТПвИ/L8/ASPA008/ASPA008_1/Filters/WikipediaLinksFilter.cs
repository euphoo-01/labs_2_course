using ASPA008_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASPA008_1.Filters;

public sealed class WikipediaLinksFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Controller is not Controller controller ||
            context.Result is not ViewResult view ||
            view.Model is not CelebrityDetailsViewModel model)
        {
            return;
        }

        var name = model.Celebrity.FullName;
        var slug = Uri.EscapeDataString(name.Replace(' ', '_'));
        model.WikipediaLinks = new[]
        {
            new WikipediaLink(name, $"https://en.wikipedia.org/wiki/{slug}"),
            new WikipediaLink($"{name} search", $"https://en.wikipedia.org/w/index.php?search={Uri.EscapeDataString(name)}")
        };

        controller.ViewData["WikipediaLinks"] = model.WikipediaLinks;
    }
}
