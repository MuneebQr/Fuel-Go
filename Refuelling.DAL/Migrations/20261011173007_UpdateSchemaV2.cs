using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Refuelling.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarNumber",
                table: "RefuelRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CnicNumber",
                table: "RefuelRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Employees",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "PhoneNumber",
                value: "0300-1111111");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                column: "PhoneNumber",
                value: "0300-222222");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3,
                column: "PhoneNumber",
                value: "0300-222223");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4,
                column: "PhoneNumber",
                value: "0300-222224");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5,
                column: "PhoneNumber",
                value: "0300-222225");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6,
                column: "PhoneNumber",
                value: "0300-222226");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                column: "PhoneNumber",
                value: "0300-333337");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                column: "PhoneNumber",
                value: "0300-333338");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                column: "PhoneNumber",
                value: "0300-333339");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                column: "PhoneNumber",
                value: "0300-3333310");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                column: "PhoneNumber",
                value: "0300-3333311");

            migrationBuilder.CreateIndex(
                name: "IX_RefuelRequests_AssignedEmployeeId",
                table: "RefuelRequests",
                column: "AssignedEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefuelRequests_Employees_AssignedEmployeeId",
                table: "RefuelRequests",
                column: "AssignedEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefuelRequests_Employees_AssignedEmployeeId",
                table: "RefuelRequests");

            migrationBuilder.DropIndex(
                name: "IX_RefuelRequests_AssignedEmployeeId",
                table: "RefuelRequests");

            migrationBuilder.DropColumn(
                name: "CarNumber",
                table: "RefuelRequests");

            migrationBuilder.DropColumn(
                name: "CnicNumber",
                table: "RefuelRequests");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Employees");
        }
    }
}
