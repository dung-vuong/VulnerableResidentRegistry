using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VRR.Application.DTOs;
using VRR.Application.Interfaces;
using VRR.Domain.Entities;
using VRR.Domain.Enums;
using VRR.Infrastructure.Data;

namespace VRR.Infrastructure.Services;

public class EmergencyDeclarationService : IEmergencyDeclarationService
{
    private readonly AppDbContext _context;

    public EmergencyDeclarationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> DeclareAsync(DeclareEmergencyDto dto, string declaredByUserId)
    {
        var zones = dto.AffectedZone
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(z => z.Length >= 5 ? z[..5] : z)
            .Distinct()
            .ToList();

        var emergency = new EmergencyEvent
        {
            Type = dto.Type,
            Description = dto.Description,
            AffectedZone = string.Join(",", zones),
            Severity = dto.Severity,
            DeclaredByUserId = declaredByUserId
        };

        var residents = await _context.Residents
            .Where(r => r.ConsentGiven
                        && r.Status != ResidentStatus.Inactive
                        && zones.Contains(r.ZipCode.Substring(0, 5)))
            .ToListAsync();

        foreach (var resident in residents)
        {
            emergency.CheckIns.Add(new CheckIn
            {
                ResidentId = resident.Id,
                Status = CheckInStatus.Pending
            });
        }

        _context.EmergencyEvents.Add(emergency);
        await _context.SaveChangesAsync();

        return emergency.Id;
    }
}