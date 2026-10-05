using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FixMyCampus.API.Extensions;
using FixMyCampus.Application.DTOs;
using FixMyCampus.Application.Services;

namespace FixMyCampus.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    /// <summary>
    /// Reporter / Student submits a new issue ticket.
    /// Returns 201 Created with Location header.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TicketDto>> Create([FromBody] CreateTicketDto dto, CancellationToken ct)
    {
        var reporterId = User.GetUserId();
        var result = await _ticketService.CreateTicketAsync(dto, reporterId, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Campus Feed: View all issues with filtering on Building and Status, plus Search and Pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<TicketDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<TicketDto>>> GetCampusFeed([FromQuery] TicketFilterDto filter, CancellationToken ct)
    {
        var result = await _ticketService.GetCampusFeedAsync(filter, ct);
        return Ok(result);
    }

    /// <summary>
    /// Reporter's "My Tickets" view.
    /// </summary>
    [HttpGet("my")]
    [ProducesResponseType(typeof(PagedResponse<TicketDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<TicketDto>>> GetMyTickets([FromQuery] TicketFilterDto filter, CancellationToken ct)
    {
        var reporterId = User.GetUserId();
        var result = await _ticketService.GetMyTicketsAsync(reporterId, filter, ct);
        return Ok(result);
    }

    /// <summary>
    /// Ticket details with full timestamped status transition history.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TicketDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDetailDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _ticketService.GetTicketByIdAsync(id, ct);
        return Ok(result);
    }

    /// <summary>
    /// Admin Hard Rule: Assigns a technician by name to take ticket from New -> Assigned.
    /// Rejects if ticket is not New (400).
    /// </summary>
    [HttpPost("{id:guid}/assign")]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDto>> Assign(Guid id, [FromBody] AssignTicketDto dto, CancellationToken ct)
    {
        var adminId = User.GetUserId();
        var result = await _ticketService.AssignTechnicianAsync(id, dto, adminId, ct);
        return Ok(result);
    }

    /// <summary>
    /// Admin Hard Rule: Moves status along strict path: New -> Assigned -> InProgress -> Resolved.
    /// API rejects illegal skips or backward moves with 400 Bad Request.
    /// </summary>
    [HttpPost("{id:guid}/status")]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDto>> MoveStatus(Guid id, [FromBody] UpdateTicketStatusDto dto, CancellationToken ct)
    {
        var adminId = User.GetUserId();
        var result = await _ticketService.MoveStatusAsync(id, dto, adminId, ct);
        return Ok(result);
    }
}
