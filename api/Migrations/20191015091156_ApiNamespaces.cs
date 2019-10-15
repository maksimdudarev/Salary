using Microsoft.EntityFrameworkCore.Migrations;

namespace MD.Salary.WebApi.Migrations
{
    public partial class ApiNamespaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Employees",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
