using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskSystem.Migrations
{
    public partial class AddUniqueIndexToTicketWatchers : Migration
    {
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