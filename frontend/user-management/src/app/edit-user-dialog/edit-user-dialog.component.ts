import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { UserService } from '../user.service'; 
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-user-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, FormsModule, MatFormFieldModule, MatInputModule],
  templateUrl: './edit-user-dialog.component.html',
  styleUrls: ['./edit-user-dialog.component.scss']
})
export class EditUserDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<EditUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private userService: UserService  // Szolgáltatás injektálása
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

  save(): void {
    this.userService.updateUser(this.data.userId, {
      name: this.data.userName,
      email: this.data.email
    }).subscribe({
      next: () => {
        this.dialogRef.close(this.data);  // A sikeres frissítés után bezárjuk a modált
      },
      error: (err) => {
        console.error('Hiba a felhasználó frissítésekor:', err);
      }
    });
  }
}
