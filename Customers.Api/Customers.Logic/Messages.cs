using System;
using Customers.Commons.Contracts;
using System.Text;

namespace Customers.Logic
{
  public sealed class Messages
  {
    private readonly IServiceProvider _provider;

    public Messages(IServiceProvider provider) => _provider = provider;

    public async Task<IEvent> DispatchAsync(ICommand command)
    {
      Type type = typeof(ICommandHandler<>);
      Type[] typeArgs = { command.GetType() };
      Type handlerType = type.MakeGenericType(typeArgs);
      dynamic handler = _provider.GetService(handlerType);
      dynamic message = (dynamic)command;
      IEvent eventArgs = await handler.HandleAsync(message);
      return eventArgs;
    }

    public async Task<T>DispatchAsync<T>(IQuery<T> query)
    {
      Type type = typeof(IQueryHandler<,>);
      Type[] typeArgs = { query.GetType(), typeof(T) };
      Type handlerType = type.MakeGenericType(typeArgs);
      dynamic handler = _provider.GetService(handlerType);
      dynamic message = (dynamic)query;
      T result = await handler.HandleAsync(message);
      return result;
    }

//    private string GetParams(dynamic message)
//    {
//      var lineParams = "";
//#if DEBUG
//      StringBuilder sbParams = new StringBuilder();
//      var properties = message.GetType().GetProperties();
//      foreach (var property in properties)
//      {
//        var value = message.GetType()
//            .GetProperty(property.Name)
//            .GetValue(message, null);
//        sbParams.Append($"'{property.Name}': '{value}', ");
//      }
//      lineParams = sbParams.ToString();
//#endif
//      return lineParams;
//    }
  }
}

