using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VRR.Application.DTOs;
using VRR.Application.Interfaces;
using VRR.Infrastructure.Data;

namespace VRR.Web.Controllers;

[Authorize(Roles = "Admin,EmergencyCoordinator,CaseWorker")]
public class EmergencyEventsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IEmergencyDeclarationService _declarationService;
    private readonly IAuditService _audit;


    public EmergencyEventsController(AppDbContext context, IEmergencyDeclarationService declarationService, IAuditService audit)
    {
        _context = context;
        _declarationService = declarationService;
        _audit = audit;
    }

    public async Task<IActionResult> Index()
    {
        var events = await _context.EmergencyEvents
            .OrderByDescending(e => e.DeclaredAt)
            .ToListAsync();
        return View(events);
    }

    [Authorize(Roles = "Admin,EmergencyCoordinator")]
    [HttpGet]
    public IActionResult Declare() => View(new DeclareEmergencyDto());

    [Authorize(Roles = "Admin,EmergencyCoordinator")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Declare(DeclareEmergencyDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var id = await _declarationService.DeclareAsync(dto, userId);

        await this.AuditAsync(_audit, "DeclareEmergency", "EmergencyEvent",
            id.ToString(), $"Type={dto.Type}, Zones={dto.AffectedZone}, Severity={dto.Severity}");

        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Details(int id)
    {
        var emergency = await _context.EmergencyEvents
            .Include(e => e.CheckIns)
                .ThenInclude(c => c.Resident)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (emergency == null) return NotFound();

        await this.AuditAsync(_audit, "ViewEmergencyDetails", "EmergencyEvent",
            emergency.Id.ToString(), $"{emergency.CheckIns.Count} residents' data displayed");

        return View(emergency);
    }
}