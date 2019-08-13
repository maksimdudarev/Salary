using Microsoft.EntityFrameworkCore.Migrations;

namespace MD.Salary.WebApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Employees",
            //    columns: table => new
            //    {
            //        ID = table.Column<long>(nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        Name = table.Column<string>(nullable: true),
            //        HireDate = table.Column<long>(nullable: false),
            //        Group = table.Column<string>(nullable: true),
            //        SalaryBase = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
            //        SuperiorID = table.Column<long>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Employees", x => x.ID);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
