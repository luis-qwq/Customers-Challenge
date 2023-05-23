using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Customers.Commons.Contracts;
using Customers.WebApi.Util;
using Microsoft.AspNetCore.Mvc;

namespace Customers.WebApi.Controllers
{
  public class BaseController : Controller
  {
    protected new IActionResult Ok()
    {
      return base.Ok(Envelope.Ok());
    }

    protected IActionResult Ok<T>(T result)
    {
      return base.Ok(Envelope.Ok(result));
    }

    protected IActionResult Error(string errorMessage)
    {
      return BadRequest(Envelope.Error(errorMessage));
    }

    protected IActionResult FromEvent(IEvent evento)
    {
      return base.Ok(Envelope.Ok(evento));
    }
  }
}

