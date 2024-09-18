import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  // URL-ek a Query és Command Service-hez
  private queryUrl = 'http://localhost:5300/api';
  private commandUrl = 'http://localhost:5299/api';

  constructor(private http: HttpClient) {}

  // Felhasználók lekérése a Query Service-től
  getUsers(): Observable<any> {
    return this.http.get<any>(`${this.queryUrl}/v1/UserLookup`);
  }

  // Felhasználó frissítése a Command Service-en keresztül
  updateUser(id: string, userData: any): Observable<any> {
    return this.http.put(`${this.commandUrl}/UpdateUser/${id}`, userData);
  }

  // Felhasználó törlése a Command Service-en keresztül
  deleteUser(id: string): Observable<any> {
  const deleteCommand = {
    id: id
  };
  return this.http.delete(`${this.commandUrl}/DeleteUser/${id}`, {
    body: deleteCommand
  });
}

  // Új felhasználó létrehozása a Command Service-en keresztül
  createUser(userData: any): Observable<any> {
    return this.http.post(`${this.commandUrl}/CreateUser`, userData);
  }
}
