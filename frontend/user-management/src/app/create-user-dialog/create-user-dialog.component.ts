import { Component } from '@angular/core';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { UserService } from '../user.service'; // Szolgáltatás importálása
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-user-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, FormsModule, MatFormFieldModule, MatInputModule],
  templateUrl: './create-user-dialog.component.html',
  styleUrls: ['./create-user-dialog.component.scss']
})
export class CreateUserDialogComponent {
  userName: string = '';
  email: string = '';
  password: string = '';

  constructor(
    public dialogRef: MatDialogRef<CreateUserDialogComponent>,
    private userService: UserService  // Szolgáltatás injektálása
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

  create(): void {
    const newUser = {
      name: this.userName,
      email: this.email,
      password: this.password
    };

    this.userService.createUser(newUser).subscribe({
      next: () => {
        this.dialogRef.close(newUser);  // Sikeres létrehozás után bezárjuk a modált
      },
      error: (err) => {
        console.error('Hiba az új felhasználó létrehozásakor:', err);
      }
    });
  }
}
