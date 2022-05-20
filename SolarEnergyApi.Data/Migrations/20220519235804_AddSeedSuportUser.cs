using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEnergyApi.Data.Migrations
{
    public partial class AddSeedSuportUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 1, "a04f0ba7-6230-49d4-8c40-c6a6d744b34d", "suport", "SUPORT" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordExpired", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, 0, "1050eff9-d1cf-41e2-aab8-34cb44421f07", "suporte@suporte.com", true, false, null, "SUPORTE@SUPORTE.COM", "SUPORTE@SUPORTE.COM", "18/05/2022", "AQAAAAEAACcQAAAAECeHuGWulLRI55a4heK4SrZ41NziYj6EH5x8HPuXdbv2/j4OuLodptWmRbrQqRlkFg==", null, false, null, false, "suporte@suporte.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
