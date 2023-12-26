using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReptileTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "account",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    last_updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reset_token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reset_token_expiration = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "feeding_event",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reptile_id = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    food_type = table.Column<int>(type: "int", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feeding_event", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "length",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reptile_id = table.Column<int>(type: "int", nullable: false),
                    measure = table.Column<int>(type: "int", nullable: false),
                    measurement_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_length", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reptile",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    species = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reptile_type = table.Column<int>(type: "int", nullable: false),
                    account_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reptile", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shedding_event",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reptile_id = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shedding_event", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "weight",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reptile_id = table.Column<int>(type: "int", nullable: false),
                    weighing = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weight", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "feeding_event",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "length",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "reptile",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "shedding_event",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "weight",
                schema: "dbo");
        }
    }
}
