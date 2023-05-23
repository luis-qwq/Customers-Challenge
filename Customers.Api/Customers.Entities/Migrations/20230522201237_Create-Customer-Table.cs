using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customers.Entities.Migrations
{
  /// <inheritdoc />
  public partial class CreateCustomerTable : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: "Customer",
        columns: table => new
        {
          CustomerId = table.Column<int>(
            nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
          FirstName = table.Column<string>(
            nullable: false,
            unicode: false,
            maxLength: 100),
          LastName = table.Column<string>(
            nullable: false,
            unicode: false,
            maxLength: 100),
          Email = table.Column<string>(
            nullable: false,
            unicode: false,
            maxLength: 100),
          CreatedDate = table.Column<DateTime>(
            nullable: true),
          ModifiedDate = table.Column<DateTime>(
            nullable: true),
          RowVersion = table.Column<byte[]>(
            nullable: true,
            rowVersion: true)
        },
        constraints: table =>
        {
          table.PrimaryKey(name: "PK_Customer", x => x.CustomerId);
        });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable("Customer");
    }
  }
}
