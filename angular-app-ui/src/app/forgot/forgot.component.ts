import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-forgot',
  templateUrl: './forgot.component.html',
  styleUrls: ['./forgot.component.css']
})
export class ForgotComponent {
  forgotForm : FormGroup;
  responseMsg: string = '';

 constructor(private fb:FormBuilder,private api:ApiService){
 this.forgotForm =fb.group({
  email: fb.control('', [Validators.required, Validators.email]),
 });
 }
 sendResetLink() {
  const email = this.Email.value;
  console.log('Reset password link sent to: ' + email);
  this.api.ForgetPassword(email).subscribe({
    next: (res: any) => {
      console.log(res);
      this.responseMsg = res.toString();
    },
    error: (err: any) => {
      console.log('Error: ');
      console.log(err);
    },
  });
 }

  getEmailErrors() {
    if (this.Email.hasError('required')) return 'Email is required!';
    if (this.Email.hasError('email')) return 'Email is invalid.';
    return '';
  }
  get Email(): FormControl {
    return this.forgotForm.get('email') as FormControl;
  }
}
