using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FixMyCampus.Application.Common.Interfaces;
using FixMyCampus.Application.DTOs;
using FixMyCampus.Application.Services;
using FixMyCampus.Domain.Enums;

namespace FixMyCampus.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly IAppDbContext _db;

    public AdminController(ITicketService ticketService, IAppDbContext db)
    {
        _ticketService = ticketService;
        _db = db;
    }

    /// <summary>
    /// Admin Dashboard Summary: Ticket counts by status and open tickets by building.
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(AdminStatsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<AdminStatsDto>> GetStats(CancellationToken ct)
    {
        var stats = await _ticketService.GetAdminStatsAsync(ct);
        return Ok(stats);
    }

    /// <summary>
    /// Available technicians for assigning tickets.
    /// </summary>
    [HttpGet("technicians")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTechnicians(CancellationToken ct)
    {
        var technicians = await _db.Users
            .AsNoTracking()
            .Where(u => u.Role == UserRole.Technician || u.Role == UserRole.Admin)
            .Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email
            })
            .ToListAsync(ct);

        return Ok(technicians);
    }
}
