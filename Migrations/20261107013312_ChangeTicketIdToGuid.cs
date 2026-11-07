
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskSystem.Migrations
{
    public partial class ChangeTicketIdToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Drop existing foreign keys
            migrationBuilder.DropForeignKey(
                name: "FK_TicketWatchers_Tickets_TicketId",
                table: "TicketWatchers");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketStatusHistories_Tickets_TicketId",
                table: "TicketStatusHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketResolution_Tickets_TicketId",
                table: "TicketResolution");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketComments_Tickets_TicketId",
                table: "TicketComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAttachments_Tickets_TicketId",
                table: "TicketAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAssignments_Tickets_TicketId",
                table: "TicketAssignments");

            // 2. Drop existing indexes
            migrationBuilder.DropIndex(
                name: "IX_TicketWatchers_TicketId_UserId",
                table: "TicketWatchers");

            migrationBuilder.DropIndex(
                name: "IX_TicketStatusHistories_TicketId",
                table: "TicketStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_TicketResolution_TicketId",
                table: "TicketResolution");

            migrationBuilder.DropIndex(
                name: "IX_TicketComments_TicketId",
                table: "TicketComments");

            migrationBuilder.DropIndex(
                name: "IX_TicketAttachments_TicketId",
                table: "TicketAttachments");

            migrationBuilder.DropIndex(
                name: "IX_TicketAssignments_TicketId",
                table: "TicketAssignments");

            // 3. Drop existing primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets");

            // 4. Drop old INT TicketId columns
            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "TicketWatchers");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "TicketStatusHistories");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "TicketResolution");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "TicketComments");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "TicketAttachments");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "TicketAssignments");

            // 5. Drop old Tickets.Id
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Tickets");

            // 6. Recreate Tickets.Id as GUID
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            // 7. Recreate dependent TicketId columns as GUID
            migrationBuilder.AddColumn<Guid>(
                name: "TicketId",
                table: "TicketWatchers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.AddColumn<Guid>(
                name: "TicketId",
                table: "TicketStatusHistories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.AddColumn<Guid>(
                name: "TicketId",
                table: "TicketResolution",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.AddColumn<Guid>(
                name: "TicketId",
                table: "TicketComments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.AddColumn<Guid>(
                name: "TicketId",
                table: "TicketAttachments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.AddColumn<Guid>(
                name: "TicketId",
                table: "TicketAssignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            // 8. Restore Tickets primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets",
                column: "Id");

            // 9. Recreate indexes
            migrationBuilder.CreateIndex(
                name: "IX_TicketWatchers_TicketId_UserId",
                table: "TicketWatchers",
                columns: new[] { "TicketId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_TicketStatusHistories_TicketId",
                table: "TicketStatusHistories",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketResolution_TicketId",
                table: "TicketResolution",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_TicketId",
                table: "TicketComments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAttachments_TicketId",
                table: "TicketAttachments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAssignments_TicketId",
                table: "TicketAssignments",
                column: "TicketId");

            // 10. Restore foreign keys
            migrationBuilder.AddForeignKey(
                name: "FK_TicketWatchers_Tickets_TicketId",
                table: "TicketWatchers",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketStatusHistories_Tickets_TicketId",
                table: "TicketStatusHistories",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketResolution_Tickets_TicketId",
                table: "TicketResolution",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketComments_Tickets_TicketId",
                table: "TicketComments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAttachments_Tickets_TicketId",
                table: "TicketAttachments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAssignments_Tickets_TicketId",
                table: "TicketAssignments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse migration is omitted here intentionally.
            // Generate a proper reverse migration if rollback is required.
            throw new NotSupportedException(
                "Rolling back this migration requires restoring the original Ticket IDs and relationships.");
        }
    }
}
