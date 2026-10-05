using FixMyCampus.Application.DTOs;

namespace FixMyCampus.Application.Services;

public interface ITicketService
{
    Task<TicketDto> CreateTicketAsync(CreateTicketDto dto, Guid reporterId, CancellationToken ct = default);
    Task<PagedResponse<TicketDto>> GetCampusFeedAsync(TicketFilterDto filter, CancellationToken ct = default);
    Task<PagedResponse<TicketDto>> GetMyTicketsAsync(Guid reporterId, TicketFilterDto filter, CancellationToken ct = default);
    Task<TicketDetailDto> GetTicketByIdAsync(Guid id, CancellationToken ct = default);
    Task<TicketDto> AssignTechnicianAsync(Guid ticketId, AssignTicketDto dto, Guid adminId, CancellationToken ct = default);
    Task<TicketDto> MoveStatusAsync(Guid ticketId, UpdateTicketStatusDto dto, Guid adminId, CancellationToken ct = default);
    Task<AdminStatsDto> GetAdminStatsAsync(CancellationToken ct = default);
}
