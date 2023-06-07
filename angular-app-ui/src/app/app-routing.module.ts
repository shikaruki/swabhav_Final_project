import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LibraryComponent } from './library/library.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AuthenticationGuard } from './guards/authentication.guard';
import { UsersListComponent } from './users-list/users-list.component';
import { AuthorizationGuard } from './authorization.guard';
import { ProfileComponent } from './profile/profile.component';
import { OrderComponent } from './order/order.component';
import { OrdersComponent } from './orders/orders.component';


const routes: Routes = [

  {
    path:'books/library',
    component:LibraryComponent,
    canActivate:[AuthenticationGuard ]
  },
  {
    path:'login',
    component:LoginComponent,
  },
  {
    path:'register',
    component:RegisterComponent
  },
  {
   path:'users/list',
   component:UsersListComponent,
   canActivate:[AuthorizationGuard]
  },
  {
    path:'users/profile',
    component:ProfileComponent,
    canActivate: [AuthenticationGuard],
  },
  {
    path: 'users/order',
    component: OrderComponent,
    canActivate: [AuthenticationGuard],
  },
  {
    path: 'users/all-orders',
    component: OrdersComponent,
    canActivate: [AuthorizationGuard],
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
