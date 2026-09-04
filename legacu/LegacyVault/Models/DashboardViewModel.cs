namespace LegacyVault.Models;

public class DashboardViewModel
{
    public string UserName { get; set; } = "there";
    public int AssetCount { get; set; }
    public int DocumentCount { get; set; }
    public int ExecutorCount { get; set; }
    public int InstructionCount { get; set; }
    public int HealthPercent { get; set; }
    public IReadOnlyList<ActivityLog> RecentActivity { get; set; } = [];
}