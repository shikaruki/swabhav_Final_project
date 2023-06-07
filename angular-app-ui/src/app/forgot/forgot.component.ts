import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-forgot',
  templateUrl: './forgot.component.html',
  styleUrls: ['./forgot.component.css']
})
export class ForgotComponent {
  forgotForm : FormGroup;

 constructor(private fb:FormBuilder){
 this.forgotForm =fb.group({
  email: fb.control('', [Validators.required, Validators.email]),
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
