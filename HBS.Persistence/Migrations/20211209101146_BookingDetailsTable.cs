using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HBS.Persistence.Migrations
{
    public partial class BookingDetailsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomBookings_Rooms_RoomIdId",
                table: "RoomBookings");

            migrationBuilder.RenameColumn(
                name: "RoomIdId",
                table: "RoomBookings",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomBookings_RoomIdId",
                table: "RoomBookings",
                newName: "IX_RoomBookings_RoomId");

            migrationBuilder.CreateTable(
                name: "BookingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomBookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingDetails_RoomBookings_RoomBookingId",
                        column: x => x.RoomBookingId,
                        principalTable: "RoomBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_RoomBookingId",
                table: "BookingDetails",
                column: "RoomBookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomBookings_Rooms_RoomId",
                table: "RoomBookings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomBookings_Rooms_RoomId",
                table: "RoomBookings");

            migrationBuilder.DropTable(
                name: "BookingDetails");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "RoomBookings",
                newName: "RoomIdId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomBookings_RoomId",
                table: "RoomBookings",
                newName: "IX_RoomBookings_RoomIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomBookings_Rooms_RoomIdId",
                table: "RoomBookings",
                column: "RoomIdId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
