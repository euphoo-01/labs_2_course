using System.ComponentModel.DataAnnotations;

namespace ASPA008_1.Models;

public sealed class CelebrityFormModel
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Nationality { get; set; } = string.Empty;

    [Display(Name = "Photo file")]
    public string? ReqPhotoPath { get; set; }
}
