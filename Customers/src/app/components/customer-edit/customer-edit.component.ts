import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomersService } from 'src/app/services/customers.service';
import { CustomerFormComponent } from '../customer-form/customer-form.component';
import { Customer } from 'src/app/models/customer';

@Component({
  selector: 'app-customer-edit',
  templateUrl: './customer-edit.component.html',
  styleUrls: ['./customer-edit.component.css']
})
export class CustomerEditComponent implements OnInit {

  customerRouteId = 0;
  customer!: Customer;

  @ViewChild(CustomerFormComponent)
  customerForm!: CustomerFormComponent;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private customersService: CustomersService) { }

  ngOnInit(): void {
    this.customerRouteId = this.activatedRoute.snapshot.params['id'];
    this.getCustomerById(this.customerRouteId);
  }

  getCustomerById(id: number) {
    this.customersService.getCustomerById(id)
      .subscribe(
        resp => {
          this.customer = resp;
          this.patchFormValues(this.customer);
        },
        error => console.error(error)
      );
  }

  patchFormValues(data: Customer) {
    this.customerForm.form.setValue({
      customerId: data.customerId,
      firstName: data.firstName,
      lastName: data.lastName,
      email: data.email,
      rowVersion: data.rowVersion
    });
  }

  onSaveClick() {
    if (this.customerForm.isValid()) {
      this.customersService.updateCustomer(this.customerForm.form.value)
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
