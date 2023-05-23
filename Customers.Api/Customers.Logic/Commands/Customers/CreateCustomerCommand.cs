using System;
using System.Data;
using System.Data.Common;
using Customers.Commons.Contracts;
using Customers.Logic.Events;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Customers.Logic.Commands.Customers
{
  public class CreateCustomerCommand : ICommand
  {
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }

    public CreateCustomerCommand(
      string firstName,
      string lastName,
      string email)
    {
      FirstName = firstName;
      LastName = lastName;
      Email = email;
    }
  }

  public sealed class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
  {
    private readonly ConnectionString _cs;

    public CreateCustomerCommandHandler(ConnectionString cs) => _cs = cs;

    public async Task<IEvent> HandleAsync(CreateCustomerCommand command)
    {
      var sqlQuery = @"
INSERT INTO Customer
(
  FirstName,
  LastName,
  Email,
  CreatedDate
)
VALUES
(
  @pFirstName,
  @pLastName,
  @pEmail,
  GETUTCDATE()
);
";

      using (SqlConnection conn = new SqlConnection(_cs.Value))
      {
        await conn.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@pFirstName", command.FirstName, DbType.String, ParameterDirection.Input);
        parameters.Add("@pLastName", command.LastName, DbType.String, ParameterDirection.Input);
        parameters.Add("@pEmail", command.Email, DbType.String, ParameterDirection.Input);

        using (DbTransaction tran = await conn.BeginTransactionAsync())
        {
          try
          {
            await conn.ExecuteAsync(
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

