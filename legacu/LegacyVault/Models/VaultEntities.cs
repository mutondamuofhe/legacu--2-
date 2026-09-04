using System.ComponentModel.DataAnnotations;

namespace LegacyVault.Models;

public abstract class OwnedEntity
{
    public int Id { get; set; }
    [Required] public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool ReleaseAfterVerification { get; set; }
}

public class DigitalAsset : OwnedEntity
{
    [Required, StringLength(120)] public string Name { get; set; } = string.Empty;
    [StringLength(80)] public string Category { get; set; } = string.Empty;
    [StringLength(160)] public string Provider { get; set; } = string.Empty;
    [StringLength(500)] public string AccountReference { get; set; } = string.Empty;
    [StringLength(2000)] public string PreferredAction { get; set; } = string.Empty;
}

public class VaultDocument : OwnedEntity
{
    [Required, StringLength(180)] public string DisplayName { get; set; } = string.Empty;
    [Required, StringLength(120)] public string StoredFileName { get; set; } = string.Empty;
    [Required, StringLength(100)] public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
}

public class DigitalExecutor : OwnedEntity
{
    [Required, StringLength(120)] public string Name { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(180)] public string Email { get; set; } = string.Empty;
    [Phone, StringLength(40)] public string Phone { get; set; } = string.Empty;
    [StringLength(80)] public string Relationship { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
}

public class LegacyInstruction : OwnedEntity
{
    [Required, StringLength(160)] public string Title { get; set; } = string.Empty;
    [Required, StringLength(4000)] public string Details { get; set; } = string.Empty;
    [StringLength(120)] public string PreferredAction { get; set; } = string.Empty;
    public int? DigitalAssetId { get; set; }
    public DigitalAsset? DigitalAsset { get; set; }
}

public class ActivityLog
{
    public int Id { get; set; }
    [Required] public string UserId { get; set; } = string.Empty;
    [Required, StringLength(180)] public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class VerificationRequest
{
    public int Id { get; set; }
    [Required] public string UserId { get; set; } = string.Empty;
    [Required, StringLength(40)] public string Status { get; set; } = "Pending";
    [StringLength(500)] public string? Notes { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReviewedAt { get; set; }
}