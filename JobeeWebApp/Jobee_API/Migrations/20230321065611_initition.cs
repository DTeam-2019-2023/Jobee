using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobee_API.Migrations
{
    public partial class initition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "tbAccount",
                columns: new[] { "ID", "IDTypeAccount", "Passwork", "Username" },
                values: new object[] { "admin", "ad", "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tbAccount",
                keyColumn: "ID",
                keyValue: "admin");
        }
    }
}
