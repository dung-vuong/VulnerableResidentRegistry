using System;
using System.Collections.Generic;
using System.Text;
using VRR.Application.Interfaces;
using VRR.Domain.Entities;
using VRR.Infrastructure.Data;

namespace VRR.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly AppDbContext _context;

    public AuditService(AppDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(string action, string entityType, string? entityId,
                               string? details, string? userId, string? userName, string? ipAddress)
    {
        _context.AuditLogs.Add(new AuditLog
        {
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = details,
            UserId = userId,
            UserName = userName,
            IpAddress = ipAddress
        });

        await _context.SaveChangesAsync();
    }
}