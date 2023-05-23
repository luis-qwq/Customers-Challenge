using System;
using System.Data;
using System.Data.Common;
using Customers.Commons.Contracts;
using Customers.Logic.Events;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Customers.Logic.Commands.Customers
{
  public class DeleteCustomerCommand : ICommand
  {
    public int CustomerId { get; }
    public int RowVersion { get; }

    public DeleteCustomerCommand(int customerId, int rowVersion)
    {
      CustomerId = customerId;
      RowVersion = rowVersion;
    }
  }

  public sealed class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand>
  {
    private readonly ConnectionString _cs;

    public DeleteCustomerCommandHandler(ConnectionString cs) => _cs = cs;

    public async Task<IEvent> HandleAsync(DeleteCustomerCommand command)
    {
      var sqlQuery = @"
DELETE
FROM Customer
WHERE CustomerId = @pCustomerId
AND CAST(RowVersion AS INT) = @pRowVersion;
";
      using (SqlConnection conn = new SqlConnection(_cs.Value))
      {
        await conn.OpenAsync();

        using (DbTransaction tran = await conn.BeginTransactionAsync())
        {
          try
          {
            var parameters = new DynamicParameters();
            parameters.Add("@pCustomerId", command.CustomerId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@pRowVersion", command.RowVersion, DbType.Int32, ParameterDirection.Input);

            await conn.QueryAsync(
              sqlQuery,
              parameters,
              tran,
              commandType: CommandType.Text);

            await tran.CommitAsync();
          }
          catch (Exception ex)
          {
            await tran.RollbackAsync();
            throw;
          }
        }
      }
      return new EventBase();
    }
  }

}

