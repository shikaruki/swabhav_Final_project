import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LibraryComponent } from './library/library.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AuthenticationGuard } from './guards/authentication.guard';
import { ProfileComponent } from './profile/profile.component';
import { OrderComponent } from './order/order.component';
import { AuthorizationGuard } from './authorization.guard';
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
