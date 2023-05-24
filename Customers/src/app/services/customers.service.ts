import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Customer } from '../models/customer';
import { environment } from 'src/environments/environment';
import { ApiResponse } from '../models/apiResponse';
import { CustomerRequest } from '../models/customerRequest';

@Injectable({
  providedIn: 'root'
})
export class CustomersService {

  protected headers: HttpHeaders;
  private endPoint: string;

  constructor(private http: HttpClient) {
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });

    this.endPoint = `${environment.customers.endPoint}/customers`;
  }

  getAllCustomers(): Observable<Customer[]> {
    return this.http.get<ApiResponse<Customer[]>>(
      encodeURI(`${this.endPoint}/list`))
      .pipe(
        map(
          (resp: ApiResponse<Customer[]>) => resp.result
        )
      );
  }

  getCustomerById(id: number): Observable<Customer> {
    return this.http.get<ApiResponse<Customer>>(
      encodeURI(`${this.endPoint}/${id}`))
      .pipe(
        map(
          (resp: ApiResponse<Customer>) => resp.result
        )
      );
  }

  createCustomer(request: CustomerRequest): Observable<any> {
    return this.http.post<ApiResponse<any>>(
      `${this.endPoint}`,
      request,
      { headers: this.headers })
      .pipe(
        map(
          (resp: ApiResponse<any>) => resp.result
        )
      );
  }

  deleteCustomer(id: number, rowVersion: number): Observable<any> {
    return this.http.delete<ApiResponse<any>>(
      `${this.endPoint}/${id}/rowversion/${rowVersion}`)
      .pipe(
        map(
          (resp: ApiResponse<any>) => resp.result
        )
      );
  }

  updateCustomer(request: CustomerRequest): Observable<any> {
    return this.http.patch<ApiResponse<any>>(
      `${this.endPoint}`,
      request,
      { headers: this.headers })
      .pipe(
        map(
          (resp: ApiResponse<any>) => resp.result
        )
      );
  }

}
