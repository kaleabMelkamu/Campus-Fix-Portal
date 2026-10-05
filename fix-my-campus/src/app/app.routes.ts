import { Routes } from '@angular/router';
import { LoginComponent } from './Features/login/login.component';
import { TicketFormComponent } from './Features/ticket-form/ticket-form.component';
import { CampusFeedComponent } from './Features/campus-feed/campus-feed.component';
import { MyTicketsComponent } from './Features/my-tickets/my-tickets.component';
import { AdminDashboardComponent } from './Features/admin-dashboard/admin-dashboard.component';
import { RegisterComponent } from './Features/register/register.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'submit-ticket', component: TicketFormComponent },
  { path: 'feed', component: CampusFeedComponent },
  { path: 'my-tickets', component: MyTicketsComponent },
  { path: 'admin', component: AdminDashboardComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'register', component: RegisterComponent },
];
