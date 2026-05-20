using DAL_Celebrity;

namespace ASPA008_1.Models;

public sealed class CelebrityDetailsViewModel
{
    public Celebrity Celebrity { get; set; } = new();
    public IReadOnlyList<Lifeevent> Lifeevents { get; set; } = [];
    public IReadOnlyList<WikipediaLink> WikipediaLinks { get; set; } = [];
}

public sealed record WikipediaLink(string Title, string Url);
