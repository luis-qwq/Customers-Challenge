using System;
using System.Data;
using System.Data.Common;
using Customers.Commons.Contracts;
using Customers.Logic.Events;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Customers.Logic.Commands.Customers
{
  public class EditCustomerCommand : ICommand
  {
    public int CustomerId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public int RowVersion { get; set; }

    public EditCustomerCommand(int customerId,
      string firstName,
      string lastName,
      string email,
      int rowVersion)
    {
      CustomerId = customerId;
      FirstName = firstName;
      LastName = lastName;
      Email = email;
      RowVersion = rowVersion;
    }
  }

  public sealed class EditCustomerCommandHandler : ICommandHandler<EditCustomerCommand>
  {
    private readonly ConnectionString _cs;

    public EditCustomerCommandHandler(ConnectionString cs) => _cs = cs;

    public async Task<IEvent> HandleAsync(EditCustomerCommand command)
    {
      var sqlQuery = @"
UPDATE Customer
SET FirstName = @pFirstName,
  LastName = @pLastName,
  Email = @pEmail,
  ModifiedDate = GETUTCDATE()
WHERE CustomerId = @pCustomerId
  AND RowVersion = @pRowVersion;
";

      using (SqlConnection conn = new SqlConnection(_cs.Value))
      {
        await conn.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@pCustomerId", command.CustomerId, DbType.Int32, ParameterDirection.Input);
        parameters.Add("@pFirstName", command.FirstName, DbType.String, ParameterDirection.Input);
        parameters.Add("@pLastName", command.LastName, DbType.String, ParameterDirection.Input);
        parameters.Add("@pEmail", command.Email, DbType.String, ParameterDirection.Input);
        parameters.Add("@pRowVersion", command.RowVersion, DbType.Int32, ParameterDirection.Input);

        using (DbTransaction tran = await conn.BeginTransactionAsync())
        {
          try
          {
            await conn.ExecuteScalarAsync<int>(
              sqlQuery,
              parameters,
              tran,
              commandType: CommandType.Text);

            await tran.CommitAsync();
          }
          catch(Exception ex)
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

