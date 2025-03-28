using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployManager.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$YU6SMAkUNCgp25MNCTbFLu7VG1keSX5mMsOa.rCWMglsmeqPX9Eta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$e08lo75s.kwOy4D9ZPXN/.Cr.PQSLoyeifyj9jzTW7ADoPIXnexUO");
        }
    }
}
