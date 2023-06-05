import { Component, OnInit } from '@angular/core';
import { User } from '../models/models';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
  //creating two variable one for storing the data from backend and another for column
  users: User[] = [];
  columnsToDisplay: string[] = [
    'id',
    'name',
    'email',
    'mobile',
    'fine',
    'blocked',
    'active',
    'created on',
    'action',
  ];

  //injecting the api service in the constructor and and make getalluser function in api service 
  constructor(private api: ApiService) { }
  ngOnInit(): void {
    this.api.getAllUsers().subscribe({
      next: (res:any) => {
        this.users = [];
        this.users = res;
      },
      error: (err: any) => console.log(),
    });
  }
  blockUser(user: User) {
    if (user.blocked) {
      this.api.unblockUser(user.id).subscribe({
        next: (res: any) => {
          if (res === 'success') user.blocked = false;
        },
        error: (err: any) => console.log(err),
      });
    } else {
      this.api.blockUser(user.id).subscribe({
        next: (res: any) => {
          if (res === 'success') user.blocked = true;
        },
        error: (err: any) => console.log(err),
      });
    }
  }

  enableUser(user: User) {
    if (user.active) {
      this.api.disableUser(user.id).subscribe({
        next: (res: any) => {
          if (res === 'success') user.active = false;
        },
        error: (err: any) => console.log(err),
      });
    } else {
      this.api.enableUser(user.id).subscribe({
        next: (res: any) => {
          if (res === 'success') user.active = true;
        },
        error: (err: any) => console.log(err),
      });
    }
  }

}
