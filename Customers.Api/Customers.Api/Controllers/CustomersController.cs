using Microsoft.AspNetCore.Mvc;
using Customers.Logic.Queries.Customers;
using Customers.Commons.Models.Request;
using Customers.Logic.Commands.Customers;
using Customers.Logic;

namespace Customers.WebApi.Controllers;

[ApiController]
[Route("api/v1/customers")]
public class CustomersController : BaseController
{

  private readonly Messages messages;

  public CustomersController(Messages messages) => this.messages = messages;

  [HttpGet("list")]
  public async Task<IActionResult> GetAllCustomers()
  {
    if (!ModelState.IsValid) { return BadRequest(); }

    return Ok(
      await messages.DispatchAsync(
        new GetCustomersQuery()));
  }

  [HttpPost]
  public async Task<IActionResult> CreateCustomer([FromBody] CustomerRequest request)
  {
    if (!ModelState.IsValid) { return BadRequest(); }

    return FromEvent(
      await messages.DispatchAsync(
        new CreateCustomerCommand(
          request.FirstName,
          request.LastName,
          request.Email)));
  }

  [HttpPatch]
  public async Task<IActionResult> UpdateCustomer([FromBody] CustomerRequest request)
  {
    if (!ModelState.IsValid) { return BadRequest(); }

    return FromEvent(
      await messages.DispatchAsync(
        new EditCustomerCommand(
          (int)request.CustomerId,
          request.FirstName,
          request.LastName,
          request.Email,
          request.RowVersion)));
  }

  [HttpDelete("{id}/rowversion/{rowVersion}")]
  public async Task<IActionResult> DeleteCustomer(int id, int rowVersion)
  {
    if (!ModelState.IsValid) { return BadRequest(); }

    return FromEvent(
      await messages.DispatchAsync(
        new DeleteCustomerCommand(id, rowVersion)));
  }
}

