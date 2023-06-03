// import { Injectable } from '@angular/core';
// import {
//   ActivatedRouteSnapshot,
//   CanActivate,
//   RouterStateSnapshot,
//   UrlTree
//   } from '@angular/router';
// import { Observable } from 'rxjs';
// import { ApiService } from '../services/api.service';

// @Injectable({
//   providedIn: 'root'
// })
// export class AuthenticationGuard implements CanActivate {
//   constructor(private api:ApiService){}
//   canActivate(
//     route: ActivatedRouteSnapshot,
//     state: RouterStateSnapshot):
//     |Observable<boolean | UrlTree>
//     | Promise<boolean | UrlTree>
//     | boolean | UrlTree {
//     return true.api.isLoggedIn();
//   }

// }


import { Injectable } from '@angular/core';
import * as router from '@angular/router';
import { Observable } from 'rxjs';
import { ApiService } from '../services/api.service';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationGuard implements router.CanActivate {
  constructor(private api: ApiService) {}
  canActivate(
    route: router.ActivatedRouteSnapshot,
    state: router.RouterStateSnapshot
  ):
    | Observable<boolean | router.UrlTree>
    | Promise<boolean | router.UrlTree>
    | boolean
    | router.UrlTree {
    return this.api.isLoggedIn();
  }
}
