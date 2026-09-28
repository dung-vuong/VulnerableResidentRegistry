using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using VRR.Application.Interfaces;

namespace VRR.Web.Controllers;

public static class AuditExtensions
{
    public static Task AuditAsync(this Controller controller, IAuditService audit,
        string action, string entityType, string? entityId = null, string? details = null)
    {
        return audit.LogAsync(
            action, entityType, entityId, details,
            controller.User.FindFirstValue(ClaimTypes.NameIdentifier),
            controller.User.Identity?.Name,
            controller.HttpContext.Connection.RemoteIpAddress?.ToString());
    }
}