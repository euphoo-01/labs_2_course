using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL_Celebrity;
using ANC25_WEBAPI_DLL;
using Microsoft.Extensions.Options;

namespace ASPA007_1.Pages;

public class NewCelebrityModel : PageModel {
    private readonly IRepository _repo;
    private readonly CelebritiesConfig _config;

    [BindProperty]
    public Celebrity Celebrity { get; set; } = new();

    [BindProperty]
    public string? PhotoFileName { get; set; }

    [BindProperty]
    public string? TempPhotoPath { get; set; }

    public bool IsConfirmed { get; set; } = false;

    public NewCelebrityModel(IRepository repo, IOptions<CelebritiesConfig> config) {
        _repo = repo;
        _config = config.Value;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(IFormFile? photoFile, string action) {
        if (action == "preview") {
            if (photoFile != null) {
                PhotoFileName = photoFile.FileName;
                var tempPath = Path.Combine(Path.GetTempPath(), PhotoFileName);
                using (var stream = new FileStream(tempPath, FileMode.Create)) {
                    await photoFile.CopyToAsync(stream);
                }
                TempPhotoPath = tempPath;
                IsConfirmed = true;
                return Page();
            }
        } else if (action == "save") {
            if (!string.IsNullOrEmpty(TempPhotoPath)) {
                var finalPath = Path.Combine(_config.PhotosFolder, PhotoFileName!);
                System.IO.File.Move(TempPhotoPath, finalPath, true);
                Celebrity.ReqPhotoPath = PhotoFileName;
                _repo.AddCelebrity(Celebrity);
                return RedirectToPage("Celebrities");
            }
        } else if (action == "cancel") {
            if (!string.IsNullOrEmpty(TempPhotoPath) && System.IO.File.Exists(TempPhotoPath)) {
                System.IO.File.Delete(TempPhotoPath);
            }
            return RedirectToPage("Celebrities");
        }

        return Page();
    }
}
