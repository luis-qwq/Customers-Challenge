using System.Data;
using Customers.Commons.Contracts;
using Customers.Commons.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customers.Logic.Queries.Customers
{
  public class GetCustomersQuery : IQuery<IEnumerable<Customer>>
  {
    public GetCustomersQuery() { }
  }

  public sealed class GetCustomersQueryHandler : IQueryHandler<GetCustomersQuery, IEnumerable<Customer>>
  {
    private readonly ConnectionString _cs;

    public GetCustomersQueryHandler(ConnectionString cs) => _cs = cs;

    public async Task<IEnumerable<Customer>> HandleAsync(GetCustomersQuery query)
    {
      var sqlQuery = @"
SELECT CustomerId,
  FirstName,
  LastName,
  Email,
  CreatedDate,
  ModifiedDate,
  CAST(RowVersion AS INT) as RowVersion
FROM Customer;
";

      using (SqlConnection conn = new SqlConnection(_cs.Value))
      {
        await conn.OpenAsync();

        return await conn.QueryAsync<Customer>(
          sqlQuery,
          null,
          commandType: CommandType.Text);
      }
    }
  }

}

