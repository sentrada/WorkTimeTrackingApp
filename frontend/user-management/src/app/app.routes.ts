import { Routes } from '@angular/router';
import { UserListComponent } from './user-list/user-list.component';

export const routes: Routes = [
  { path: '', component: UserListComponent }  // Az alapértelmezett útvonal a felhasználói lista
];
