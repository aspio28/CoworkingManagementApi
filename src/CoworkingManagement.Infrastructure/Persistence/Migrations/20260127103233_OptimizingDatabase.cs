using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoworkingManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OptimizingDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_reservations_room_id",
                table: "Reservations");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:btree_gist", ",,");

            migrationBuilder.CreateIndex(
                name: "ix_rooms_location_capacity",
                table: "Rooms",
                columns: new[] { "location", "capacity" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Availability_Lookup",
                table: "Reservations",
                columns: new[] { "room_id", "start_date", "end_date" },
                filter: "\"status\" = 'Reserved'")
                .Annotation("Npgsql:IndexMethod", "gist");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_rooms_location_capacity",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_Availability_Lookup",
                table: "Reservations");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:btree_gist", ",,");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_room_id",
                table: "Reservations",
                column: "room_id");
        }
    }
}
