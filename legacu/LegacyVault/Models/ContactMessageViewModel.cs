using System.ComponentModel.DataAnnotations;

namespace LegacyVault.Models;

public class ContactMessageViewModel
{
    [Required]
    [Display(Name = "Full name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "Email address")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Subject")]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Message")]
    [StringLength(2000)]
    public string Message { get; set; } = string.Empty;
}
