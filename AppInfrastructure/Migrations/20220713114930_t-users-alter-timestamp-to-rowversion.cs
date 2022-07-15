using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppInfrastructure.Migrations
{
    public partial class tusersaltertimestamptorowversion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Users",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Accounts",
                newName: "RowVersion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "Users",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "Accounts",
                newName: "Timestamp");
        }
    }
}
