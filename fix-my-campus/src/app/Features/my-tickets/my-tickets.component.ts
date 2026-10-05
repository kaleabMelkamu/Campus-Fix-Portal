import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Ticket {
  title: string;
  location: string;
  description: string;
  status: string;
  image?: string; 
  technician?: string; 
}

@Component({
  selector: 'app-my-tickets',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-tickets.component.html',
  styleUrls: ['./my-tickets.component.css'],
})
export class MyTicketsComponent {
  myTickets: Ticket[] = [
    {
      title: 'Electrical',
      location: 'Library',
      description: 'Light not working in study area',
      status: 'Pending',
      image: 'assets/example-light.jpg',
    },
    {
      title: 'Plumbing',
      location: 'Dormitory',
      description: 'Leaking pipe in bathroom',
      status: 'In Progress',
    },
    {
      title: 'Internet',
      location: 'Computer Lab',
      description: 'Wi-Fi keeps disconnecting',
      status: 'Resolved',
    },
  ];
}
