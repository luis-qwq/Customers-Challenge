using System;
namespace Customers.Commons.Contracts
{
  public interface IEventHandler<TEvent> where TEvent : IEvent
  {
    Task<TEvent> HandleAsync(TEvent command);
  }
}

