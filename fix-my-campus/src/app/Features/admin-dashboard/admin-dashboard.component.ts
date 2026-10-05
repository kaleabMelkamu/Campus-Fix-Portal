import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TicketService, Ticket } from '../../core/ticket.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css'],
})
export class AdminDashboardComponent {
  // ✅ Define technicians so the template can use it
  technicians: string[] = ['Tech A', 'Tech B', 'Tech C'];

  constructor(private ticketService: TicketService) {}

  get tickets(): Ticket[] {
    return this.ticketService.getTickets();
  }

  getStatusCount(status: string): number {
    return this.tickets.filter(
      (ticket) => ticket.status.toLowerCase() === status.toLowerCase(),
    ).length;
  }

  assignTechnician(ticket: Ticket, technician: string) {
    this.ticketService.updateTicket(ticket, { technician, status: 'Assigned' });
  }

  updateStatus(ticket: Ticket, newStatus: string) {
    this.ticketService.updateTicket(ticket, { status: newStatus });
  }
}
