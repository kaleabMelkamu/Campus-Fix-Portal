import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true, // Standalone component
  imports: [FormsModule], 
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  email: string = '';
  password: string = '';

  constructor(private router: Router) {}
  
  login() {
    if (this.email && this.password) {
      this.router.navigate(['/submit-ticket']);  
    } else {
      alert('Please enter email and password');
    }
  }
}
