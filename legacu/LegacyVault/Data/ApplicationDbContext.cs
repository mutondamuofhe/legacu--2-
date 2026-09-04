using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LegacyVault.Models;

namespace LegacyVault.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
	public DbSet<DigitalAsset> DigitalAssets => Set<DigitalAsset>();
	public DbSet<VaultDocument> VaultDocuments => Set<VaultDocument>();
	public DbSet<DigitalExecutor> DigitalExecutors => Set<DigitalExecutor>();
	public DbSet<LegacyInstruction> LegacyInstructions => Set<LegacyInstruction>();
	public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
	public DbSet<VerificationRequest> VerificationRequests => Set<VerificationRequest>();
}
