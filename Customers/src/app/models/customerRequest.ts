export interface CustomerRequest {
  customerId: number;
  firstName: string;
  lastName: string;
  email: string;
  rowVersion: number;
}