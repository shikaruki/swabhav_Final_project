import { Component } from '@angular/core';
import { ApiService } from '../services/api.service';
import { Order } from '../models/models';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent {
  listOfOrders: Order[] = [];
  
  columns: string[] = ['id', 'name', 'bookid', 'book', 'date', 'returned'];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    let userid = this.api.getTokenUserInfo()?.id ?? 0;
    this.api.getOrdersOfUser(userid).subscribe({
      next: (res: Order[]) => {
        console.log(res);
        this.listOfOrders = res;
      },
      error: (err: any) => console.log(err),
    });
  }
}
