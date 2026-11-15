using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskSystem.Migrations
{
    public partial class ChangeTicketStatusHistoryIdToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketStatusHistories",
                table: "TicketStatusHistories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TicketStatusHistories");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "TicketStatusHistories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketStatusHistories",
                table: "TicketStatusHistories",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketStatusHistories",
                table: "TicketStatusHistories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TicketStatusHistories");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TicketStatusHistories",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketStatusHistories",
                table: "TicketStatusHistories",
                column: "Id");
        }
    }
}