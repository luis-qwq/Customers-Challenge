using System;
namespace Customers.Commons.Contracts
{
  public interface ICommandHandler<TCommand> where TCommand : ICommand
  {
    Task<IEvent> HandleAsync(TCommand command);
  }
}

