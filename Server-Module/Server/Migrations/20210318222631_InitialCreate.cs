using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientID);
                });

            migrationBuilder.CreateTable(
                name: "HotelInfos",
                columns: table => new
                {
                    HotelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotelPreviewPicture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    HotelName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotelDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelInfos", x => x.HotelID);
                });

            migrationBuilder.CreateTable(
                name: "HotelPictures",
                columns: table => new
                {
                    PictureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelID = table.Column<int>(type: "int", nullable: false),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelPictures", x => x.PictureID);
                    table.ForeignKey(
                        name: "FK_HotelPictures_HotelInfos_HotelID",
                        column: x => x.HotelID,
                        principalTable: "HotelInfos",
                        principalColumn: "HotelID");
                });

            migrationBuilder.CreateTable(
                name: "HotelRooms",
                columns: table => new
                {
                    RoomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelID = table.Column<int>(type: "int", nullable: false),
                    HotelRoomNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRooms", x => x.RoomID);
                    table.ForeignKey(
                        name: "FK_HotelRooms_HotelInfos_HotelID",
                        column: x => x.HotelID,
                        principalTable: "HotelInfos",
                        principalColumn: "HotelID");
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    OfferID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferPreviewPicture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CostPerChild = table.Column<double>(type: "float", nullable: false),
                    CostPerAdult = table.Column<double>(type: "float", nullable: false),
                    MaxGuests = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.OfferID);
                    table.ForeignKey(
                        name: "FK_Offers_HotelInfos_HotelID",
                        column: x => x.HotelID,
                        principalTable: "HotelInfos",
                        principalColumn: "HotelID");
                });

            migrationBuilder.CreateTable(
                name: "ClientReservations",
                columns: table => new
                {
                    ReservationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    HotelID = table.Column<int>(type: "int", nullable: false),
                    OfferID = table.Column<int>(type: "int", nullable: false),
                    FromTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfChildren = table.Column<long>(type: "bigint", nullable: false),
                    NumberOfAdults = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientReservations", x => x.ReservationID);
                    table.ForeignKey(
                        name: "FK_ClientReservations_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ClientID");
                    table.ForeignKey(
                        name: "FK_ClientReservations_HotelInfos_HotelID",
                        column: x => x.HotelID,
                        principalTable: "HotelInfos",
                        principalColumn: "HotelID");
                    table.ForeignKey(
                        name: "FK_ClientReservations_HotelRooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "HotelRooms",
                        principalColumn: "RoomID");
                    table.ForeignKey(
                        name: "FK_ClientReservations_Offers_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Offers",
                        principalColumn: "OfferID");
                });

            migrationBuilder.CreateTable(
                name: "ClientReviews",
                columns: table => new
                {
                    ReviewID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    OfferID = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<long>(type: "bigint", nullable: false),
                    ReviewData = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientReviews", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK_ClientReviews_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ClientID");
                    table.ForeignKey(
                        name: "FK_ClientReviews_Offers_OfferID",
                        column: x => x.OfferID,
                        principalTable: "Offers",
                        principalColumn: "OfferID");
                });

            migrationBuilder.CreateTable(
                name: "OfferHotelRooms",
                columns: table => new
                {
                    OfferID = table.Column<int>(type: "int", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferHotelRooms", x => new { x.OfferID, x.RoomID });
                    table.ForeignKey(
                        name: "FK_OfferHotelRooms_HotelRooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "HotelRooms",
                        principalColumn: "RoomID");
                    table.ForeignKey(
                        name: "FK_OfferHotelRooms_Offers_OfferID",
                        column: x => x.OfferID,
                        principalTable: "Offers",
                        principalColumn: "OfferID");
                });

            migrationBuilder.CreateTable(
                name: "OfferPictures",
                columns: table => new
                {
                    PictureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferID = table.Column<int>(type: "int", nullable: false),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferPictures", x => x.PictureID);
                    table.ForeignKey(
                        name: "FK_OfferPictures_Offers_OfferID",
                        column: x => x.OfferID,
                        principalTable: "Offers",
                        principalColumn: "OfferID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientReservations_ClientID",
                table: "ClientReservations",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientReservations_HotelID",
                table: "ClientReservations",
                column: "HotelID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientReservations_RoomID",
                table: "ClientReservations",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientReviews_ClientID",
                table: "ClientReviews",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientReviews_OfferID",
                table: "ClientReviews",
                column: "OfferID");

            migrationBuilder.CreateIndex(
                name: "IX_HotelPictures_HotelID",
                table: "HotelPictures",
                column: "HotelID");

            migrationBuilder.CreateIndex(
                name: "IX_HotelRooms_HotelID",
                table: "HotelRooms",
                column: "HotelID");

            migrationBuilder.CreateIndex(
                name: "IX_OfferHotelRooms_RoomID",
                table: "OfferHotelRooms",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_OfferPictures_OfferID",
                table: "OfferPictures",
                column: "OfferID");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_HotelID",
                table: "Offers",
                column: "HotelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientReservations");

            migrationBuilder.DropTable(
                name: "ClientReviews");

            migrationBuilder.DropTable(
                name: "HotelPictures");

            migrationBuilder.DropTable(
                name: "OfferHotelRooms");

            migrationBuilder.DropTable(
                name: "OfferPictures");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "HotelRooms");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "HotelInfos");
        }
    }
}
