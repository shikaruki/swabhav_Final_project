import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { SidenavComponent } from './sidenav/sidenav.component';
import { MaterialModule } from './material/material.module';
import { LibraryComponent } from './library/library.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import { ProfileComponent } from './profile/profile.component';
<<<<<<< HEAD
import { UsersListComponent } from './users-list/users-list.component';
=======
import { OrderComponent } from './order/order.component';
import { OrdersComponent } from './orders/orders.component';
>>>>>>> 48111468f37f78cd6ab85a560ac2c272c5171617

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    SidenavComponent,
    LibraryComponent,
    LoginComponent,
    RegisterComponent,
    ProfileComponent,
<<<<<<< HEAD
    UsersListComponent,
=======
    OrderComponent,
    OrdersComponent,
>>>>>>> 48111468f37f78cd6ab85a560ac2c272c5171617

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    ReactiveFormsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
      tokenGetter: () => {
        return localStorage.getItem('access_token');
      },
      allowedDomains: ['localhost:7038'],
      },
    }),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
