using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskSystem.Migrations
{
    /// <inheritdoc />
    public partial class RenameTicketResolutionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketResolution_AspNetUsers_ResolvedById",
                table: "TicketResolution");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketResolution_Tickets_TicketId",
                table: "TicketResolution");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketResolution",
                table: "TicketResolution");

            migrationBuilder.DropIndex(
                name: "IX_TicketResolution_TicketId",
                table: "TicketResolution");

            migrationBuilder.RenameTable(
                name: "TicketResolution",
                newName: "TicketResolutions");

            migrationBuilder.RenameIndex(
                name: "IX_TicketResolution_ResolvedById",
                table: "TicketResolutions",
                newName: "IX_TicketResolutions_ResolvedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketResolutions",
                table: "TicketResolutions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TicketResolutions_TicketId",
                table: "TicketResolutions",
                column: "TicketId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketResolutions_AspNetUsers_ResolvedById",
                table: "TicketResolutions",
                column: "ResolvedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketResolutions_Tickets_TicketId",
                table: "TicketResolutions",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketResolutions_AspNetUsers_ResolvedById",
                table: "TicketResolutions");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketResolutions_Tickets_TicketId",
                table: "TicketResolutions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketResolutions",
                table: "TicketResolutions");

            migrationBuilder.DropIndex(
                name: "IX_TicketResolutions_TicketId",
                table: "TicketResolutions");

            migrationBuilder.RenameTable(
                name: "TicketResolutions",
                newName: "TicketResolution");

            migrationBuilder.RenameIndex(
                name: "IX_TicketResolutions_ResolvedById",
                table: "TicketResolution",
                newName: "IX_TicketResolution_ResolvedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketResolution",
                table: "TicketResolution",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TicketResolution_TicketId",
                table: "TicketResolution",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketResolution_AspNetUsers_ResolvedById",
                table: "TicketResolution",
                column: "ResolvedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketResolution_Tickets_TicketId",
                table: "TicketResolution",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
