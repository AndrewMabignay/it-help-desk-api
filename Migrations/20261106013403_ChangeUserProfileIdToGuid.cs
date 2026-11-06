
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskSystem.Migrations
{
    public partial class ChangeUserProfileIdToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Remove the existing UserProfile primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles"
            );

            // 2. Drop the old INT Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserProfiles"
            );

            // 3. Recreate UserProfile Id as GUID
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()"
            );

            // 4. Restore UserProfile primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles",
                column: "Id"
            );

            // 5. Update ProfilePictureUrl column length
            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "UserProfiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true
            );

            // 6. Update PhoneNumber column length
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "UserProfiles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Remove the existing UserProfile primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles"
            );

            // 2. Drop the GUID Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserProfiles"
            );

            // 3. Restore the old INT Id column
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserProfiles",
                type: "int",
                nullable: false
            )
            .Annotation("SqlServer:Identity", "1, 1");

            // 4. Restore UserProfile primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles",
                column: "Id"
            );

            // 5. Restore ProfilePictureUrl column length
            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true
            );

            // 6. Restore PhoneNumber column length
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true
            );
        }
    }
}
