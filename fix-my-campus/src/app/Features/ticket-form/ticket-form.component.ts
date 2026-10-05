import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common'; //

@Component({
  selector: 'app-ticket-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './ticket-form.component.html',
  styleUrls: ['./ticket-form.component.css'],
})
export class TicketFormComponent {
  categories: string[] = [
    'Electrical',
    'Plumbing',
    'Internet',
    'Furniture',
    'Cleaning',
    'Other',
  ];

  selectedCategory: string = '';
  customTitle: string = '';
  location: string = '';
  description: string = '';
  attachedFile: File | null = null;

  onFileSelected(event: any) {
    this.attachedFile = event.target.files[0];
  }

  submitTicket() {
    let finalTitle =
      this.selectedCategory === 'Other'
        ? this.customTitle
        : this.selectedCategory;

    if (finalTitle && this.location && this.description) {
      let fileInfo = this.attachedFile
        ? `\nAttached File: ${this.attachedFile.name}`
        : '';
      alert(
        `Ticket submitted!\n\nTitle: ${finalTitle}\nLocation: ${this.location}\nDescription: ${this.description}${fileInfo}`,
      );

      // Reset form
      this.selectedCategory = '';
      this.customTitle = '';
      this.location = '';
      this.description = '';
      this.attachedFile = null;
    } else {
      alert('Please fill in all fields');
    }
  }
}
