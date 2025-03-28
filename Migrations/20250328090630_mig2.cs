using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployManager.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BasicSalary",
                table: "Employees",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Bonus",
                table: "Employees",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Deduction",
                table: "Employees",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BasicSalary", "Bonus", "Deduction", "PasswordHash" },
                values: new object[] { 0.0, 0.0, 0.0, "$2a$11$Hai4KiCD0w92rVMTjHxOQ.F3oHyXvo0ovGafSYt.BuJEcOxs5GVEK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasicSalary",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Bonus",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Deduction",
                table: "Employees");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$YU6SMAkUNCgp25MNCTbFLu7VG1keSX5mMsOa.rCWMglsmeqPX9Eta");
        }
    }
}
