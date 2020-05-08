using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTracker.Data.Migrations
{
    public partial class removelist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketGroup_TicketGroupId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TicketGroupId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketGroupId",
                table: "Tickets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketGroupId",
                table: "Tickets",
                type: "int",
                nullable: true);

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
    }
}
