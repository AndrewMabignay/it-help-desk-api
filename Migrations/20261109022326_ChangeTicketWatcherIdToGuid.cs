using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTicketWatcherIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketWatchers",
                table: "TicketWatchers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TicketWatchers");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "TicketWatchers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketWatchers",
                table: "TicketWatchers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
               name: "PK_TicketWatchers",
               table: "TicketWatchers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TicketWatchers");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TicketWatchers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketWatchers",
                table: "TicketWatchers",
                column: "Id");
        }
    }
}
