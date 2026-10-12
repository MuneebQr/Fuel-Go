using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Refuelling.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeePortalsAndHybridStocks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedTankId",
                table: "RefuelRequests",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "RefuelRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DieselCapacityLiters",
                table: "FuelTanks",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DieselStockLiters",
                table: "FuelTanks",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HiOctaneCapacityLiters",
                table: "FuelTanks",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HiOctaneStockLiters",
                table: "FuelTanks",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                column: "FuelTypeManaged",
                value: "Hi-Octane");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                column: "FuelTypeManaged",
                value: "Hi-Octane");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                column: "FuelTypeManaged",
                value: "Hi-Octane");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                column: "FuelTypeManaged",
                value: "Hi-Octane");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                column: "FuelTypeManaged",
                value: "Hi-Octane");

            migrationBuilder.UpdateData(
                table: "FuelTanks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DieselCapacityLiters", "DieselStockLiters", "HiOctaneCapacityLiters", "HiOctaneStockLiters" },
                values: new object[] { 10000.0, 10000.0, 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "FuelTanks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DieselCapacityLiters", "DieselStockLiters", "FuelType", "HiOctaneCapacityLiters", "HiOctaneStockLiters", "TankNumber" },
                values: new object[] { 10000.0, 10000.0, "Diesel", 0.0, 0.0, "Tank-D-02" });

            migrationBuilder.InsertData(
                table: "FuelTanks",
                columns: new[] { "Id", "AssignedEmployeeId", "CapacityLiters", "CurrentStockLiters", "DieselCapacityLiters", "DieselStockLiters", "FuelType", "HiOctaneCapacityLiters", "HiOctaneStockLiters", "TankNumber" },
                values: new object[,]
                {
                    { 3, null, 10000.0, 10000.0, 10000.0, 10000.0, "Diesel", 0.0, 0.0, "Tank-D-03" },
                    { 4, null, 10000.0, 10000.0, 10000.0, 10000.0, "Diesel", 0.0, 0.0, "Tank-D-04" },
                    { 5, null, 10000.0, 10000.0, 10000.0, 10000.0, "Diesel", 0.0, 0.0, "Tank-D-05" },
                    { 6, null, 10000.0, 10000.0, 5000.0, 5000.0, "Hybrid", 5000.0, 5000.0, "Tank-H-01" },
                    { 7, null, 10000.0, 10000.0, 5000.0, 5000.0, "Hybrid", 5000.0, 5000.0, "Tank-H-02" },
                    { 8, null, 10000.0, 10000.0, 5000.0, 5000.0, "Hybrid", 5000.0, 5000.0, "Tank-H-03" },
                    { 9, null, 10000.0, 10000.0, 5000.0, 5000.0, "Hybrid", 5000.0, 5000.0, "Tank-H-04" },
                    { 10, null, 10000.0, 10000.0, 5000.0, 5000.0, "Hybrid", 5000.0, 5000.0, "Tank-H-05" }
                });

            migrationBuilder.Sql(
                """
                UPDATE "RefuelRequests"
                SET "AssignedTankId" = CASE
                    WHEN "FuelType" = 'Diesel' THEN 1
                    WHEN "FuelType" = 'Hi-Octane' THEN 6
                    ELSE NULL
                END
                WHERE "AssignedTankId" IS NULL;

                UPDATE "RefuelRequests"
                SET "Status" = 'Cancelled', "AssignedTankId" = NULL
                WHERE "FuelType" NOT IN ('Diesel', 'Hi-Octane')
                  AND "Status" <> 'Completed';
                """);

            migrationBuilder.CreateIndex(
                name: "IX_RefuelRequests_AssignedTankId",
                table: "RefuelRequests",
                column: "AssignedTankId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefuelRequests_FuelTanks_AssignedTankId",
                table: "RefuelRequests",
                column: "AssignedTankId",
                principalTable: "FuelTanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefuelRequests_FuelTanks_AssignedTankId",
                table: "RefuelRequests");

            migrationBuilder.DropIndex(
                name: "IX_RefuelRequests_AssignedTankId",
                table: "RefuelRequests");

            migrationBuilder.DeleteData(
                table: "FuelTanks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FuelTanks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FuelTanks",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FuelTanks",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "FuelTanks",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "FuelTanks",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "FuelTanks",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "FuelTanks",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DropColumn(
                name: "AssignedTankId",
                table: "RefuelRequests");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "RefuelRequests");

            migrationBuilder.DropColumn(
                name: "DieselCapacityLiters",
                table: "FuelTanks");

            migrationBuilder.DropColumn(
                name: "DieselStockLiters",
                table: "FuelTanks");

            migrationBuilder.DropColumn(
                name: "HiOctaneCapacityLiters",
                table: "FuelTanks");

            migrationBuilder.DropColumn(
                name: "HiOctaneStockLiters",
                table: "FuelTanks");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7,
                column: "FuelTypeManaged",
                value: "Petrol & Hi-Octane");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8,
                column: "FuelTypeManaged",
                value: "Petrol & Hi-Octane");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9,
                column: "FuelTypeManaged",
                value: "Petrol & Hi-Octane");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10,
                column: "FuelTypeManaged",
                value: "Petrol & Hi-Octane");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11,
                column: "FuelTypeManaged",
                value: "Petrol & Hi-Octane");

            migrationBuilder.UpdateData(
                table: "FuelTanks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FuelType", "TankNumber" },
                values: new object[] { "Petrol & Hi-Octane", "Tank-PO-01" });
        }
    }
}
