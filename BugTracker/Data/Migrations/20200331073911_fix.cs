using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTracker.Data.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketGroupId",
                table: "Tickets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TicketGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: false),
                    author = table.Column<string>(nullable: false),
                    dateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketGroup", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketGroupId",
                table: "Tickets",
                column: "TicketGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketGroup_TicketGroupId",
                table: "Tickets",
                column: "TicketGroupId",
                principalTable: "TicketGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketGroup_TicketGroupId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "TicketGroup");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TicketGroupId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketGroupId",
                table: "Tickets");
        }
    }
}
