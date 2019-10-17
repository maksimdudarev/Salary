using Microsoft.EntityFrameworkCore.Migrations;

namespace MD.Salary.WebApi.Migrations
{
    public partial class UsersDelEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Employee",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Employee",
                table: "Users",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
