using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployManager.Migrations
{
    /// <inheritdoc />
    public partial class day2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$1htHxbxobip/hJ9NBA6Q6uPOPXU7DrrDdEVfuFEAa2WrmCJKiw8I2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Hai4KiCD0w92rVMTjHxOQ.F3oHyXvo0ovGafSYt.BuJEcOxs5GVEK");
        }
    }
}
