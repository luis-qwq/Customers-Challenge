import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { CustomersService } from 'src/app/services/customers.service';
import { CustomerFormComponent } from '../customer-form/customer-form.component';

@Component({
  selector: 'app-customer-add',
  templateUrl: './customer-add.component.html',
  styleUrls: ['./customer-add.component.css']
})
export class CustomerAddComponent {

  @ViewChild(CustomerFormComponent)
  customerForm!: CustomerFormComponent;

  constructor(
    private router: Router,
    private customersService : CustomersService) {}

  onAddClick() {
    if (this.customerForm.isValid()) {
      this.customersService.createCustomer(this.customerForm.form.value)
        .subscribe(
          result => {
            this.router.navigate(['/']);
          },
          error => {
            console.log('http error => ', error);
          });
    }
  }

  onCancelClick() {
    this.router.navigate(['/']);
  }

}
