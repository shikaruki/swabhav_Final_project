import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';

export interface TableElement {
  name: string;
  value: string;
}
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  dataSource: TableElement[] = [];
  columns: string[] = ['name', 'value'];

  //injecting api service inside the constructor.
  constructor(private api: ApiService) { }
  ngOnInit(): void {
    // throw new Error('Method not implemented.');
    let user = this.api.getTokenUserInfo();
    //initializing the data source
    this.dataSource = [
      {name:'Name',value:user?.firstName+""+user?.lastName},
      {name:'Email',value:user?.email??""},
      {name:'Blocked',value:this.blockedStatus()},
      {name:'Active',value:this.activeStatus()},
      
      
    ];
  }
  blockedStatus():string{
    let blocked=this.api.getTokenUserInfo()!.blocked;
    return blocked ? 'Yes,you are blocked':'No you are not blocked!';
  }

  activeStatus():string{
    let active=this.api.getTokenUserInfo()!.active;
    return active? 'Yes,your account is active':'No your account is not active';
  }

}
