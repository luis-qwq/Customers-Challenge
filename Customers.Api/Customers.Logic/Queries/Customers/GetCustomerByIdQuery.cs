using System;
using System.Data;
using Customers.Commons.Contracts;
using Customers.Commons.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Customers.Logic.Queries.Customers
{
  public class GetCustomerByIdQuery : IQuery<Customer>
  {
    public int CustomerId { get; set; }

    public GetCustomerByIdQuery(int customerId) => CustomerId = customerId;
  }

  public sealed class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, Customer>
  {
    private readonly ConnectionString _cs;

    public GetCustomerByIdQueryHandler(ConnectionString cs) => _cs = cs;

    public async Task<Customer> HandleAsync(GetCustomerByIdQuery query)
    {
      var sqlQuery = @"
SELECT CustomerId,
  FirstName,
  LastName,
  Email,
  CreatedDate,
  ModifiedDate,
  CAST(RowVersion AS INT) as RowVersion
FROM Customer
WHERE CustomerId = @pCustomerId;
";

      using (SqlConnection conn = new SqlConnection(_cs.Value)) {
        var parameters = new DynamicParameters();
        parameters.Add("@pCustomerId", query.CustomerId, DbType.Int32, ParameterDirection.Input);

        return await conn.QueryFirstOrDefaultAsync<Customer>(
          sqlQuery,
          parameters,
          commandType: CommandType.Text);
      }
    }
  }

}

