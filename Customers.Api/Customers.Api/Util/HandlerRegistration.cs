﻿using Customers.Commons.Contracts;
using Customers.WebApi.Decorators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Customers.WebApi.Util
{
  public static class HandlerRegistration
  {
    public static void AddHandlers(this IServiceCollection services)
    {
      List<Type> handlerTypes = Assembly.Load("Customers.Logic").GetTypes()
          .Where(x => x.GetInterfaces().Any(a => IsHandlerInterface(a)))
          .Where(x => x.Name.EndsWith("Handler", StringComparison.Ordinal))
          .ToList();

      foreach (Type type in handlerTypes)
      {
        AddHandler(services, type);
      }
    }

    private static void AddHandler(IServiceCollection services, Type type)
    {
      object[] attributes = type.GetCustomAttributes(false);

      if (attributes.Length > 0)
      {
        attributes = attributes.Where(
          att => att.GetType().Name != "NullableContextAttribute"
            && att.GetType().Name != "NullableAttribute").ToArray<object>();
      }

      List<Type> pipeline = attributes
          .Select(x => ToDecorator(x))
          .Concat(new[] { type })
          .Reverse()
          .ToList();

      Type interfaceType = type.GetInterfaces().Single(s => IsHandlerInterface(s));
      Func<IServiceProvider, object> factory = BuildPipeLine(pipeline, interfaceType);

      services.AddTransient(interfaceType, factory);
    }

    private static Func<IServiceProvider, object> BuildPipeLine(List<Type> pipeline, Type interfaceType)
    {
      List<ConstructorInfo> constructors = pipeline
          .Select(x =>
          {
            Type type = x.IsGenericType ? x.MakeGenericType(interfaceType.GenericTypeArguments) : x;
            return type.GetConstructors().Single();
          })
          .ToList();

      Func<IServiceProvider, object> func = provider =>
      {
        object current = null;
        foreach (ConstructorInfo ctor in constructors)
        {
          List<ParameterInfo> parameterInfos = ctor.GetParameters().ToList();
          object[] parameters = GetParameters(parameterInfos, current, provider);
          current = ctor.Invoke(parameters);
        }
        return current;
      };

      return func;
    }

    private static object[] GetParameters(List<ParameterInfo> parameterInfos, object current, IServiceProvider provider)
    {
      var result = new object[parameterInfos.Count];

      for (int i = 0; i < parameterInfos.Count; i++)
      {
        result[i] = GetParameter(parameterInfos[i], current, provider);
      }

      return result;
    }

    private static object GetParameter(ParameterInfo parameterInfo, object current, IServiceProvider provider)
    {
      Type parameterType = parameterInfo.ParameterType;

      if (IsHandlerInterface(parameterType))
        return current;

      object service = provider.GetService(parameterType);
      if (service != null)
        return service;

      throw new ArgumentException($"Type {parameterType} not found");
    }

    private static Type ToDecorator(object attribute)
    {
      Type type = attribute.GetType();

      if (type == typeof(AuditLogAttribute))
        return typeof(AuditLoggingDecorator<>);

      //add other decorators here

      throw new ArgumentException(attribute.ToString());
    }

    private static bool IsHandlerInterface(Type type)
    {
      if (!type.IsGenericType)
      {
        return false;
      }

      Type typeDefinition = type.GetGenericTypeDefinition();

      return typeDefinition == typeof(ICommandHandler<>) || typeDefinition == typeof(IQueryHandler<,>);
    }

  }
}

