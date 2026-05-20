using ASPA008_1.Models;
using DAL_Celebrity;
using Microsoft.AspNetCore.Mvc;

namespace ASPA008_1.Controllers;

public sealed class CelebritiesController : Controller
{
    private readonly IRepository repository;

    public CelebritiesController(IRepository repository)
    {
        this.repository = repository;
    }

    public IActionResult Index() => View(repository.GetAllCelebrities());

    public IActionResult Details(int id)
    {
        var celebrity = repository.GetCelebrityById(id);
        if (celebrity is null)
        {
            return NotFound();
        }

        return View(new CelebrityDetailsViewModel
        {
            Celebrity = celebrity,
            Lifeevents = repository.GetLifeeventsByCelebrityId(id)
        });
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.FormAction = nameof(ConfirmCreate);
        ViewBag.SubmitText = "Preview";
        return View(new CelebrityFormModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmCreate(CelebrityFormModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.FormAction = nameof(ConfirmCreate);
            ViewBag.SubmitText = "Preview";
            return View("Create", model);
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateConfirmed(CelebrityFormModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("ConfirmCreate", model);
        }

        repository.AddCelebrity(ToCelebrity(model));
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var celebrity = repository.GetCelebrityById(id);
        if (celebrity is null)
        {
            return NotFound();
        }

        ViewBag.FormAction = nameof(Edit);
        ViewBag.SubmitText = "Save";
        return View(ToFormModel(celebrity));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CelebrityFormModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.FormAction = nameof(Edit);
            ViewBag.SubmitText = "Save";
            return View(model);
        }

        repository.UpdCelebrity(model.Id, ToCelebrity(model));
        return RedirectToAction(nameof(Details), new { id = model.Id });
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var celebrity = repository.GetCelebrityById(id);
        return celebrity is null ? NotFound() : View(celebrity);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        repository.DelCelebrity(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error() => View("~/Views/Shared/Error.cshtml");

    private static CelebrityFormModel ToFormModel(Celebrity celebrity) => new()
    {
        Id = celebrity.Id,
        FullName = celebrity.FullName,
        Nationality = celebrity.Nationality,
        ReqPhotoPath = celebrity.ReqPhotoPath
    };

    private static Celebrity ToCelebrity(CelebrityFormModel model) => new()
    {
        Id = model.Id,
        FullName = model.FullName.Trim(),
        Nationality = model.Nationality,
        ReqPhotoPath = model.ReqPhotoPath?.Trim()
    };
}
