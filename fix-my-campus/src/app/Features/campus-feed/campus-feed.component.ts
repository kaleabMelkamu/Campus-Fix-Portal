import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TicketService, Ticket } from '../../core/ticket.service';
@Component({
  selector: 'app-campus-feed',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './campus-feed.component.html',
  styleUrls: ['./campus-feed.component.css'],
})
export class CampusFeedComponent {
  constructor(private ticketService: TicketService) {}

  get tickets(): Ticket[] {
    return this.ticketService.getTickets();
  }

  getTicketsByStatus(status: string): Ticket[] {
    return this.tickets.filter(
      (ticket) => ticket.status.toLowerCase() === status.toLowerCase(),
    );
  }
}
