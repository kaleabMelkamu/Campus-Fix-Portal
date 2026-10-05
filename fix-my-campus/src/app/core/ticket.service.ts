import { Injectable } from '@angular/core';

export interface Ticket {
  title: string;
  location: string;
  description: string;
  status: string;
  reporter: string;
  technician?: string;
}

@Injectable({
  providedIn: 'root',
})
export class TicketService {
  private tickets: Ticket[] = [
    {
      title: 'Electrical',
      location: 'Library',
      description: 'Light not working in study area',
      status: 'Pending',
      reporter: 'rahmet',
    },
    {
      title: 'Plumbing',
      location: 'Dormitory',
      description: 'Leaking pipe in bathroom',
      status: 'Assigned',
      reporter: 'abebe',
      technician: 'Tech B',
    },
    {
      title: 'Internet',
      location: 'Computer Lab',
      description: 'Wi-Fi keeps disconnecting',
      status: 'In Progress',
      reporter: 'kassa',
      technician: 'Tech A',
    },
    {
      title: 'Furniture',
      location: 'Dormitory',
      description: 'Broken chair in common room',
      status: 'Resolved',
      reporter: 'biruk',
      technician: 'Tech C',
    },
  ];

  getTickets(): Ticket[] {
    return this.tickets;
  }

  addTicket(ticket: Ticket) {
    this.tickets.push(ticket);
  }

  updateTicket(ticket: Ticket, updates: Partial<Ticket>) {
    Object.assign(ticket, updates);
  }
}
