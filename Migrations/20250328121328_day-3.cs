using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployManager.Migrations
{
    /// <inheritdoc />
    public partial class day3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Allowance",
                table: "Employees",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Allowance", "PasswordHash" },
                values: new object[] { 0.0, "$2a$11$A5FTEZSmkic/7XyBIr/WM.OhirHgtw4f0zwQOCVbUCFhNhAdHX3U6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allowance",
                table: "Employees");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$1htHxbxobip/hJ9NBA6Q6uPOPXU7DrrDdEVfuFEAa2WrmCJKiw8I2");
        }
    }
}
