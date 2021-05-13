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
                name: "Hotels",
                columns: table => new
                {
                    HotelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotelPreviewPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotelName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotelDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.HotelID);
                });

            migrationBuilder.CreateTable(
                name: "HotelPictures",
                columns: table => new
                {
                    PictureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelID = table.Column<int>(type: "int", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelPictures", x => x.PictureID);
                    table.ForeignKey(
                        name: "FK_HotelPictures_Hotels_HotelID",
                        column: x => x.HotelID,
                        principalTable: "Hotels",
                        principalColumn: "HotelID");
                });

            migrationBuilder.CreateTable(
                name: "HotelRooms",
                columns: table => new
                {
                    RoomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelID = table.Column<int>(type: "int", nullable: false),
                    HotelRoomNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRooms", x => x.RoomID);
                    table.ForeignKey(
                        name: "FK_HotelRooms_Hotels_HotelID",
                        column: x => x.HotelID,
                        principalTable: "Hotels",
                        principalColumn: "HotelID");
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    OfferID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelID = table.Column<int>(type: "int", nullable: false),
                    OfferTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferPreviewPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CostPerChild = table.Column<double>(type: "float", nullable: false),
                    CostPerAdult = table.Column<double>(type: "float", nullable: false),
                    MaxGuests = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.OfferID);
                    table.ForeignKey(
                        name: "FK_Offers_Hotels_HotelID",
                        column: x => x.HotelID,
                        principalTable: "Hotels",
                        principalColumn: "HotelID");
                });

            migrationBuilder.CreateTable(
                name: "ClientReservations",
                columns: table => new
                {
                    ReservationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomID = table.Column<int>(type: "int", nullable: true),
                    ClientID = table.Column<int>(type: "int", nullable: true),
                    ReviewID = table.Column<int>(type: "int", nullable: true),
                    HotelID = table.Column<int>(type: "int", nullable: true),
                    OfferID = table.Column<int>(type: "int", nullable: true),
                    FromTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfChildren = table.Column<int>(type: "int", nullable: false),
                    NumberOfAdults = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_ClientReservations_HotelRooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "HotelRooms",
                        principalColumn: "RoomID");
                    table.ForeignKey(
                        name: "FK_ClientReservations_Hotels_HotelID",
                        column: x => x.HotelID,
                        principalTable: "Hotels",
                        principalColumn: "HotelID");
                    table.ForeignKey(
                        name: "FK_ClientReservations_Offers_OfferID",
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
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ClientReviews",
                columns: table => new
                {
                    ReviewID = table.Column<int>(type: "int", nullable: false),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    OfferID = table.Column<int>(type: "int", nullable: false),
                    HotelID = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<long>(type: "bigint", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientReviews", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK_ClientReviews_ClientReservations_ReviewID",
                        column: x => x.ReviewID,
                        principalTable: "ClientReservations",
                        principalColumn: "ReservationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientReviews_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ClientID");
                    table.ForeignKey(
                        name: "FK_ClientReviews_Hotels_HotelID",
                        column: x => x.HotelID,
                        principalTable: "Hotels",
                        principalColumn: "HotelID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientReviews_Offers_OfferID",
                        column: x => x.OfferID,
                        principalTable: "Offers",
                        principalColumn: "OfferID");
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ClientID", "Email", "Name", "Password", "Surname", "Username" },
                values: new object[,]
                {
                    { 1, "TestEmail1", "TestName1", "TestPassword1", "TestSurname1", "TestUsername1" },
                    { 2, "TestEmail2", "TestName2", "TestPassword2", "TestSurname2", "TestUsername2" },
                    { 3, "TestEmail3", "TestName3", "TestPassword3", "TestSurname3", "TestUsername3" }
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "HotelID", "AccessToken", "City", "Country", "HotelDescription", "HotelName", "HotelPreviewPicture" },
                values: new object[,]
                {
                    { 1, "TestAccessToken1", "TestCity1", "TestCountry1", "TestHotelDesc1", "TestHotelName1", "TestHotelPreviewPicture1" },
                    { 2, "TestAccessToken2", "TestCity2", "TestCountry2", "TestHotelDesc2", "TestHotelName2", "TestHotelPreviewPicture2" },
                    { 3, "TestAccessToken3", "TestCity3", "TestCountry3", "TestHotelDesc3", "TestHotelName3", "TestHotelPreviewPicture3" }
                });

            migrationBuilder.InsertData(
                table: "HotelPictures",
                columns: new[] { "PictureID", "HotelID", "Picture" },
                values: new object[,]
                {
                    { 1, 2, "TestPicture1" },
                    { 2, 3, "TestPicture2" },
                    { 3, 3, "TestPicture3" }
                });

            migrationBuilder.InsertData(
                table: "HotelRooms",
                columns: new[] { "RoomID", "HotelID", "HotelRoomNumber", "IsActive" },
                values: new object[,]
                {
                    { 1, 2, "TestHotelRoomNumber1", false },
                    { 2, 3, "TestHotelRoomNumber2", false },
                    { 3, 3, "TestHotelRoomNumber3", false },
                    { 4, 3, "TestHotelRoomNumber4", false }
                });

            migrationBuilder.InsertData(
                table: "Offers",
                columns: new[] { "OfferID", "CostPerAdult", "CostPerChild", "Description", "HotelID", "IsActive", "IsDeleted", "MaxGuests", "OfferPreviewPicture", "OfferTitle" },
                values: new object[,]
                {
                    { 1, 11.0, 10.0, "TestDescription1", 2, true, false, 1, "TestOfferPreviewPicture1", "TestOfferTitle1" },
                    { 2, 22.0, 20.0, "TestDescription2", 3, true, false, 2, "TestOfferPreviewPicture2", "TestOfferTitle2" },
                    { 3, 33.0, 30.0, "TestDescription3", 3, false, true, 3, "TestOfferPreviewPicture3", "TestOfferTitle3" }
                });

            migrationBuilder.InsertData(
                table: "ClientReservations",
                columns: new[] { "ReservationID", "ClientID", "FromTime", "HotelID", "NumberOfAdults", "NumberOfChildren", "OfferID", "ReviewID", "RoomID", "ToTime" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, 0, 2, 1, 2, new DateTime(2001, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 3, new DateTime(2001, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, 1, 3, 2, 2, new DateTime(3001, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, new DateTime(3001, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, 2, 3, 3, 3, new DateTime(3001, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "OfferHotelRooms",
                columns: new[] { "OfferID", "RoomID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "OfferPictures",
                columns: new[] { "PictureID", "OfferID", "Picture" },
                values: new object[,]
                {
                    { 1, 2, "TestPicture1" },
                    { 2, 3, "TestPicture2" },
                    { 3, 3, "TestPicture3" }
                });

            migrationBuilder.InsertData(
                table: "ClientReviews",
                columns: new[] { "ReviewID", "ClientID", "Content", "HotelID", "OfferID", "Rating", "ReviewDate" },
                values: new object[] { 1, 2, "TestContent1", 2, 2, 1L, new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "ClientReviews",
                columns: new[] { "ReviewID", "ClientID", "Content", "HotelID", "OfferID", "Rating", "ReviewDate" },
                values: new object[] { 2, 3, "TestContent2", 3, 3, 2L, new DateTime(2001, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "ClientReviews",
                columns: new[] { "ReviewID", "ClientID", "Content", "HotelID", "OfferID", "Rating", "ReviewDate" },
                values: new object[] { 3, 3, "TestContent3", 3, 3, 3L, new DateTime(2001, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_ClientReservations_ClientID",
                table: "ClientReservations",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientReservations_HotelID",
                table: "ClientReservations",
                column: "HotelID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientReservations_OfferID",
                table: "ClientReservations",
                column: "OfferID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientReservations_RoomID",
                table: "ClientReservations",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientReviews_ClientID",
                table: "ClientReviews",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientReviews_HotelID",
                table: "ClientReviews",
                column: "HotelID");

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
                name: "ClientReviews");

            migrationBuilder.DropTable(
                name: "HotelPictures");

            migrationBuilder.DropTable(
                name: "OfferHotelRooms");

            migrationBuilder.DropTable(
                name: "OfferPictures");

            migrationBuilder.DropTable(
                name: "ClientReservations");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "HotelRooms");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Hotels");
        }
    }
}
