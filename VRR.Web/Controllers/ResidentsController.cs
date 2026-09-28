using Microsoft.AspNetCore.Mvc;
using VRR.Application.DTOs;
using VRR.Application.Interfaces;
using VRR.Domain.Entities;
using VRR.Domain.Enums;
using VRR.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace VRR.Web.Controllers;

public class ResidentsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IRiskScoringService _riskScoringService;
    private readonly IAuditService _audit;


    public ResidentsController(AppDbContext context, IRiskScoringService riskScoringService, IAuditService audit)
    {
        _context = context;
        _riskScoringService = riskScoringService;
        _audit = audit;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new ResidentRegistrationDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(ResidentRegistrationDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var resident = new Resident
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            AddressLine = dto.AddressLine,
            City = dto.City,
            ZipCode = dto.ZipCode,
            MobilityStatus = dto.MobilityStatus,
            MedicalNeeds = dto.MedicalNeeds,
            LivesAlone = dto.LivesAlone,
            HasAirConditioning = dto.HasAirConditioning,
            HasBackupPower = dto.HasBackupPower,
            EmergencyContactName = dto.EmergencyContactName,
            EmergencyContactPhone = dto.EmergencyContactPhone,
            EmergencyContactRelationship = dto.EmergencyContactRelationship,
            ConsentGiven = dto.ConsentGiven,
            ConsentDate = DateTime.UtcNow,
            Status = ResidentStatus.PendingVerification
        };

        resident.RiskScore = _riskScoringService.CalculateRiskScore(resident);

        _context.Residents.Add(resident);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Confirmation));
    }

    [HttpGet]
    public IActionResult Confirmation()
    {
        return View();
    }

    [Authorize(Roles = "Admin,EmergencyCoordinator,CaseWorker")]
    public async Task<IActionResult> Dashboard()
    {
        var residents = await _context.Residents
            .OrderByDescending(r => r.RiskScore)
            .ToListAsync();

        await this.AuditAsync(_audit, "ViewResidentList", "Resident",
            details: $"{residents.Count} records returned");

        return View(residents);
    }
}