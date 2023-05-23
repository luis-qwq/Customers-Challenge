using System;
namespace Customers.Commons.Models
{
  public class Customer
  {
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int RowVersion { get; set; }
  }
}

