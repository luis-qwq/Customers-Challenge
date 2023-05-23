using System;
using Customers.Commons.Contracts;
using Newtonsoft.Json;

namespace Customers.WebApi.Decorators
{
  public class AuditLoggingDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
  {
    private readonly ICommandHandler<TCommand> _handler;

    public AuditLoggingDecorator(ICommandHandler<TCommand> handler)
    {
      _handler = handler;
    }

    public async Task<IEvent> HandleAsync(TCommand command)
    {
      string commandJson = JsonConvert.SerializeObject(command);
      //log
      Console.WriteLine($"{command.GetType().Name}: {commandJson}");
      return await _handler.HandleAsync(command);
    }
  }
}

