using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VRR.Application.DTOs;
using VRR.Application.Interfaces;
using VRR.Domain.Enums;
using VRR.Infrastructure.Data;

namespace VRR.Web.Controllers;

[Authorize(Roles = "Admin,EmergencyCoordinator,CaseWorker")]
public class CheckInsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IAuditService _audit;


    public CheckInsController(AppDbContext context, IAuditService audit)
    {
        _context = context;
        _audit = audit;
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var checkIn = await _context.CheckIns
            .Include(c => c.Resident)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (checkIn == null) return NotFound();

        ViewData["ResidentName"] = $"{checkIn.Resident.FirstName} {checkIn.Resident.LastName}";
        ViewData["EventId"] = checkIn.EmergencyEventId;

        return View(new UpdateCheckInDto
        {
            Id = checkIn.Id,
            Status = checkIn.Status,
            Notes = checkIn.Notes
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UpdateCheckInDto dto)
    {
        var checkIn = await _context.CheckIns
            .Include(c => c.Resident)
            .FirstOrDefaultAsync(c => c.Id == dto.Id);

        if (checkIn == null) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewData["ResidentName"] = $"{checkIn.Resident.FirstName} {checkIn.Resident.LastName}";
            ViewData["EventId"] = checkIn.EmergencyEventId;
            return View(dto);
        }
        var oldStatus = checkIn.Status;
        checkIn.Status = dto.Status;
        checkIn.Notes = dto.Notes;
        checkIn.AssignedStaffUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (dto.Status != CheckInStatus.Pending && checkIn.AttemptedAt == null)
            checkIn.AttemptedAt = DateTime.UtcNow;

        checkIn.CompletedAt = dto.Status == CheckInStatus.Completed ? DateTime.UtcNow : null;

        await _context.SaveChangesAsync();
        await this.AuditAsync(_audit, "UpdateCheckIn", "CheckIn",
            checkIn.Id.ToString(), $"Status {oldStatus} -> {dto.Status}");

        return RedirectToAction("Details", "EmergencyEvents", new { id = checkIn.EmergencyEventId });
    }
}