import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/models';
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  baseUrl = 'https://localhost:7038/api/Library/';
  constructor(private http: HttpClient) { }

  createAccount(user: User) {
    return this.http.post(this.baseUrl + 'CreateAccount', user, {
      responseType: 'text',
    });
  }
}
