
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskSystem.Migrations
{
    public partial class ChangePriorityIdToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Remove the existing Priority foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Priorities_PriorityId",
                table: "Tickets"
            );

            // 2. Remove the existing PriorityId index
            migrationBuilder.DropIndex(
                name: "IX_Tickets_PriorityId",
                table: "Tickets"
            );

            // 3. Remove the Priority primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_Priorities",
                table: "Priorities"
            );

            // 4. Drop the old INT columns
            migrationBuilder.DropColumn(
                name: "PriorityId",
                table: "Tickets"
            );

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Priorities"
            );

            // 5. Recreate Priority Id as GUID
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Priorities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()"
            );

            // 6. Recreate Ticket PriorityId as GUID
            migrationBuilder.AddColumn<Guid>(
                name: "PriorityId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty
            );

            // 7. Restore Priority primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_Priorities",
                table: "Priorities",
                column: "Id"
            );

            // 8. Restore PriorityId index
            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PriorityId",
                table: "Tickets",
                column: "PriorityId"
            );

            // 9. Restore foreign key relationship
            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Priorities_PriorityId",
                table: "Tickets",
                column: "PriorityId",
                principalTable: "Priorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Priorities_PriorityId",
                table: "Tickets"
            );

            migrationBuilder.DropIndex(
                name: "IX_Tickets_PriorityId",
                table: "Tickets"
            );

            migrationBuilder.DropPrimaryKey(
                name: "PK_Priorities",
                table: "Priorities"
            );

            migrationBuilder.DropColumn(
                name: "PriorityId",
                table: "Tickets"
            );

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Priorities"
            );

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Priorities",
                type: "int",
                nullable: false,
                defaultValue: 0
            )
            .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "PriorityId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.AddPrimaryKey(
                name: "PK_Priorities",
                table: "Priorities",
                column: "Id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PriorityId",
                table: "Tickets",
                column: "PriorityId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Priorities_PriorityId",
                table: "Tickets",
                column: "PriorityId",
                principalTable: "Priorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );
        }
    }
}
