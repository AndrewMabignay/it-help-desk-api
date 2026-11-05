
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskSystem.Migrations
{
    public partial class ChangeDepartmentIdToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Remove the existing Department foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Departments_DepartmentId",
                table: "UserProfiles"
            );

            // 2. Remove the existing DepartmentId index
            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_DepartmentId",
                table: "UserProfiles"
            );

            // 3. Remove the Department primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments"
            );

            // 4. Drop the old INT columns
            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "UserProfiles"
            );

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Departments"
            );

            // 5. Recreate Department Id as GUID
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()"
            );

            // 6. Recreate UserProfile DepartmentId as nullable GUID
            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "UserProfiles",
                type: "uniqueidentifier",
                nullable: true
            );

            // 7. Restore Department primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "Id"
            );

            // 8. Restore DepartmentId index
            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_DepartmentId",
                table: "UserProfiles",
                column: "DepartmentId"
            );

            // 9. Restore foreign key relationship
            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Departments_DepartmentId",
                table: "UserProfiles",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Departments_DepartmentId",
                table: "UserProfiles"
            );

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_DepartmentId",
                table: "UserProfiles"
            );

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments"
            );

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "UserProfiles"
            );

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Departments"
            );

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0
            )
            .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "UserProfiles",
                type: "int",
                nullable: true
            );

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "Id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_DepartmentId",
                table: "UserProfiles",
                column: "DepartmentId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Departments_DepartmentId",
                table: "UserProfiles",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull
            );
        }
    }
}
