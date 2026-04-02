using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL_Celebrity;
using ANC25_WEBAPI_DLL;
using Microsoft.Extensions.Options;

namespace ASPA007_1.Pages;

public class CelebrityModel : PageModel {
    private readonly IRepository _repo;
    public string PhotosRequestPath { get; }

    public Celebrity? Celebrity { get; set; }
    public List<Lifeevent> Events { get; set; } = new();

    public CelebrityModel(IRepository repo, IOptions<CelebritiesConfig> config) {
        _repo = repo;
        PhotosRequestPath = config.Value.PhotosRequestPath;
    }

    public void OnGet(int id) {
        Celebrity = _repo.GetCelebrityById(id);
        if (Celebrity != null) {
            Events = _repo.GetLifeeventsByCelebrityId(id);
        }
    }
}
