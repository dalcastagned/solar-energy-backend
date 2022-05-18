using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarEnergyApi.Data.Migrations
{
    public partial class addPasswordExpired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PasswordExpired",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordExpired",
                table: "AspNetUsers");
        }
    }
}
