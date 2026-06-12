using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuickGridDemo.Data.Migrations.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class InitialCreatePostgreSql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    ContactName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ContactTitle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    City = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Phone = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    Fax = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    TitleOfCourtesy = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    HomePhone = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    Extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeID);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContactName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ContactTitle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
