using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixTicketWatcherUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TicketWatchers_TicketId_UserId",
                table: "TicketWatchers");

            migrationBuilder.CreateIndex(
                name: "IX_TicketWatchers_TicketId_UserId",
                table: "TicketWatchers",
                columns: new[] { "TicketId", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TicketWatchers_TicketId_UserId",
                table: "TicketWatchers");

            migrationBuilder.CreateIndex(
                name: "IX_TicketWatchers_TicketId_UserId",
                table: "TicketWatchers",
                columns: new[] { "TicketId", "UserId" });
        }
    }
}
