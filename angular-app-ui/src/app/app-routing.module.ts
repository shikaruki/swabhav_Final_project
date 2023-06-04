import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LibraryComponent } from './library/library.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AuthenticationGuard } from './guards/authentication.guard';
import { ProfileComponent } from './profile/profile.component';


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
  }
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
