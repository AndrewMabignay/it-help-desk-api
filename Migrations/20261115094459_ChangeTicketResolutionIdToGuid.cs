using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskSystem.Migrations
{
    public partial class ChangeTicketResolutionIdToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketResolution",
                table: "TicketResolution");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TicketResolution");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "TicketResolution",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketResolution",
                table: "TicketResolution",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketResolution",
                table: "TicketResolution");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TicketResolution");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TicketResolution",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketResolution",
                table: "TicketResolution",
                column: "Id");
        }
    }
}