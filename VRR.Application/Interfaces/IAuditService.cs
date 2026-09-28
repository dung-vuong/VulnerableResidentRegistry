namespace VRR.Application.Interfaces;

public interface IAuditService
{
    Task LogAsync(string action, string entityType, string? entityId,
                  string? details, string? userId, string? userName, string? ipAddress);
}