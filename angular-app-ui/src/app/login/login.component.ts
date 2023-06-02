import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
hide=true;
loginForm:FormGroup;
responseMsg:string='';

constructor(private fb:FormBuilder){
  this.loginForm=fb.group({
    email:fb.control('',[Validators.required,Validators.email]),
    password:fb.control('',[
     Validators.required,
     Validators.minLength(8),
     Validators.maxLength(15),
    ]),
  });
  
}

login(){
  let loginInfo={
    email:this.loginForm.get('email')?.value,
    password:this.loginForm.get('password')?.value,

    };
}

getEmailErrors() {
  if (this.Email.hasError('required')) return 'Email is required!';
  if (this.Email.hasError('email')) return 'Email is invalid.';
  return '';
}
getPasswordErrors() {
  if (this.Password.hasError('required')) return 'Password is required!';
  if (this.Password.hasError('minlength'))
    return 'Minimum 8 characters are required!';
  if (this.Password.hasError('maxlength'))
    return 'Maximum 15 characters are required!';
  return '';
}


get Email(): FormControl {
  return this.loginForm.get('email') as FormControl;
}
get Password(): FormControl {
  return this.loginForm.get('password') as FormControl;
}
}





