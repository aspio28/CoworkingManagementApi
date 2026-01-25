using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoworkingManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCreaterAtAndUpdateAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "end_time",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "start_time",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Reservations",
                newName: "start_date");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Rooms",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "Rooms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "Rooms",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "cancelled_at",
                table: "Reservations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Reservations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "end_date",
                table: "Reservations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Reservations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "Reservations",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "location",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "cancelled_at",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "end_date",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "Reservations",
                newName: "date");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "end_time",
                table: "Reservations",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "start_time",
                table: "Reservations",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
