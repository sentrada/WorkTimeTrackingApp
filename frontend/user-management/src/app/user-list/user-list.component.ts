import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { EditUserDialogComponent } from '../edit-user-dialog/edit-user-dialog.component';
import { CreateUserDialogComponent } from '../create-user-dialog/create-user-dialog.component';
import { ConfirmDeleteDialogComponent } from '../confirm-delete-dialog/confirm-delete-dialog.component';
import { interval } from 'rxjs';
import { take, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  displayedColumns: string[] = ['userId', 'userName', 'email', 'actions'];
  users: any[] = [];
  pollingAttempts = 0;

  constructor(private userService: UserService, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getUsers().subscribe({
      next: (data) => {
        this.users = data.users;
      },
      error: (error) => {
        console.error('Hiba a felhasználók lekérése során:', error);
      }
    });
  }

  openEditDialog(user: any): void {
    const dialogRef = this.dialog.open(EditUserDialogComponent, {
      width: '300px',
      data: user
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadUsers();
      }
    });
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateUserDialogComponent, {
      width: '300px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadUsers();
      }
    });
  }

  confirmDelete(user: any): void {
    const dialogRef = this.dialog.open(ConfirmDeleteDialogComponent, {
      width: '250px',
      data: user
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.userService.deleteUser(user.userId).subscribe({
          next: () => {
             this.pollForChanges();
          },
          error: (err) => {
            console.error('Hiba a felhasználó törlése során:', err);
          }
        });
      }
    });
  }

  pollForChanges(): void {
    const maxPollingAttempts = 20;
    this.pollingAttempts = 0;
    interval(100)
      .pipe(
        take(maxPollingAttempts),
        switchMap(() => this.userService.getUsers())
      )
      .subscribe({
        next: (data) => {
          this.users = data.users;
          this.pollingAttempts++;
          if (!this.users.find(u => u.userId === null)) {
            this.pollingAttempts = maxPollingAttempts;
          }
        },
        error: (error) => {
          console.error('Hiba a felhasználók frissítése során:', error);
        }
      });
  }
}
