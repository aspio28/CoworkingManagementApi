using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoworkingManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Users",
                newName: "last_modified_at");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Rooms",
                newName: "last_modified_at");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Reservations",
                newName: "last_modified_at");

            migrationBuilder.AddColumn<Guid>(
                name: "created_by",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "last_modified_by",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "created_by",
                table: "Rooms",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "last_modified_by",
                table: "Rooms",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "created_by",
                table: "Reservations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "last_modified_by",
                table: "Reservations",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "last_modified_at",
                table: "Users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "last_modified_at",
                table: "Rooms",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "last_modified_at",
                table: "Reservations",
                newName: "updated_at");
        }
    }
}
