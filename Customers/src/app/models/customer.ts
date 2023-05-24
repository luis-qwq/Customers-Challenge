export interface Customer {
  customerId: number;
  firstName: string;
  lastName: string;
  email: string;
  createdDate: Date;
  modifiedDate: Date;
  rowVersion: number;
}