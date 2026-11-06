
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskSystem.Migrations
{
    public partial class ChangeCategoryIdToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Remove the existing Category foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Categories_CategoryId",
                table: "Tickets"
            );

            // 2. Remove the existing CategoryId index
            migrationBuilder.DropIndex(
                name: "IX_Tickets_CategoryId",
                table: "Tickets"
            );

            // 3. Remove the Category primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories"
            );

            // 4. Drop the old INT columns
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Tickets"
            );

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Categories"
            );

            // 5. Recreate Category Id as GUID
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()"
            );

            // 6. Recreate Ticket CategoryId as GUID
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty
            );

            // 7. Restore Category primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id"
            );

            // 8. Restore CategoryId index
            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CategoryId",
                table: "Tickets",
                column: "CategoryId"
            );

            // 9. Restore foreign key relationship
            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Categories_CategoryId",
                table: "Tickets",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Categories_CategoryId",
                table: "Tickets"
            );

            migrationBuilder.DropIndex(
                name: "IX_Tickets_CategoryId",
                table: "Tickets"
            );

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories"
            );

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Tickets"
            );

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Categories"
            );

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0
            )
            .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CategoryId",
                table: "Tickets",
                column: "CategoryId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Categories_CategoryId",
                table: "Tickets",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );
        }
    }
}
