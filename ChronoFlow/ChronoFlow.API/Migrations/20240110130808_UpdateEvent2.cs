using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChronoFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEvent2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Templates_TemplateEntityId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_UserEntityEmail",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_UserEntityEmail",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UserEntityEmail",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "TemplateEntityId",
                table: "Events",
                newName: "TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_TemplateEntityId",
                table: "Events",
                newName: "IX_Events_TemplateId");

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                table: "Events",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserEmail",
                table: "Events",
                column: "UserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Templates_TemplateId",
                table: "Events",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_UserEmail",
                table: "Events",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email");
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
                name: "IX_Events_UserEmail",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "TemplateId",
                table: "Events",
                newName: "TemplateEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_TemplateId",
                table: "Events",
                newName: "IX_Events_TemplateEntityId");

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEntityEmail",
                table: "Events",
                type: "text",
                nullable: true);

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
    }
}
