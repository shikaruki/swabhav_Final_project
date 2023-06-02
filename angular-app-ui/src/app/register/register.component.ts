import { Component } from '@angular/core';
import { AbstractControl, AbstractControlOptions, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { User, UserType } from '../models/models';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  hide = true;
  //for displaying the msg
  responseMsg: string = '';
  registerForm: FormGroup;
  //using reactive form ,form builder is syntactic sugar 
  constructor(private fb: FormBuilder) {
    //initailzating the form and validating it 
    this.registerForm = fb.group(
      {
        firstName: fb.control('', [Validators.required]),
        lastName: fb.control('', [Validators.required]),
        email: fb.control('', [Validators.required, Validators.email]),
        password: fb.control('', [
          Validators.minLength(8),
          Validators.maxLength(15),
        ]),
        rpassword: fb.control(''),
        userType: fb.control('student'),
      }, {
        validators: [repeatPasswordValidator],          //applying validator on whole form ,
        //and typecasting the validator object into abstract control option
      } as AbstractControlOptions
    );
  }
  //creating the register function so that when we click on the create account button this function will get caall
  //creating a get and set function for accesing the individual element of form
  register(){
  let user:User ={
  id:0,
  firstName:this.registerForm.get('firstName')?.value,
  lastName:this.registerForm.get('lastName')?.value,
  email:this.registerForm.get('email')?.value,
  userType:UserType.USER,
  mobile: 0,
  password:this.registerForm.get('password')?.value,
  blocked:false,
  active:false,
  createdOn:'',
  fine:0,
  }
   console.log(user);
  }
  
  getFirstNameErrors() {
    if (this.FirstName.hasError('required')) return 'Field is requied!';
    return '';
  }
  getLastNameErrors() {
    if (this.LastName.hasError('required')) return 'Field is requied!';
    return '';
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

  get FirstName(): FormControl {
    return this.registerForm.get('firstName') as FormControl;
  }
  get LastName(): FormControl {
    return this.registerForm.get('lastName') as FormControl;
  }
  get Email(): FormControl {
    return this.registerForm.get('email') as FormControl;
  }
  get Password(): FormControl {
    return this.registerForm.get('password') as FormControl;
  }
  get RPassword(): FormControl {
    return this.registerForm.get('rpassword') as FormControl;
  }
  
}
//creating the custom validator for confirm password,
//validatorFn is function that takes input on validation 
//are applying if error return the validation error object otherwise return a null

export const repeatPasswordValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const pwd = control.get('password')?.value;
  const rpwd = control.get('rpassword')?.value;
  if (pwd === rpwd) {
    control.get('rpassword')?.setErrors(null);
    return null;
  } else {
    control.get('rpassword')?.setErrors({ rpassword: true });
    return { rpassword: true };
  }
};
