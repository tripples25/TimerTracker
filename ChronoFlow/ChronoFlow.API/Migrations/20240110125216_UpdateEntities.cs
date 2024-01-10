﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronoFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<Guid>(
                name: "TemplateId",
                table: "Events",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "TemplateEntityId",
                table: "Events",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEntityEmail",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_TemplateEntityId",
                table: "Events",
                column: "TemplateEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserEntityEmail",
                table: "Events",
                column: "UserEntityEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Templates_TemplateEntityId",
                table: "Events",
                column: "TemplateEntityId",
                principalTable: "Templates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_UserEntityEmail",
                table: "Events",
                column: "UserEntityEmail",
                principalTable: "Users",
                principalColumn: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Templates_TemplateEntityId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_UserEntityEmail",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_TemplateEntityId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_UserEntityEmail",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TemplateEntityId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UserEntityEmail",
                table: "Events");

            migrationBuilder.AlterColumn<Guid>(
                name: "TemplateId",
                table: "Events",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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
    }
}
