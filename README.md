# Customers-Challenge

Preview of the CRUD of customers.
The Customers.Api folder contains the backend project, which implements the CQRS pattern.
  Customers.Commons: contains the models used and other files such as contracts.
  Customers.Entities: contains a migration and the script to create the Customers table.
  Customers.Logic: contains the commands and queries for adding, editing, deleting, listing and detailing customers.
  Customers.WebApi: contains the controller that invokes the commands and queries.

The Customer folder contains the Angular project, which contains the models, services, environments and components corresponding to list, create, edit and delete customers.
