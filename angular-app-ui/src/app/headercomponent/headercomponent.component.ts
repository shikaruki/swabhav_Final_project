import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-headercomponent',
  templateUrl: './headercomponent.component.html',
  styleUrls: ['./headercomponent.component.css']
})
export class HeadercomponentComponent {
//onclicking the menu button it will hide and show the side bar
@Output() menuClicked= new  EventEmitter<boolean>();
}
