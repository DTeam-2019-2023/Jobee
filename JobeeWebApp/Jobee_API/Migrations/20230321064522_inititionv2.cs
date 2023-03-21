using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobee_API.Migrations
{
    public partial class inititionv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tbTypeAccount",
                keyColumn: "ID",
                keyValue: "ad");

            migrationBuilder.DeleteData(
                table: "tbTypeAccount",
                keyColumn: "ID",
                keyValue: "emp");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Award",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Award");

            migrationBuilder.InsertData(
                table: "tbTypeAccount",
                columns: new[] { "ID", "Description", "Name" },
                values: new object[] { "ad", "role for admin manager", "Admin" });

            migrationBuilder.InsertData(
                table: "tbTypeAccount",
                columns: new[] { "ID", "Description", "Name" },
                values: new object[] { "emp", "role for employee", "Employee" });
        }
    }
}
