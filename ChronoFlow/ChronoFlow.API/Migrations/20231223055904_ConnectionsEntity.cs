using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronoFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class ConnectionsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "TemplateId",
                table: "Events",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Events_TemplateId",
                table: "Events",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserEmail",
                table: "Events",
                column: "UserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Templates_TemplateId",
                table: "Events",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_UserEmail",
                table: "Events",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Templates_TemplateId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_UserEmail",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_TemplateId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_UserEmail",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Events");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
