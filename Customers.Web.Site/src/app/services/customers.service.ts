import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer } from '../models/customer';

@Injectable({
  providedIn: 'root'
})
export class CustomersService {

  protected headers: HttpHeaders;
  private endPoint: string;

  constructor(private http: HttpClient) {
    
  }

  getAllCustomers(): Observable<Customer[]> {
    // return this.http.get<Customer[]>(`${this.endPoint}/get`);

    return new Observable<Customer[]>();
  }
  
}
