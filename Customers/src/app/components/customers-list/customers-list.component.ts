import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Customer } from 'src/app/models/customer';
import { CustomersService } from 'src/app/services/customers.service';

@Component({
  selector: 'app-customers-list',
  templateUrl: './customers-list.component.html',
  styleUrls: ['./customers-list.component.css']
})
export class CustomersListComponent implements OnInit {

  customers: Customer[] = [];
  displayedColumns = ['customerId', 'firstName', 'lastName', 'email', 'createdDate', 'modifiedDate', 'actions'];

  constructor(
    private router: Router,
    private customersService: CustomersService) { }

    ngOnInit(): void {
      this.getCustomersList();    
    }
  
    getCustomersList() {
      this.customersService.getAllCustomers()
      .subscribe(
        response => {
          this.customers = response;
        },
        error => console.error(error)
      );
    }

    onCreateCustomerClick() {
      this.router.navigate(['/customers/add'])
    }

    onEditCustomerClick(data: Customer) {
      this.router.navigate(['/customers', data.customerId, 'edit']);
    }

    onDropCustomerClick(data: Customer) {
      this.customersService.deleteCustomer(data.customerId, data.rowVersion)
      .subscribe(
        response => this.getCustomersList(),
        error => console.error(error)        
      );
    }

}
