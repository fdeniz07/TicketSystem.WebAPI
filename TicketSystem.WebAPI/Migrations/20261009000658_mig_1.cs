using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class mig_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "DeleteAt",
                table: "Users",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Tickets",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "DeleteAt",
                table: "Tickets",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Tickets",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "TicketReplies",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "DeleteAt",
                table: "TicketReplies",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "TicketReplies",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "TicketRatings",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "DeleteAt",
                table: "TicketRatings",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "TicketRatings",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Users",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Users",
                newName: "DeleteAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Tickets",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Tickets",
                newName: "DeleteAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Tickets",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "TicketReplies",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "TicketReplies",
                newName: "DeleteAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TicketReplies",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "TicketRatings",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "TicketRatings",
                newName: "DeleteAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TicketRatings",
                newName: "CreateAt");
        }
    }
}
