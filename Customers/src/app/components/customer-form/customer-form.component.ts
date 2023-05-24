import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-customer-form',
  templateUrl: './customer-form.component.html',
  styleUrls: ['./customer-form.component.css']
})
export class CustomerFormComponent {

  form!: FormGroup;
  isSubmitted = false;

  constructor(private formBuilder: FormBuilder) {
    this.setForm();
  }

  isValid(): boolean {
    this.isSubmitted = true;
    return this.form.valid;
  }

  private setForm() {
    this.form = this.formBuilder.group({
      customerId: [0, [Validators.required]],
      firstName: [null, [
        Validators.required,
        Validators.maxLength(100)
      ]],
      lastName: [null, [
        Validators.required,
        Validators.maxLength(100)
      ]],
      email: [null, [
        Validators.required,
        Validators.email,
        Validators.maxLength(100)
      ]],
      rowVersion: [0, [Validators.required]]
    });
  }

}
