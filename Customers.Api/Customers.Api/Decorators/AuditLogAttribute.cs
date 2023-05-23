using System;
namespace Customers.WebApi.Decorators
{

  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
  public sealed class AuditLogAttribute : Attribute
  {
    public AuditLogAttribute()
    {
    }
  }

}

