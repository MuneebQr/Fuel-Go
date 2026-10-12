using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Refuelling.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    FuelTypeManaged = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuelTanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TankNumber = table.Column<string>(type: "TEXT", nullable: false),
                    FuelType = table.Column<string>(type: "TEXT", nullable: false),
                    CapacityLiters = table.Column<double>(type: "REAL", nullable: false),
                    CurrentStockLiters = table.Column<double>(type: "REAL", nullable: false),
                    AssignedEmployeeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelTanks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefuelRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    FuelType = table.Column<string>(type: "TEXT", nullable: false),
                    QuantityLiters = table.Column<double>(type: "REAL", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    AssignedEmployeeId = table.Column<int>(type: "INTEGER", nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefuelRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsOnDuty = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "FuelTypeManaged", "IsActive", "Name", "Role" },
                values: new object[,]
                {
                    { 1, "All", true, "System Admin", "Admin" },
                    { 2, "Diesel", true, "Diesel Supplier 1", "Supplier" },
                    { 3, "Diesel", true, "Diesel Supplier 2", "Supplier" },
                    { 4, "Diesel", true, "Diesel Supplier 3", "Supplier" },
                    { 5, "Diesel", true, "Diesel Supplier 4", "Supplier" },
                    { 6, "Diesel", true, "Diesel Supplier 5", "Supplier" },
                    { 7, "Petrol & Hi-Octane", true, "Multi-Fuel Supplier 1", "Supplier" },
                    { 8, "Petrol & Hi-Octane", true, "Multi-Fuel Supplier 2", "Supplier" },
                    { 9, "Petrol & Hi-Octane", true, "Multi-Fuel Supplier 3", "Supplier" },
                    { 10, "Petrol & Hi-Octane", true, "Multi-Fuel Supplier 4", "Supplier" },
                    { 11, "Petrol & Hi-Octane", true, "Multi-Fuel Supplier 5", "Supplier" }
                });

            migrationBuilder.InsertData(
                table: "FuelTanks",
                columns: new[] { "Id", "AssignedEmployeeId", "CapacityLiters", "CurrentStockLiters", "FuelType", "TankNumber" },
                values: new object[,]
                {
                    { 1, null, 10000.0, 10000.0, "Diesel", "Tank-D-01" },
                    { 2, null, 10000.0, 10000.0, "Petrol & Hi-Octane", "Tank-PO-01" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_EmployeeId",
                table: "Shifts",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelTanks");

            migrationBuilder.DropTable(
                name: "RefuelRequests");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
