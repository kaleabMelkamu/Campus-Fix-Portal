import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Ticket {
  title: string;
  location: string;
  description: string;
  status: string;
  reporter: string;
  technician?: string;
}

@Component({
  selector: 'app-technician-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './technician-dashboard.component.html',
  styleUrls: ['./technician-dashboard.component.css'],
})
export class TechnicianDashboardComponent {
  currentTechnician: string = 'Tech A';

  tickets: Ticket[] = [
    {
      title: 'Electrical',
      location: 'Library',
      description: 'Light not working in study area',
      status: 'Assigned',
      reporter: 'abebe',
      technician: 'Tech A',
    },
    {
      title: 'Plumbing',
      location: 'Dormitory',
      description: 'Leaking pipe in bathroom',
      status: 'Assigned',
      reporter: 'rahmet',
      technician: 'Tech B',
    },
    {
      title: 'Internet',
      location: 'Computer Lab',
      description: 'Wi-Fi keeps disconnecting',
      status: 'Resolved',
      reporter: 'biruk',
      technician: 'Tech A',
    },
  ];

  get myTickets(): Ticket[] {
    return this.tickets.filter(
      (ticket) => ticket.technician === this.currentTechnician,
    );
  }

  updateStatus(ticket: Ticket, newStatus: string) {
    ticket.status = newStatus;
  }
}
