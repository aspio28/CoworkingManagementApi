using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoworkingManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStatusForRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Rooms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Rooms",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
