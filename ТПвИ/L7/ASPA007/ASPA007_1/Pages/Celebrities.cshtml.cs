using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL_Celebrity;
using ANC25_WEBAPI_DLL;
using Microsoft.Extensions.Options;

namespace ASPA007_1.Pages;

public class CelebritiesModel : PageModel {
    private readonly IRepository _repo;
    public string PhotosRequestPath { get; }

    public List<Celebrity> Celebrities { get; set; } = new();

    public CelebritiesModel(IRepository repo, IOptions<CelebritiesConfig> config) {
        _repo = repo;
        PhotosRequestPath = config.Value.PhotosRequestPath;
    }

    public void OnGet() {
        Celebrities = _repo.GetAllCelebrities();
    }
}
