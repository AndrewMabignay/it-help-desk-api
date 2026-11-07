using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskSystem.Migrations
{
    public partial class ChangeTicketAttachmentIdToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Remove existing TicketAttachments primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketAttachments",
                table: "TicketAttachments"
            );

            // 2. Drop the old INT Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "TicketAttachments"
            );

            // 3. Recreate Id as GUID
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "TicketAttachments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()"
            );

            // 4. Restore TicketAttachments primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketAttachments",
                table: "TicketAttachments",
                column: "Id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Remove the GUID primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketAttachments",
                table: "TicketAttachments"
            );

            // 2. Drop the GUID Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "TicketAttachments"
            );

            // 3. Recreate Id as INT
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TicketAttachments",
                type: "int",
                nullable: false
            )
            .Annotation("SqlServer:Identity", "1, 1");

            // 4. Restore primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketAttachments",
                table: "TicketAttachments",
                column: "Id"
            );
        }
    }
}