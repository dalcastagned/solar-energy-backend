using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEnergyApi.Data.Migrations
{
    public partial class removeRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordExpired", "PasswordHash" },
                values: new object[] { "a42926c5-239d-4a3c-9174-964cb35921ad", "30/05/2022", "AQAAAAEAACcQAAAAEBWwOBcS6LIr4SkPodc4qcpCHdgd3XTd18byY/Is8p2IVi4SLcTa20hW4wKF5EmRXw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "55af4041-ac25-4312-868b-41a614161d37", "admin", "ADMIN" },
                    { 2, "0b16c298-995a-4504-aee6-c8523be4d52a", "employee", "EMPLOYEE" },
                    { 3, "230d39d4-2af0-433a-82e8-513b726405c2", "visitor", "VISITOR" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordExpired", "PasswordHash" },
                values: new object[] { "74616f8c-6e01-460a-8ac5-dcd2e1791b95", "19/05/2022", "AQAAAAEAACcQAAAAEIdIe0t59Pr09fFulIXpkISiqimIdnnbqymXmGeuo2hKVAlVxTrmUdgnrCb6AlR6iw==" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 2, 1 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 3, 1 });
        }
    }
}
