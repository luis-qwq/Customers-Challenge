using System;
namespace Customers.Commons.Contracts
{
  public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
  {
    Task<TResult> HandleAsync(TQuery query);
  }
}

