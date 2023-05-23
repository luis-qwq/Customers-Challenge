using System;
namespace Customers.Commons.Models.Request
{
  public class CustomerRequest
  {
    public int? CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int RowVersion { get; set; }
  }
}

